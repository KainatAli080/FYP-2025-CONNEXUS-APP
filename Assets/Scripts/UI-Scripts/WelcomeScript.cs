using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeScript : MonoBehaviour
{
    [Header("Scripts To Attach")]
    public AppNavigationScript _NavScript;
    public FirebaseFunctions _FirebaseFunctions;

    [Header("Terms And Conditions")]
    [SerializeField] private Toggle termsAndCondition;

    [Header("Toast Message Panel")]
    [SerializeField] private GameObject toastPanel;
    [SerializeField] private CanvasGroup toastMessage;
    [SerializeField] private TextMeshProUGUI toastText;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 0.5f;

    private void displayToast(GameObject toastObject, string textToDisplay)
    {
        CanvasGroup toastMessage = toastObject.GetComponent<CanvasGroup>();
        TextMeshProUGUI canvasGroupText = toastObject.GetComponentInChildren<TextMeshProUGUI>();
        canvasGroupText.text = textToDisplay;


        // Stop any existing tweens on this CanvasGroup
        toastMessage.DOKill();

        // Show the panel (Fade In)
        toastMessage.DOFade(1, fadeDuration)
            .OnComplete(() =>
            {
                // After the duration, fade out
                toastMessage.DOFade(0, fadeDuration).SetDelay(displayDuration);
            });
    }

    public void checkTermsAndConditions()
    {
        if (termsAndCondition.isOn)
        {
            PlayerPrefs.SetInt("Terms And Conditions", 1);
            _NavScript.fromWelcomeP3ToQ1GenderPanel();
        }
        else
        {
            string str = toastText.text;
            toastText.text = "Cannot proceed without agreeing...";
            displayToast(toastPanel, "Cannot proceed without agreeing...");

            toastText.text = str;            
        }
    }
}
