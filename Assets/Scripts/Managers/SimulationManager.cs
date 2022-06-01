using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance { get; private set; }

    public SimulationSettings simulationSettings;

    [Header("Simulation Timing")]
    public float simulationDuration;
    public float currentTime;
    [HideInInspector] public float previousTime;
    public float speed;

    [Header("Simulation Logic")]
    public MemoryUnitManager memoryUnitManager;
    private Queue<Request> currentSequence;
    public List<Process> processes;

    [Header("Comfort Features")]
    public CameraMovement cameraMovement;

    private bool playing = false;
    private float playingSpeed;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        previousTime = currentTime;

        LoadDefaultSettings();
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewSequence();
        RunSimulation();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    public void GenerateNewSequence()
    {
        (currentSequence, processes) = SequenceGenerator.GenerateSequence();
        simulationDuration = currentSequence.Count;
    }

    public void RunSimulation()
    {
        memoryUnitManager.RunSimulations(currentSequence);
    }

    public void SetSimulationSettings(SimulationSettings settings)
    {
        simulationSettings = settings;
    }

    private void LoadDefaultSettings()
    {
        simulationSettings = new SimulationSettings
        {
            memorySize = 100,
            processCount = 10,
            sequenceLength = 500,
            minProcessSize = 1,
            maxProcessSize = 50,
            zoningLookback = 20,
            simulationSpeed = 1f,
        };
    }

    public bool IsPlaying()
    {
        return playing;
    }

    public float GetPlayingSpeed()
    {
        return playingSpeed;
    }

    public void PlayForward()
    {
        if (!playing || playingSpeed < 0)
        {
            playing = true;
            playingSpeed = speed;
        }
        else
        {
            Pause();
        }
    }

    public void PlayBackward()
    {
        if (!playing || playingSpeed > 0)
        {
            playing = true;
            playingSpeed = -speed;
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        playing = false;
        playingSpeed = 0;
    }

    public void Rewind()
    {
        playingSpeed = 0;
        currentTime = 0;
        playing = false;
    }

    public void Forward()
    {
        playingSpeed = 0;
        currentTime = simulationDuration;
        playing = false;
    }

    public void StepForward()
    {
        currentTime++;
    }

    public void StepBack()
    {
        currentTime--;
    }

    private void UpdateTime()
    {
        currentTime += playingSpeed * simulationSettings.simulationSpeed * Time.deltaTime;
        ClampTime();
    }

    private void ClampTime()
    {
        if (currentTime < 0)
        {
            Pause();
            currentTime = 0;
        }
        if (currentTime > simulationDuration)
        {
            Pause();
            currentTime = simulationDuration;
        }
    }
}
