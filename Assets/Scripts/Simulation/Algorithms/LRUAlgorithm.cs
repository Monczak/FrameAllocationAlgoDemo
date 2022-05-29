using System.Collections.Generic;

public class LRUAlgorithm : PageReplacementAlgorithm
{
    public override string AlgorithmName => "LRU";
    public override AlgorithmType AlgorithmType => AlgorithmType.LeastRecentlyUsed;

    private Dictionary<int, int> processUseTimes;
    private int currentTime;

    protected override void Setup(Queue<Process> processes)
    {
        processUseTimes = new Dictionary<int, int>();
        currentTime = 0;
    }

    protected override int HandlePageFault(Process process)
    {
        int processId = 0, minTime = int.MaxValue;
        foreach (KeyValuePair<int, int> pair in processUseTimes)
        {
            if (pair.Value < minTime)
            {
                minTime = pair.Value;
                processId = pair.Key;
            }
        }
        processUseTimes.Remove(processId);

        return GetPageLocation(processId);
    }

    protected override void Tick(Process currentProcess)
    {
        if (processUseTimes.ContainsKey(currentProcess.id))
            processUseTimes[currentProcess.id] = currentTime;
        else
            processUseTimes.Add(currentProcess.id, currentTime);
        currentTime++;
    }

    protected override void LateTick(Process currentProcess)
    {
        // Do nothing
    }
}
