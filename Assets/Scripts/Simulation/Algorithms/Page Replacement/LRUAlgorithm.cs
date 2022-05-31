using System.Collections.Generic;

public class LRUAlgorithm : PageReplacementAlgorithm
{
    public override string AlgorithmName => "LRU";
    public override PageReplacementAlgorithmType AlgorithmType => PageReplacementAlgorithmType.LeastRecentlyUsed;

    private Dictionary<Process, Dictionary<int, int>> processUseTimes;
    private int currentTime;

    public LRUAlgorithm(FrameAllocator algorithm) : base(algorithm)
    {
    }

    protected override void Setup(Queue<Request> requests)
    {
        processUseTimes = new Dictionary<Process, Dictionary<int, int>>();
        currentTime = 0;
    }

    protected override int HandlePageFault(Request request)
    {
        int pageId = 0, minTime = int.MaxValue;
        foreach (KeyValuePair<int, int> pair in processUseTimes[request.process])
        {
            if (pair.Value < minTime)
            {
                minTime = pair.Value;
                pageId = pair.Key;
            }
        }
        processUseTimes[request.process].Remove(pageId);

        return GetPageLocation(request.process, pageId);
    }

    protected override void Tick(Request currentRequest)
    {
        if (!processUseTimes.ContainsKey(currentRequest.process))
            processUseTimes.Add(currentRequest.process, new Dictionary<int, int>());

        if (processUseTimes[currentRequest.process].ContainsKey(currentRequest.pageId))
            processUseTimes[currentRequest.process][currentRequest.pageId] = currentTime;
        else
            processUseTimes[currentRequest.process].Add(currentRequest.pageId, currentTime);
        currentTime++;
    }

    protected override void LateTick(Request currentRequest)
    {
        // Do nothing
    }
}
