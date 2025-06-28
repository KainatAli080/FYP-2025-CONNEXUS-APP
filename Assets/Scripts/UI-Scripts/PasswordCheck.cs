using MyUtils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PasswordCheck : MonoBehaviour
{
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI warningMessage;
    [SerializeField] private Sprite[] togglePassword;   // 0 is visible, 1 is invisible. 1 is default image
    [SerializeField] private Image toggleVisbilityIcon;

    [SerializeField] private ValidationFunctions validate;

    private int maxLength = 25;

    private void Start()
    {
        warningMessage.text = "";
        validate = new ValidationFunctions();

        password.onValueChanged.AddListener(validatePassword);

        UnityEngine.UI.Button toggleButton = toggleVisbilityIcon.GetComponent<UnityEngine.UI.Button>();
        if(toggleButton != null ) 
            toggleButton.onClick.AddListener(togglePasswordVisbility);
    }

    private void validatePassword(string input)
    {
        // Removing special Characters 
        string cleanedInput = validate.isValidPassword(input, maxLength);

        // Update the input field text if changes were made
        if (input != cleanedInput)
        {
            password.text = cleanedInput;
            StartCoroutine(displayWarningMessage("Special characters not allowed..."));
        }

        //Debug.Log("Cleaned Input: " + cleanedInput);        
    }

    private IEnumerator displayWarningMessage(string msg)
    {
        warningMessage.text = msg;
        yield return new WaitForSeconds(1.5f);
        warningMessage.text = "";
    }

    public void togglePasswordVisbility()
    {
        if (toggleVisbilityIcon.sprite == togglePassword[0])
        {
            // 0 means password is visible (Standard)
            password.contentType = TMP_InputField.ContentType.Password;
            toggleVisbilityIcon.sprite = togglePassword[1];
        }
        else if (toggleVisbilityIcon.sprite == togglePassword[1])
        {
            // meaning password is hidden (password)
            password.contentType = TMP_InputField.ContentType.Standard;
            toggleVisbilityIcon.sprite = togglePassword[0];
        }
        password.ForceLabelUpdate(); // Refresh the input field display (this forces the changes to show immediately)
    }

    private void OnDestroy()
    {
        // Remove the listener when the object is destroyed
        password.onValueChanged.RemoveListener(validatePassword);
    }
}
