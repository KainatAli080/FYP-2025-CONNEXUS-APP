using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ForgotPasswordScript : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 0.5f;
    [SerializeField] private GameObject toastPanel;
    [SerializeField] private TMP_InputField emailField;

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

    public void sendCode_btnClicked()
    {
        string email = emailField.text; 
        bool val = FirebaseFunctions.instance.SendPasswordResetEmail(email);
        if (val == true)
        {
            displayToast(toastPanel, "Email to change password sent...");
        }
        else
        {
            displayToast(toastPanel, "Error sending email. Try again later...");
        }
    }

}
