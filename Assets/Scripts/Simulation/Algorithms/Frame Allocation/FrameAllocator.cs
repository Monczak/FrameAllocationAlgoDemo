using System.Collections.Generic;

public abstract class FrameAllocator
{
    public abstract string AlgorithmName { get; }
    public abstract FrameAllocatorType AlgorithmType { get; }

    protected Dictionary<Process, int> availableFrames;

    public abstract void Setup(SimulationSettings cachedSettings);
    public abstract void Update(Request request);
    public abstract void LateUpdate(Request request, bool hadPageFault);

    public int GetAvailableFrames(Process process) => availableFrames[process];
}
