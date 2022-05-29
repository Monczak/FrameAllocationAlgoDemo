using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiSelector : MonoBehaviour
{
    public Color inactiveColor, activeColor;

    private List<Button> buttons;
    private int selected;

    public delegate void OnChangedDelegate();
    public event OnChangedDelegate OnChanged;

    public int Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
            UpdateButtons();
        }
    }

    private void Awake()
    {
        buttons = new List<Button>();
        for (int i = 0; i < transform.childCount; i++)
        {
            buttons.Add(transform.GetChild(i).GetComponent<Button>());
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            int ii = i;
            buttons[i].onClick.AddListener(() => OnButtonPress(ii));
        }

    }

    private void OnButtonPress(int buttonIndex)
    {
        selected = buttonIndex;
        UpdateButtons();
        OnChanged?.Invoke();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponentInChildren<TMP_Text>().color = i == selected ? activeColor : inactiveColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
