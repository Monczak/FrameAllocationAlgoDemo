using System.Collections.Generic;

public class SecondChanceAlgorithm : PageReplacementAlgorithm
{
    public override string AlgorithmName => "SC";
    public override AlgorithmType AlgorithmType => AlgorithmType.SecondChance;

    private Queue<int> currentProcessIds;
    private Dictionary<int, bool> referencedBits;

    protected override void Setup(Queue<Process> processes)
    {
        currentProcessIds = new Queue<int>();
        referencedBits = new Dictionary<int, bool>();
    }

    protected override int HandlePageFault(Process process)
    {
        while (true)
        {
            int currentProcessId = currentProcessIds.Dequeue();
            if (referencedBits[currentProcessId])
            {
                referencedBits[currentProcessId] = false;
                currentProcessIds.Enqueue(currentProcessId);
            }
            else
            {
                currentProcessIds.Enqueue(process.id);
                return GetPageLocation(currentProcessId);
            }
        }

    }

    protected override void Tick(Process currentProcess)
    {
        if (!referencedBits.ContainsKey(currentProcess.id))
        {
            referencedBits.Add(currentProcess.id, true);
            if (currentProcessIds.Count < currentState.MemorySize)
                currentProcessIds.Enqueue(currentProcess.id);
        }
        else
            referencedBits[currentProcess.id] = true;
    }

    protected override void LateTick(Process currentProcess)
    {
        //string message = "";
        //foreach (int processId in currentProcessIds)
        //    message += $"{processId} ";
        //message += "| ";
        //foreach (var pair in referencedBits)
        //    message += $"({pair.Key} {pair.Value}) ";
        //Debug.Log(message);
    }
}
