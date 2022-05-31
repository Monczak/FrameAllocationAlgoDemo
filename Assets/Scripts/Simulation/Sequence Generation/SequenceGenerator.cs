using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SequenceGenerator
{
    public static (Queue<Request>, List<Process>) GenerateSequence()
    {
        Queue<Request> queue = new Queue<Request>();
        List<Process> processes = new List<Process>();

        for (int i = 0; i < SimulationManager.Instance.simulationSettings.processCount; i++)
        {
            processes.Add(new Process
            {
                id = i,
                index = i,
                size = Random.Range(SimulationManager.Instance.simulationSettings.minProcessSize, SimulationManager.Instance.simulationSettings.maxProcessSize),
            });
        }

        int n = 0;
        while (n < SimulationManager.Instance.simulationSettings.sequenceLength)
        {
            Process process = processes[Random.Range(0, processes.Count)];
            int requests = Random.Range(0, process.size);
            for (int i = 0; i < requests; i++)
            {
                queue.Enqueue(new Request
                {
                    process = process,
                    pageId = Random.Range(0, process.size)
                });
                n++;

                if (n >= SimulationManager.Instance.simulationSettings.sequenceLength)
                    break;
            }
        }

        return (queue, processes);
    }

    private static void Shuffle(List<int> list, float shuffleRatio)
    {
        int n = (int)(shuffleRatio * list.Count);
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);

            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
