using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterestsPanelToggleFunctionality : MonoBehaviour
{
    [SerializeField] private Toggle topicToggle;
    [SerializeField] private Image toggleCurrentBackground;
    [SerializeField] private TextMeshProUGUI toggleText;

    [SerializeField] private Sprite selectedBackground;
    [SerializeField] private Sprite deselectedBackground;

    private string selectedTextColorHexa = "#FFFFFF";
    private string deselectedTextColorHexa = "#1C7780";

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;

    void Start()
    {
        ColorUtility.TryParseHtmlString(selectedTextColorHexa, out selectedColor);
        ColorUtility.TryParseHtmlString(deselectedTextColorHexa, out deselectedColor);

        topicToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            toggleCurrentBackground.sprite = selectedBackground;
            toggleText.color = selectedColor;
        }
        else
        {
            toggleCurrentBackground.sprite = deselectedBackground;
            toggleText.color = deselectedColor;
        }
    }
}
