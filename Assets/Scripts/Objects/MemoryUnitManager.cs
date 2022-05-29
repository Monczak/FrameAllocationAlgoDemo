using System;
using System.Collections.Generic;
using UnityEngine;

public class MemoryUnitManager : MonoBehaviour
{
    public GameObject memoryUnitPrefab;

    public List<MemoryUnit> memoryUnits;

    public Vector3 unitSize;
    public float margin;

    public HashSet<MemoryUnit> unitsLeft;

    private void Awake()
    {
        memoryUnits = new List<MemoryUnit>();

        CreateMemoryUnits();
        LayoutMemoryUnits();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateMemoryUnits()
    {
        memoryUnits.Clear();

        foreach (AlgorithmType algorithmType in Enum.GetValues(typeof(AlgorithmType)))
        {
            MemoryUnit unit = Instantiate(memoryUnitPrefab, transform).GetComponent<MemoryUnit>();
            memoryUnits.Add(unit);

            PageReplacementAlgorithm algorithm = algorithmType switch
            {
                AlgorithmType.FirstInFirstOut => new FIFOAlgorithm(),
                AlgorithmType.Optimal => new OPTAlgorithm(),
                AlgorithmType.LeastRecentlyUsed => new LRUAlgorithm(),
                AlgorithmType.SecondChance => new SecondChanceAlgorithm(),
                AlgorithmType.Random => new RANDAlgorithm(),
                _ => throw new NotImplementedException(),
            };

            unit.SetProperties(algorithm, unitSize);

            unit.OnSimulationFinished += OnUnitSimulationFinished;
        }
    }

    public void LayoutMemoryUnits()
    {
        float totalWidth = memoryUnits.Count * (unitSize.x + margin) - margin;

        int i = 0;
        for (float x = -totalWidth / 2; x <= totalWidth / 2; x += unitSize.x + margin)
        {
            memoryUnits[i].transform.position = new Vector3(x + unitSize.x / 2, 0, 0);
            i++;
        }
    }

    public void RunSimulations(Queue<Process> sequence)
    {
        unitsLeft = new HashSet<MemoryUnit>(memoryUnits);

        foreach (MemoryUnit unit in memoryUnits)
        {
            unit.Simulate(new Queue<Process>(sequence));
        }
    }


    void OnUnitSimulationFinished(MemoryUnit unit)
    {
        Debug.Log($"{unit.algorithmType} finished");
        unitsLeft.Remove(unit);
    }


}
