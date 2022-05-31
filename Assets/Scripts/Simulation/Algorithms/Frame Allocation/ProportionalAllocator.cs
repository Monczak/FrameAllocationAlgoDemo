using System;
using System.Collections.Generic;

public class ProportionalAllocator : FrameAllocator
{
    public override string AlgorithmName => "Proportional";

    public override FrameAllocatorType AlgorithmType => FrameAllocatorType.Proportional;

    public override void Setup(SimulationSettings cachedSettings)
    {
        availableFrames = new Dictionary<Process, int>();

        int totalSize = 0;
        foreach (Process process in SimulationManager.Instance.processes)
            totalSize += process.size;

        foreach (Process process in SimulationManager.Instance.processes)
        {
            availableFrames.Add(process, Math.Max((int)((float)process.size / totalSize * cachedSettings.memorySize), 1));
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
