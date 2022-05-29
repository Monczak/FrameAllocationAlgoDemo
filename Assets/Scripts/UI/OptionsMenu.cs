using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public TMP_InputField memorySizeInput;
    public TMP_InputField processCountInput;
    public TMP_InputField sequenceLengthInput;
    public MultiSelector sequencePatternInput;
    public TMP_InputField shuffleRatioInput;
    public TMP_InputField simulationSpeedInput;

    public TogglableButtonText applyButton;

    public Color normalInputColor, invalidInputColor;
    public float colorChangeRate;

    private Dictionary<TMP_InputField, bool> inputValidity;

    private SimulationSettings currentSettings;

    public float minSimulationSpeed, maxSimulationSpeed;

    private bool valid;

    private void Awake()
    {
        inputValidity = new Dictionary<TMP_InputField, bool>
        {
            { memorySizeInput, true },
            { processCountInput, true },
            { sequenceLengthInput, true },
            { shuffleRatioInput, true },
            { simulationSpeedInput, true }
        };

        memorySizeInput.onValueChanged.AddListener(s => OnMemorySizeUpdate(s));
        processCountInput.onValueChanged.AddListener(s => OnProcessCountUpdate(s));
        sequenceLengthInput.onValueChanged.AddListener(s => OnSequenceLengthUpdate(s));
        sequencePatternInput.OnChanged += OnSequencePatternChanged;
        shuffleRatioInput.onValueChanged.AddListener(s => OnShuffleRatioUpdate(s));
        simulationSpeedInput.onValueChanged.AddListener(s => OnSimulationSpeedUpdate(s));
    }

    private void OnSequencePatternChanged()
    {
        currentSettings.generationPattern = (SequenceGenerationPattern)sequencePatternInput.Selected;
    }

    private void Start()
    {
        UpdateInputs();
    }

    private void Update()
    {
        CheckValidity();
        UpdateColors();
    }

    private void CheckValidity()
    {
        valid = AreInputsValid();
    }

    private void UpdateInputs()
    {
        currentSettings = SimulationManager.Instance.simulationSettings;

        memorySizeInput.text = currentSettings.memorySize.ToString();
        processCountInput.text = currentSettings.processCount.ToString();
        sequenceLengthInput.text = currentSettings.sequenceLength.ToString();
        sequencePatternInput.Selected = (int)currentSettings.generationPattern;
        shuffleRatioInput.text = currentSettings.shuffleRatio.ToString();
        simulationSpeedInput.text = currentSettings.simulationSpeed.ToString();
    }

    private void ApplySettings()
    {
        bool newSequence =
            currentSettings.processCount != SimulationManager.Instance.simulationSettings.processCount ||
            currentSettings.sequenceLength != SimulationManager.Instance.simulationSettings.sequenceLength ||
            currentSettings.generationPattern != SimulationManager.Instance.simulationSettings.generationPattern ||
            currentSettings.shuffleRatio != SimulationManager.Instance.simulationSettings.shuffleRatio;
        bool rerunSimulation = newSequence ||
            currentSettings.memorySize != SimulationManager.Instance.simulationSettings.memorySize;

        SimulationManager.Instance.SetSimulationSettings(currentSettings);

        if (newSequence)
        {
            SimulationManager.Instance.GenerateNewSequence();
        }
        if (rerunSimulation)
        {
            SimulationManager.Instance.RunSimulation();
        }

        SimulationManager.Instance.Rewind();
        Hide();
    }

    public void Show()
    {
        UpdateInputs();
        UIManager.Instance.showOptionsMenu = true;
    }

    public void Hide()
    {
        UIManager.Instance.showOptionsMenu = false;
    }

    private void UpdateColors()
    {
        foreach (KeyValuePair<TMP_InputField, bool> pair in inputValidity)
        {
            pair.Key.textComponent.color = Color.Lerp(pair.Key.textComponent.color, pair.Value ? normalInputColor : invalidInputColor, colorChangeRate * Time.deltaTime);
        }

        applyButton.SetActive(valid);
    }

    private bool AreInputsValid()
    {
        bool valid = true;
        foreach (bool v in inputValidity.Values)
            valid &= v;
        return valid;
    }

    private void OnMemorySizeUpdate(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < 1)
                MarkInvalid(memorySizeInput);
            else
            {
                MarkNormal(memorySizeInput);
                currentSettings.memorySize = value;
            }

        }
        else
        {
            MarkInvalid(memorySizeInput);
        }
    }

    private void OnProcessCountUpdate(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < 1)
                MarkInvalid(processCountInput);
            else
            {
                MarkNormal(processCountInput);
                currentSettings.processCount = value;
            }

        }
        else
        {
            MarkInvalid(processCountInput);
        }
    }

    private void OnSequenceLengthUpdate(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < 0)
                MarkInvalid(sequenceLengthInput);
            else
            {
                MarkNormal(sequenceLengthInput);
                currentSettings.sequenceLength = value;
            }

        }
        else
        {
            MarkInvalid(sequenceLengthInput);
        }
    }

    private void OnShuffleRatioUpdate(string input)
    {
        if (float.TryParse(input, out float value))
        {
            if (value < 0 || value > 1)
                MarkInvalid(shuffleRatioInput);
            else
            {
                MarkNormal(shuffleRatioInput);
                currentSettings.shuffleRatio = value;
            }
        }
        else
        {
            MarkInvalid(shuffleRatioInput);
        }
    }
    
    private void OnSimulationSpeedUpdate(string input)
    {
        if (float.TryParse(input, out float value))
        {
            if (value < minSimulationSpeed || value > maxSimulationSpeed)
                MarkInvalid(simulationSpeedInput);
            else
            {
                MarkNormal(simulationSpeedInput);
                currentSettings.simulationSpeed = value;
            }
        }
        else
        {
            MarkInvalid(simulationSpeedInput);
        }
    }

    private void MarkInvalid(TMP_InputField input)
    {
        inputValidity[input] = false;
    }

    private void MarkNormal(TMP_InputField input)
    {
        inputValidity[input] = true;
    }

    public void OnApply()
    {
        ApplySettings();
    }

    public void OnReturn()
    {
        Hide();
    }
}
