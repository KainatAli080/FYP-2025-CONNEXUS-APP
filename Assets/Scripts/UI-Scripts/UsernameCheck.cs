using MyUtils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernameCheck : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TextMeshProUGUI warningMessage;
    [SerializeField] private ValidationFunctions validate;
    private int maxLength = 25;

    private void Start()
    {
        username.onValueChanged.AddListener(validateUsername);
        validate = new ValidationFunctions();
        warningMessage.text = "";
    }

    private void validateUsername(string input)
    {
        // Onlky allowing alphabets, numbers, hyphen, and underscore
        // Removing special Characters 
        string cleanedInput = validate.isValidUsername(input,maxLength);

        // Update the input field text if changes were made
        if (input != cleanedInput)
        {
            username.text = cleanedInput;
            StartCoroutine(displayWarningMessage("Only A-Z, 0-9, _, - allowed..."));
        }

        //Debug.Log("Cleaned Input: " + cleanedInput);
    }

    private IEnumerator displayWarningMessage(string msg)
    {
        warningMessage.text = msg;
        yield return new WaitForSeconds(1.5f);
        warningMessage.text = "";
    }

}
