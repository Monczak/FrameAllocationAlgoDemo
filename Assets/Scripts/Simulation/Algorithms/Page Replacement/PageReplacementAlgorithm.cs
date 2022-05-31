using System;
using System.Collections.Generic;

public abstract class PageReplacementAlgorithm
{
    public abstract string AlgorithmName { get; }
    public abstract PageReplacementAlgorithmType AlgorithmType { get; }

    public FrameAllocator frameAllocator;

    protected MemoryUnitState currentState;
    protected Dictionary<Process, Dictionary<int, int>> pageLocations;
    private Dictionary<Process, int> framesUsed;

    protected int pagesOccupied;

    public int PageFaults { get; private set; }

    public PageReplacementAlgorithm(FrameAllocator algorithm)
    {
        frameAllocator = algorithm;
    }

    public IEnumerator<MemoryUnitState> Run(Queue<Request> requests, SimulationSettings cachedSettings)
    {
        currentState = new MemoryUnitState()
        {
            Pages = new MemoryPage[cachedSettings.memorySize]
        };

        pageLocations = new Dictionary<Process, Dictionary<int, int>>();
        foreach (Process p in SimulationManager.Instance.processes)
        {
            pageLocations.Add(p, new Dictionary<int, int>());
            for (int i = 0; i < currentState.MemorySize; i++)
            {
                pageLocations[p].Add(i, MemoryPage.NullPage.pageId);
            }
        }

        for (int i = 0; i < currentState.MemorySize; i++)
        {
            currentState.Pages[i] = MemoryPage.NullPage;
        }

        framesUsed = new Dictionary<Process, int>();
        foreach (Process p in SimulationManager.Instance.processes)
        {
            framesUsed.Add(p, 0);
        }

        PageFaults = 0;
        pagesOccupied = 0;

        Setup(requests);
        frameAllocator.Setup(cachedSettings);

        Random random = new Random();

        while (requests.Count > 0)
        {
            Request request = requests.Dequeue();

            Tick(request);
            frameAllocator.Update(request);

            bool hadPageFault = false;

            if (pagesOccupied < currentState.MemorySize && framesUsed[request.process] < frameAllocator.GetAvailableFrames(request.process))
            {
                if (!IsPageInMemory(request))
                {
                    PageFaults++;

                    ReplacePage(pagesOccupied, request);
                    framesUsed[request.process]++;

                    pagesOccupied++;

                    hadPageFault = true;
                }
            }
            else
            {
                if (!IsPageInMemory(request))
                {
                    PageFaults++;

                    int pageToReplace = HandlePageFault(request);
                    if (pageToReplace == MemoryPage.NullPage.pageId)    // Out of memory
                    {
                        int randomPage = random.Next(cachedSettings.memorySize);
                        Process pageProcess = currentState.Pages[randomPage].process;

                        framesUsed[pageProcess]--;
                        framesUsed[request.process]++;

                        ReplacePage(randomPage, request);
                    }
                    else
                    {
                        ReplacePage(pageToReplace, request);
                    }

                    hadPageFault = true;
                }
            }

            LateTick(request);
            frameAllocator.LateUpdate(request, hadPageFault);

            MemoryUnitState newState = new MemoryUnitState
            {
                Pages = new MemoryPage[SimulationManager.Instance.simulationSettings.memorySize],
                PageFaults = PageFaults,
            };
            currentState.Pages.CopyTo(newState.Pages, 0);
            yield return newState;
        }
    }

    private void ReplacePage(int pageToReplace, Request request)
    {
        SetPageLocation(request.process, currentState.Pages[pageToReplace].pageId, MemoryPage.NullPage.pageId);

        currentState.Pages[pageToReplace].processId = request.process.id;
        currentState.Pages[pageToReplace].pageId = request.pageId;
        currentState.Pages[pageToReplace].index = request.process.index;
        currentState.Pages[pageToReplace].process = request.process;

        SetPageLocation(request.process, request.pageId, pageToReplace);
    }

    private void SetPageLocation(Process process, int page, int location)
    {
        if (pageLocations[process].ContainsKey(page))
            pageLocations[process][page] = location;
        else
            pageLocations[process].Add(page, location);
    }

    private void ClearPage(Process process, int pageId)
    {
        pageLocations[process][pageId] = MemoryPage.NullPage.pageId;
    }

    protected int GetPageLocation(Process process, int pageId)
    {
        return pageLocations[process][pageId];
    }

    protected bool IsPageInMemory(Request request)
    {
        return pageLocations[request.process].ContainsKey(request.pageId) && pageLocations[request.process][request.pageId] != MemoryPage.NullPage.pageId;
    }

    protected abstract void Setup(Queue<Request> requests);
    protected abstract int HandlePageFault(Request request);
    protected abstract void Tick(Request currentRequest);
    protected abstract void LateTick(Request currentRequest);
}
