using System;
using System.Collections.Generic;

public class EqualAllocator : FrameAllocator
{
    public override string AlgorithmName => "Equal";

    public override FrameAllocatorType AlgorithmType => FrameAllocatorType.Equal;

    public override void Setup(SimulationSettings cachedSettings)
    {
        availableFrames = new Dictionary<Process, int>();
        foreach (Process process in SimulationManager.Instance.processes)
        {
            availableFrames.Add(process, Math.Max(cachedSettings.memorySize / cachedSettings.processCount, 1));
        }
    }

    public override void LateUpdate(Request request, bool hadPageFault)
    {
        // Do nothing
    }

    public override void Update(Request request)
    {
        // Do nothing
    }
}
