using System.Collections.Generic;

public class ZoningAllocator : FrameAllocator
{
    public override string AlgorithmName => "Zoning";

    public override FrameAllocatorType AlgorithmType => FrameAllocatorType.Zoning;

    private int lookback;

    private Dictionary<Process, Queue<int>> recentLookups;

    public override void Setup(SimulationSettings cachedSettings)
    {
        availableFrames = new Dictionary<Process, int>();
        recentLookups = new Dictionary<Process, Queue<int>>();
        foreach (Process process in SimulationManager.Instance.processes)
        {
            availableFrames.Add(process, 1);
            recentLookups.Add(process, new Queue<int>());
        }

        lookback = cachedSettings.zoningLookback;
    }

    public override void LateUpdate(Request request, bool hadPageFault)
    {
        // Do nothing
    }

    public override void Update(Request request)
    {
        AddLookup(request);
        ManageAvailableFrames();
    }

    private void AddLookup(Request request)
    {
        Queue<int> queue = recentLookups[request.process];
        if (queue.Count >= lookback)
            queue.Dequeue();

        queue.Enqueue(request.pageId);
    }

    private void ManageAvailableFrames()
    {
        foreach (KeyValuePair<Process, Queue<int>> pair in recentLookups)
        {
            availableFrames[pair.Key] = new HashSet<int>(pair.Value).Count;
        }
    }
}
