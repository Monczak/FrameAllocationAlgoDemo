using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SequenceGenerator
{
    public static Queue<Process> GenerateSequence()
    {
        // TODO: Enable generation pattern selection
        ISequenceGenerationPattern generationPattern = SimulationManager.Instance.simulationSettings.generationPattern switch
        {
            SequenceGenerationPattern.Random => new RandomPattern(),
            SequenceGenerationPattern.Simple => new SimplePattern(),
            SequenceGenerationPattern.Triangle => new TrianglePattern(),
            SequenceGenerationPattern.BackAndForth1x => new BackAndForthPattern(SimulationManager.Instance.simulationSettings.memorySize),
            SequenceGenerationPattern.BackAndForth2x => new BackAndForthPattern(SimulationManager.Instance.simulationSettings.memorySize * 2),
            _ => throw new System.NotImplementedException(),
        };

        List<int> indexes = new List<int>();
        for (int i = 0; i < SimulationManager.Instance.simulationSettings.sequenceLength; i++)
            indexes.Add(generationPattern.GetIndex(i));

        Shuffle(indexes, SimulationManager.Instance.simulationSettings.shuffleRatio);

        Queue<Process> queue = new Queue<Process>();    

        for (int i = 0; i < SimulationManager.Instance.simulationSettings.sequenceLength; i++)
        {
            queue.Enqueue(new Process
            {
                id = indexes[i],
                index = i
            });
        }

        return queue;
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
