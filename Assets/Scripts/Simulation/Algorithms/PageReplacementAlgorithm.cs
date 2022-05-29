using System.Collections.Generic;

public abstract class PageReplacementAlgorithm
{
    public abstract string AlgorithmName { get; }
    public abstract AlgorithmType AlgorithmType { get; }

    protected MemoryUnitState currentState;
    protected Dictionary<int, int> pageLocations;

    protected int pagesOccupied;

    public int PageFaults { get; private set; }

    public IEnumerator<MemoryUnitState> Run(Queue<Process> processes)
    {
        currentState = new MemoryUnitState()
        {
            Pages = new MemoryPage[SimulationManager.Instance.simulationSettings.memorySize]
        };
        pageLocations = new Dictionary<int, int>();

        for (int i = 0; i < currentState.MemorySize; i++)
        {
            currentState.Pages[i] = MemoryPage.NullPage;
            pageLocations.Add(i, currentState.Pages[i].processId);
        }

        PageFaults = 0;
        pagesOccupied = 0;

        Setup(processes);

        while (processes.Count > 0)
        {
            Process process = processes.Dequeue();

            Tick(process);

            if (pagesOccupied < currentState.MemorySize)
            {
                if (!IsPageInMemory(process.id))
                {
                    PageFaults++;
                    ReplacePage(pagesOccupied, process);
                    pagesOccupied++;
                }
            }
            else
            {
                if (!IsPageInMemory(process.id))
                {
                    PageFaults++;

                    int pageToReplace = HandlePageFault(process);
                    ReplacePage(pageToReplace, process);
                }
            }

            LateTick(process);

            MemoryUnitState newState = new MemoryUnitState
            {
                Pages = new MemoryPage[SimulationManager.Instance.simulationSettings.memorySize],
                PageFaults = PageFaults,
            };
            currentState.Pages.CopyTo(newState.Pages, 0);
            yield return newState;
        }
    }

    private void ReplacePage(int pageToReplace, Process process)
    {
        SetPageLocation(currentState.Pages[pageToReplace].processId, MemoryPage.NullPage.processId);

        currentState.Pages[pageToReplace].processId = process.id;
        currentState.Pages[pageToReplace].index = process.index;

        SetPageLocation(process.id, pageToReplace);
    }

    private void SetPageLocation(int page, int location)
    {
        if (pageLocations.ContainsKey(location))
            pageLocations[page] = location;
        else
            pageLocations.Add(page, location);
    }

    protected int GetPageLocation(int processId)
    {
        return pageLocations[processId];
    }

    protected bool IsPageInMemory(int processId)
    {
        return pageLocations.ContainsKey(processId) && pageLocations[processId] != MemoryPage.NullPage.processId;
    }

    protected abstract void Setup(Queue<Process> processes);
    protected abstract int HandlePageFault(Process process);
    protected abstract void Tick(Process currentProcess);
    protected abstract void LateTick(Process currentProcess);
}
