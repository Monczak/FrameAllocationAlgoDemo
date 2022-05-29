using System.Collections.Generic;

public class FIFOAlgorithm : PageReplacementAlgorithm
{
    public override string AlgorithmName => "FIFO";
    public override AlgorithmType AlgorithmType => AlgorithmType.FirstInFirstOut;

    private int pagePointer;

    protected override void Setup(Queue<Process> processes)
    {
        pagePointer = -1;
    }

    protected override int HandlePageFault(Process process)
    {
        pagePointer++;
        pagePointer %= currentState.MemorySize;
        return pagePointer;
    }

    protected override void Tick(Process currentProcess)
    {
        // Do nothing
    }

    protected override void LateTick(Process currentProcess)
    {
        // Do nothing
    }
}
