using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public TMP_InputField memorySizeInput;
    public TMP_InputField processCountInput;
    public TMP_InputField sequenceLengthInput;
    public TMP_InputField minProcessSizeInput;
    public TMP_InputField maxProcessSizeInput;
    public TMP_InputField zoningLookbackInput;
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
            { minProcessSizeInput, true },
            { maxProcessSizeInput, true },
            { zoningLookbackInput, true },
            { simulationSpeedInput, true }
        };

        memorySizeInput.onValueChanged.AddListener(s => OnMemorySizeUpdate(s));
        processCountInput.onValueChanged.AddListener(s => OnProcessCountUpdate(s));
        sequenceLengthInput.onValueChanged.AddListener(s => OnSequenceLengthUpdate(s));
        minProcessSizeInput.onValueChanged.AddListener(s => OnMinProcessSizeUpdate(s));
        maxProcessSizeInput.onValueChanged.AddListener(s => OnMaxProcessSizeUpdate(s));
        zoningLookbackInput.onValueChanged.AddListener(s => OnZoningLookbackUpdate(s));
        simulationSpeedInput.onValueChanged.AddListener(s => OnSimulationSpeedUpdate(s));
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
        minProcessSizeInput.text = currentSettings.minProcessSize.ToString();
        maxProcessSizeInput.text = currentSettings.maxProcessSize.ToString();
        zoningLookbackInput.text = currentSettings.zoningLookback.ToString();
        simulationSpeedInput.text = currentSettings.simulationSpeed.ToString();
    }

    private void ApplySettings()
    {
        bool newSequence =
            currentSettings.processCount != SimulationManager.Instance.simulationSettings.processCount ||
            currentSettings.sequenceLength != SimulationManager.Instance.simulationSettings.sequenceLength ||
            currentSettings.minProcessSize != SimulationManager.Instance.simulationSettings.minProcessSize ||
            currentSettings.maxProcessSize != SimulationManager.Instance.simulationSettings.maxProcessSize;
        bool rerunSimulation = newSequence ||
            currentSettings.memorySize != SimulationManager.Instance.simulationSettings.memorySize ||
            currentSettings.zoningLookback != SimulationManager.Instance.simulationSettings.zoningLookback;

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

    private void OnMinProcessSizeUpdate(string input, bool sub = false)
    {
        if (int.TryParse(input, out int minSize))
        {
            if (minSize < 0 || minSize > currentSettings.maxProcessSize)
                MarkInvalid(minProcessSizeInput);
            else
            {
                MarkNormal(minProcessSizeInput);
                currentSettings.minProcessSize = minSize;
            }
        }
        else
        {
            MarkInvalid(minProcessSizeInput);
        }

        if (!sub)
            OnMaxProcessSizeUpdate(maxProcessSizeInput.text, true);
    }

    private void OnMaxProcessSizeUpdate(string input, bool sub = false)
    {
        if (int.TryParse(input, out int maxSize))
        {
            if (maxSize < 0 || maxSize < currentSettings.maxProcessSize)
                MarkInvalid(maxProcessSizeInput);
            else
            {
                MarkNormal(maxProcessSizeInput);
                currentSettings.maxProcessSize = maxSize;
            }
        }
        else
        {
            MarkInvalid(maxProcessSizeInput);
        }

        if (!sub)
            OnMinProcessSizeUpdate(minProcessSizeInput.text, true);
    }

    private void OnZoningLookbackUpdate(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < 0)
                MarkInvalid(zoningLookbackInput);
            else
            {
                MarkNormal(zoningLookbackInput);
                currentSettings.zoningLookback = value;
            }

        }
        else
        {
            MarkInvalid(zoningLookbackInput);
        }
    }

    private void OnSimulationSpeedUpdate(string input)
    {
        if (int.TryParse(input, out int value))
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
