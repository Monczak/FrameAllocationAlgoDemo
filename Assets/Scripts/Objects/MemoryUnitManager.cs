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

        foreach (FrameAllocatorType algorithmType in Enum.GetValues(typeof(FrameAllocatorType)))
        {
            MemoryUnit unit = Instantiate(memoryUnitPrefab, transform).GetComponent<MemoryUnit>();
            memoryUnits.Add(unit);

            FrameAllocator algorithm = algorithmType switch
            {
                FrameAllocatorType.Equal => new EqualAllocator(),
                FrameAllocatorType.Proportional => new ProportionalAllocator(),
                FrameAllocatorType.PageFaultBased => new PageFaultBasedAllocator(),
                FrameAllocatorType.Zoning => new ZoningAllocator(),
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

    public void RunSimulations(Queue<Request> sequence)
    {
        unitsLeft = new HashSet<MemoryUnit>(memoryUnits);

        foreach (MemoryUnit unit in memoryUnits)
        {
            unit.Simulate(new Queue<Request>(sequence));
        }
    }


    void OnUnitSimulationFinished(MemoryUnit unit)
    {
        Debug.Log($"{unit.frameAllocationAlgorithmType} finished");
        unitsLeft.Remove(unit);
    }


}
