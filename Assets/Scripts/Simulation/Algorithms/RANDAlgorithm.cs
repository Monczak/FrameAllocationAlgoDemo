using System;
using System.Collections.Generic;

public class RANDAlgorithm : PageReplacementAlgorithm
{
    public override string AlgorithmName => "RAND";
    public override AlgorithmType AlgorithmType => AlgorithmType.Random;

    private Random random;

    protected override void Setup(Queue<Process> processes)
    {
        random = new Random();
    }

    protected override int HandlePageFault(Process process)
    {
        return random.Next(0, currentState.MemorySize);
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
