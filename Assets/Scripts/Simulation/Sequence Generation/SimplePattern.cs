public class SimplePattern : ISequenceGenerationPattern
{
    public int GetIndex(int i)
    {
        return i % (SimulationManager.Instance.simulationSettings.memorySize * 2);
    }
}
