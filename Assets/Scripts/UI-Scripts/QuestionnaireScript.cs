using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class QuestionnaireScript : MonoBehaviour
{
    public AppNavigationScript _NavScript;
    public FirebaseFunctions _FirebaseFunctions;
    public UtilFunctionality _UtilFunctionality;

    [Header("Q/A Related")]
    int[] answers;
    string[] questions = {
        /* Gender + Age */
        "What is your Gender?", 
        "Which age group do you belong to?",
        /* Comfort */
        "How comfortable are you with using VR technology alone?",        
        "How confident are you in your ability to manage stressful situations on your own?",         
        "What do you do when you begin to feel anxious?",
        /* Triggers */
        "How often do you feel overwhelmed when speaking in front of people?",
        "When asked to make a decision under time pressure, how do you react?",
        "Do you experience any of the following physical symptoms when you're anxious?"        
    };

    [Header("Gender Panel")]
    [SerializeField] private ToggleGroup toggleGroup1_gender;

    [Header("Age Panel")]
    [SerializeField] private ToggleGroup toggleGroup2_agegroup;

    [Header("Comfort 1 Panel")]
    [SerializeField] private ToggleGroup toggleGroup3_comfort1;

    [Header("Comfort 2 Panel")]
    [SerializeField] private ToggleGroup toggleGroup4_comfort2;

    [Header("Comfort 3 Panel")]
    [SerializeField] private ToggleGroup toggleGroup5_comfort3;

    [Header("Trigger 1 Panel")]
    [SerializeField] private ToggleGroup toggleGroup6_trigger1;

    [Header("Trigger 2 Panel")]
    [SerializeField] private ToggleGroup toggleGroup7_trigger2;

    [Header("Trigger 3 Panel")]
    [SerializeField] private Transform toggle8_trigger3;

    [Header("Trigger 3 Panel")]
    [SerializeField] private Transform interests_list;

    [Header("Toast Message Panel")]
    [SerializeField] private GameObject toastPanel;
    [SerializeField] private CanvasGroup toastMessage;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 0.5f;

    private int q1_gender;
    private int q2_age;
    private int q3_c1;
    private int q4_c2;
    private int q5_c3;
    private int q6_t1;
    private int q7_t2;
    List<int> thirdTriggerAnswers = new List<int>(); // Stores multi-answer selection

    // Interest related question as well
    List<string> interestsAnswers = new List<string>();

    void Start()
    {
        // Q1 is Gender, save separately
        // Q2 is Age Group, save separately
        // Q8 (Trigger 3) is multi answer
        answers = new int[questions.Length - 1];
        toastMessage.alpha = 0;

        q1_gender = PlayerPrefs.GetInt("Q1", -1);
        q2_age = PlayerPrefs.GetInt("Q2", -1);
        q3_c1 = PlayerPrefs.GetInt("Q3", -1);
        q4_c2 = PlayerPrefs.GetInt("Q4", -1);
        q5_c3 = PlayerPrefs.GetInt("Q5", -1);
        q6_t1 = PlayerPrefs.GetInt("Q6", -1);
        q7_t2 = PlayerPrefs.GetInt("Q7", -1);

        // no need to save q8_t3 in player prefs
    }

    // ---------------------------------------------------------------------------------------- //
    //                                      UTILITY FUNCTIONS                                   //
    // ---------------------------------------------------------------------------------------- //

    public int calculateIntroversionScore(int[] answers, List<int> multiAnswerQuestion)
    {
        if (answers == null)
        {
            Debug.LogError("Invalid number of answers. Expected 6.");
            return -1;
        }

        int introversionScore = 0;

        // Comfort Questions (c1, c2, c3) - Lower confidence = Higher introversion
        introversionScore += answers[2]; // "How comfortable are you with using VR technology alone?"
        Debug.Log("Introversion Score Q1: " + introversionScore);
        introversionScore += answers[3]; // "How confident are you in your ability to manage stressful situations on your own?"
        Debug.Log("Introversion Score Q2: " + introversionScore);
        introversionScore += answers[4]; // "What do you do when you begin to feel anxious?"
        Debug.Log("Introversion Score Q3: " + introversionScore);

        // Triggers Questions (c4, c5, c6) - Higher struggle = Higher introversion
        introversionScore += answers[5]; // "How often do you feel overwhelmed when speaking in front of people?"
        Debug.Log("Introversion Score Q4: " + introversionScore);
        introversionScore += answers[6]; // "When asked to make a decision under time pressure, how do you react?"
        Debug.Log("Introversion Score Q5: " + introversionScore);

        // Physical Symptoms Question (c6) - Multi-select, sum of all selected options
        int multiQuestionScore = multiAnswerQuestion.Sum();
        Debug.Log("Introversion Score Q6: " + multiQuestionScore);
        introversionScore += multiQuestionScore; // "Do you experience any of the following physical symptoms when you're anxious?"

        Debug.Log("Total Introversion Score: " + introversionScore);
        return introversionScore;
    }

    private Toggle GetSelectedToggle(ToggleGroup toggleGroup)
    {
        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            if (toggle.isOn)
            {
                return toggle;
            }
        }
        return null;
    }

    private void TurnOnCheckedToggle(ToggleGroup toggleGroup, int optionSaved)
    {
        Transform parent = toggleGroup.transform; // Get the parent containing all toggles
        Toggle savedToggle = parent.GetChild(optionSaved).GetComponent<Toggle>();

        if (savedToggle != null)
        {
            savedToggle.isOn = true;
        }
    }

    private void displayToast()
    {
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

    private void displayToast_Testing(GameObject toastObject, string textToDisplay)
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

    // ---------------------------------------------------------------------------------------- //
    //                                  QUESTIONNAIRE FUNCTIONS                                 //
    // ---------------------------------------------------------------------------------------- //

    // ---------------------  QUESTION 1 RELATED  -------------------- //

    public void question1_gender()
    {        
        Toggle selectedToggle = GetSelectedToggle(toggleGroup1_gender);
        if (selectedToggle != null)
        {
            int choice = selectedToggle.transform.GetSiblingIndex();
            answers[0] = choice;

            // Save selected option in PlayerPrefs
            PlayerPrefs.SetInt("Q1", choice);
            PlayerPrefs.Save();
            Debug.Log("Saved: Question 1 = Option " + choice);

            _NavScript.fromQuestionGenderToQuestionAge();
            // after navigation, chgecking if 2 has amnything saved. If so, selecting that toggle
            if (q2_age != -1)
            {
                TurnOnCheckedToggle(toggleGroup2_agegroup, q2_age);
            }
        }
        else
        {
            Debug.LogWarning("No option selected for Question 1");
            // Show toast message that no option selected
            //displayToast();
            displayToast_Testing(toastPanel, "Select an option to proceed...");
        }        
    }

    public void backFromQ2ToQ1()
    {
        _NavScript.goBackFromQuestionAgeToQuestionGender();
        if (q1_gender < 0)
        {
            Debug.Log("Nothing Saved for Question1.");
        }
        else
        {
            TurnOnCheckedToggle(toggleGroup1_gender, q1_gender);
        }        
    }

    // ---------------------  QUESTION 2 RELATED  -------------------- //

    public void question2_age()
    {
        Toggle selectedToggle = GetSelectedToggle(toggleGroup2_agegroup);
        if (selectedToggle != null)
        {
            int choice = selectedToggle.transform.GetSiblingIndex();
            answers[1] = choice;

            // Save selected option in PlayerPrefs
            PlayerPrefs.SetInt("Q2", choice);
            PlayerPrefs.Save();
            Debug.Log("Saved: Question 2 = Option " + choice);

            _NavScript.fromQuestionAgeToQuestionComfort1();
        }
        else
        {
            Debug.LogWarning("No option selected for Question 1");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select an option to proceed...");;
        }
    }

    public void backFromQ3ToQ2()
    {
        _NavScript.goBackFromQuestionComfort1ToQuestionAge();
        if (q2_age < 0)
        {
            Debug.Log("Nothing Saved for Question2.");
        }
        else
        {
            TurnOnCheckedToggle(toggleGroup2_agegroup, q2_age);
        }        
    }

    // ---------------------  QUESTION 3 RELATED  -------------------- //

    public void question3_comfort1()
    {
        Toggle selectedToggle = GetSelectedToggle(toggleGroup3_comfort1);
        if (selectedToggle != null)
        {
            int choice = selectedToggle.transform.GetSiblingIndex();
            answers[2] = choice;

            // Save selected option in PlayerPrefs
            PlayerPrefs.SetInt("Q3", choice);
            PlayerPrefs.Save();
            Debug.Log("Saved: Question 3 = Option " + choice);

            _NavScript.fromQuestionComfort1ToQuestionComfort2();
            if (q4_c2 != -1)
            {
                TurnOnCheckedToggle(toggleGroup4_comfort2, q4_c2);
            }
        }
        else
        {
            Debug.LogWarning("No option selected for Question 1");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select an option to proceed...");;
        }
    }

    public void backFromQ4ToQ3()
    {
        _NavScript.goBackFromQuestionComfort1ToQuestionAge();
        if (q3_c1 < 0)
        {
            Debug.Log("Nothing Saved for Question3.");
        }
        else
        {
            TurnOnCheckedToggle(toggleGroup3_comfort1, q3_c1);
        }        
    }

    // ---------------------  QUESTION 4 RELATED  -------------------- //

    public void question4_comfort2()
    {
        Toggle selectedToggle = GetSelectedToggle(toggleGroup4_comfort2);
        if (selectedToggle != null)
        {
            int choice = selectedToggle.transform.GetSiblingIndex();
            answers[3] = choice;

            // Save selected option in PlayerPrefs
            PlayerPrefs.SetInt("Q4", choice);
            PlayerPrefs.Save();
            Debug.Log("Saved: Question 4 = Option " + choice);

            // Q5 work
            _NavScript.fromQuestionComfort2ToQuestionComfort3();
            if (q5_c3 != -1)
            {
                TurnOnCheckedToggle(toggleGroup5_comfort3, q5_c3);
            }
        }
        else
        {
            Debug.LogWarning("No option selected for Question 1");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select an option to proceed...");
        }
    }

    public void backFromQ5ToQ4()
    {
        _NavScript.goBackFromQuestionComfort3ToQuestionComfort2();
        if (q4_c2 < 0)
        {
            Debug.Log("Nothing Saved for Question4.");
        }
        else
        {
            TurnOnCheckedToggle(toggleGroup4_comfort2, q4_c2);
        }
    }

    // ---------------------  QUESTION 5 RELATED  -------------------- //

    public void question5_comfort3()
    {
        Toggle selectedToggle = GetSelectedToggle(toggleGroup5_comfort3);
        if (selectedToggle != null)
        {
            int choice = selectedToggle.transform.GetSiblingIndex();
            answers[4] = choice;

            // Save selected option in PlayerPrefs
            PlayerPrefs.SetInt("Q5", choice);
            PlayerPrefs.Save();
            Debug.Log("Saved: Question 5 = Option " + choice);

            // Q5 work
            _NavScript.fromQuestionComfort3ToQuestionTrigger1();
            if (q6_t1 != -1)
            {
                TurnOnCheckedToggle(toggleGroup6_trigger1, q6_t1);
            }
        }
        else
        {
            Debug.LogWarning("No option selected for Question 1");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select an option to proceed...");
        }
    }

    public void backFromQ6ToQ5()
    {
        _NavScript.goBackFromQuestionTrigger1ToQuestionComfort3();
        if (q5_c3 < 0)
        {
            Debug.Log("Nothing Saved for Question5.");
        }
        else
        {
            TurnOnCheckedToggle(toggleGroup5_comfort3, q5_c3);
        }
    }

    // ---------------------  QUESTION 6 RELATED  -------------------- //

    public void question6_trigger1()
    {
        Toggle selectedToggle = GetSelectedToggle(toggleGroup6_trigger1);
        if (selectedToggle != null)
        {
            int choice = selectedToggle.transform.GetSiblingIndex();
            answers[5] = choice;

            // Save selected option in PlayerPrefs
            PlayerPrefs.SetInt("Q6", choice);
            PlayerPrefs.Save();
            Debug.Log("Saved: Question 6 = Option " + choice);

            // Q7 work
            _NavScript.fromQuestionTrigger1ToQuestionTrigger2();
            if (q7_t2 != -1)
            {
                TurnOnCheckedToggle(toggleGroup7_trigger2, q7_t2);
            }
        }
        else
        {
            Debug.LogWarning("No option selected for Question 1");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select an option to proceed...");
        }
    }

    public void backFromQ7ToQ6()
    {
        _NavScript.goBackFromQuestionTrigger2ToQuestionTrigger1();
        if (q6_t1 < 0)
        {
            Debug.Log("Nothing Saved for Question6.");
        }
        else
        {
            TurnOnCheckedToggle(toggleGroup6_trigger1, q6_t1);
        }
    }

    // ---------------------  QUESTION 7 RELATED  -------------------- //

    public void question7_trigger2()
    {
        Toggle selectedToggle = GetSelectedToggle(toggleGroup7_trigger2);
        if (selectedToggle != null)
        {
            int choice = selectedToggle.transform.GetSiblingIndex();
            answers[6] = choice;

            // Save selected option in PlayerPrefs
            PlayerPrefs.SetInt("Q7", choice);
            PlayerPrefs.Save();
            Debug.Log("Saved: Question 7 = Option " + choice);

            // Q5 work
            _NavScript.fromQuestionTrigger2ToQuestionTrigger3();            
        }
        else
        {
            Debug.LogWarning("No option selected for Question 7");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select an option to proceed...");
        }
    }

    public void backFromQ8ToQ7()
    {
        _NavScript.goBackFromQuestionTrigger3ToQuestionTrigger2();
        
    }

    // ---------------------  QUESTION 5 RELATED  -------------------- //

    public void question8_trigger3()
    {
        Toggle[] toggles = toggle8_trigger3.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn) // Allows multiple selections
            {
                thirdTriggerAnswers.Add(i); // Store index of selected toggle
            }
        }

        if (thirdTriggerAnswers.Count == 0)
        {
            Debug.LogWarning("No option selected for Question 8");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select an option to proceed...");
        }
        else
        {
            //FirebaseFunctions.instance.SaveQuestionnaire(answers, thirdTriggerAnswers);

            int introversion_score = _UtilFunctionality.calculateIntroversionScore(answers, thirdTriggerAnswers);
            //int introversion_score = calculateIntroversionScore(answers, thirdTriggerAnswers);
            Debug.Log("Introversion Score returned: " + introversion_score);
            FirebaseFunctions.instance.SaveIntroversionScore(introversion_score);

            _FirebaseFunctions.SaveQuestionnaire(answers, thirdTriggerAnswers);
            Debug.Log("Questionnaire Saved!");
            _NavScript.fromQuestionTrigger3ToInterestsPanel();
        }
    }

    // ---------------------  INTERESTS RELATED  -------------------- //
    public void interests_storage()
    {
        Toggle[] toggles = interests_list.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn) // Allows multiple selections
            {
                string str = toggles[i].GetComponentInChildren<TextMeshProUGUI>().text;
                interestsAnswers.Add(str); // Store index of selected toggle
            }
        }

        if (interestsAnswers.Count == 0)
        {
            Debug.LogWarning("No option selected for Interests");
            // Show toast message that no option selected
            displayToast_Testing(toastPanel, "Select a few interests to proceed...");
        }
        else
        {
            FirebaseFunctions.instance.SaveInterests(interestsAnswers);
            //_FirebaseFunctions.SaveInterests(interestsAnswers);
            Debug.Log("Interests Saved!");
            _NavScript.fromInterestsToDashboard();
        }
    }
}
