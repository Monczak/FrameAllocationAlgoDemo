using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TogglableButtonText : MonoBehaviour
{
    private bool active;

    private Button button;
    public TMP_Text text;

    private Color currentColor;

    public Color activeColor, inactiveColor;
    public float colorSmoothing;

    private void Awake()
    {
        button = GetComponent<Button>();

        currentColor = button.interactable ? activeColor : inactiveColor;
    }

    private void Update()
    {
        currentColor = Color.Lerp(currentColor, active ? activeColor : inactiveColor, colorSmoothing * Time.deltaTime);
        text.color = currentColor;
    }

    public void SetActive(bool active)
    {
        this.active = active;
        button.interactable = active;
    }
}
