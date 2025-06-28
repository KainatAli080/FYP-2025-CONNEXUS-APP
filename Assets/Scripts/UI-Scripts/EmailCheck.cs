using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyUtils;

public class EmailCheck : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TextMeshProUGUI warningMessage;
    [SerializeField] private ValidationFunctions validate;
    private int maxLength = 25;

    private void Start()
    {
        email.onEndEdit.AddListener(validateEmail);
        validate = new ValidationFunctions();
    }

    private void validateEmail(string input)
    {
        // Onlky allowing alphabets, numbers, hyphen, and underscore
        if (validate.isValidEmail(input, maxLength))
        {
            Debug.Log("Valid email!");
        }
        else
        {
            StartCoroutine(displayWarningMessage("Invalid Email..."));
        }
    }

    private IEnumerator displayWarningMessage(string msg)
    {
        warningMessage.text = msg;
        yield return new WaitForSeconds(1.5f);
        warningMessage.text = "";
    }

}
