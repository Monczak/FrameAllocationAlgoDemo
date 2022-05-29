using System.Collections.Generic;

public class OPTAlgorithm : PageReplacementAlgorithm
{
    public override string AlgorithmName => "OPT";
    public override AlgorithmType AlgorithmType => AlgorithmType.Optimal;

    private List<Process> processList;
    private int sequencePosition;

    protected override void Setup(Queue<Process> processes)
    {
        processList = new List<Process>(processes);
        sequencePosition = -1;
    }

    protected override int HandlePageFault(Process process)
    {
        return FindFurthestPage();
    }

    private int FindFurthestPage()
    {
        Dictionary<int, int> furthestUse = new Dictionary<int, int>();

        foreach (MemoryPage page in currentState.Pages)
        {
            int processId = page.processId;
            bool found = false;
            for (int i = sequencePosition; i < processList.Count; i++)
            {
                if (processList[i].id == processId)
                {
                    found = true;
                    if (furthestUse.ContainsKey(processId))
                        furthestUse[processId] = i;
                    else
                        furthestUse.Add(processId, i);
                }
            }

            if (!found)
                furthestUse.Add(processId, int.MaxValue);
        }

        int furthestProcessId = 0, maxIndex = 0;
        foreach (KeyValuePair<int, int> pair in furthestUse)
        {
            if (pair.Value > maxIndex)
            {
                maxIndex = pair.Value;
                furthestProcessId = pair.Key;
            }
        }

        return GetPageLocation(furthestProcessId);
    }

    protected override void Tick(Process currentProcess)
    {
        sequencePosition++;
    }

    protected override void LateTick(Process currentProcess)
    {
        // Do nothing
    }
}
