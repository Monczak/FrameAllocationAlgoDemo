using System.Collections.Generic;

public class PageFaultBasedAllocator : FrameAllocator
{
    public override string AlgorithmName => "Page Fault Based";

    public override FrameAllocatorType AlgorithmType => FrameAllocatorType.PageFaultBased;

    public override void Setup(SimulationSettings cachedSettings)
    {
        availableFrames = new Dictionary<Process, int>();
        foreach (Process process in SimulationManager.Instance.processes)
        {
            availableFrames.Add(process, 1);
        }
    }

    public override void LateUpdate(Request request, bool hadPageFault)
    {
        if (hadPageFault)
        {
            availableFrames[request.process]++;
        }
    }

    public override void Update(Request request)
    {
        // Do nothing
    }
}
