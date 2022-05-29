using UnityEngine;

public class RandomPattern : ISequenceGenerationPattern
{
    public int GetIndex(int i)
    {
        return Random.Range(0, SimulationManager.Instance.simulationSettings.processCount);
    }
}
