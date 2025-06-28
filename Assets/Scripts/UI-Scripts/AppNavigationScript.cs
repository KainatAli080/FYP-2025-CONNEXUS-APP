using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AppNavigationScript : MonoBehaviour
{
    [Header("Splash, Login, SignUp")]
    [SerializeField] private GameObject splashScreenPanel;
    [SerializeField] private GameObject loginScreenPanel;
    [SerializeField] private GameObject signupScreenPanel;

    [Header("Password Related")]
    [SerializeField] private GameObject forgotPasswordPanel;
    [SerializeField] private GameObject codeVerificationPanel;
    [SerializeField] private GameObject newPasswordPanel;

    [Header("Welcome Panels")]
    [SerializeField] private GameObject welcomeP1Panel;
    [SerializeField] private GameObject welcomeP2Panel;
    [SerializeField] private GameObject welcomeP3Panel;

    [Header("Questionnaire + Interests")]
    [SerializeField] private GameObject questionnaireGenderPanel;
    [SerializeField] private GameObject questionnaireAgePanel;
    [SerializeField] private GameObject questionnaireComfort1Panel;
    [SerializeField] private GameObject questionnaireComfort2Panel;
    [SerializeField] private GameObject questionnaireComfort3Panel;
    [SerializeField] private GameObject questionnaireTrigger1Panel;
    [SerializeField] private GameObject questionnaireTrigger2Panel;
    [SerializeField] private GameObject questionnaireTrigger3Panel;
    [SerializeField] private GameObject interestsPanel;

    [Header("Dashboard Screens")]
    [SerializeField] private GameObject dashboardPanel;
    [SerializeField] private GameObject leaderboardPanel;     
    [SerializeField] private GameObject simulationsListScreen;
    
    [Header("Simulation Three Types")]
    [SerializeField] private GameObject decisionSimulationsListScreen;
    [SerializeField] private GameObject confidenceSimulationsListScreen;
    [SerializeField] private GameObject groupSimulationsListScreen;

    [Header("Simulation Type 1: Decision Making")]
    [SerializeField] private GameObject decisionMakingEasyScreen;
    [SerializeField] private GameObject decisionMakingMediumScreen;
    [SerializeField] private GameObject decisionMakingEasy_MostRecentResult;
    [SerializeField] private GameObject decisionMakingMedium_MostRecentResult;

    [Header("Simulation Type 2: Confidence Building")]
    [SerializeField] private GameObject confidenceBuildingEasyScreen;
    [SerializeField] private GameObject confidenceBuildingMediumScreen;
    [SerializeField] private GameObject confidenceBuildingEasy_MostRecentResult;

    [Header("Simulation Type 3: Group Interaction")]
    [SerializeField] private GameObject groupInteractionEasyScreen;
    [SerializeField] private GameObject groupInteractionMediumScreen;
    [SerializeField] private GameObject groupInteractionEasy_MostRecentResult;
    [SerializeField] private GameObject groupInteractionMedium_MostRecentResult;

    [Header("Bottom Navigation")]
    [SerializeField] private GameObject bottomNavigationScreen;
    [SerializeField] private Image homeRoofLogo;
    [SerializeField] private Image homeBodyLogo;
    [SerializeField] private Image simulationLogo;
    [SerializeField] private Image leaderboardLogo;
    [SerializeField] private Image profileHeadLogo;
    [SerializeField] private Image profileBodyLogo;

    [Header("Animation Scripts")]
    [SerializeField] private ManualAnimationsScipt animScript;

    [Header("Extra Variables")]
    [SerializeField] private bool splashDisplayed = false;

    // Color Conversion Variables
    private string selectedColorHexa = "#C9F4F1"; 
    private string unselectedColorHexa = "#6EB7AE";
    private Color selectedIconColor;
    private Color unselectedIconColor;

    private float splashDisplayTime = 3f;


    // Start is called before the first frame update
    void Start()
    {
        splashScreenPanel.SetActive(true);
        loginScreenPanel.SetActive(false);
        signupScreenPanel.SetActive(false);

        forgotPasswordPanel.SetActive(false);
        codeVerificationPanel.SetActive(false);
        newPasswordPanel.SetActive(false);

        welcomeP1Panel.SetActive(false);
        welcomeP2Panel.SetActive(false);
        welcomeP3Panel.SetActive(false);

        questionnaireAgePanel.SetActive(false);
        questionnaireGenderPanel.SetActive(false);
        questionnaireComfort1Panel.SetActive(false);
        questionnaireComfort2Panel.SetActive(false);
        questionnaireComfort3Panel.SetActive(false);
        questionnaireTrigger1Panel.SetActive(false);
        questionnaireTrigger2Panel.SetActive(false);
        questionnaireTrigger3Panel.SetActive(false);
        interestsPanel.SetActive(false);

        dashboardPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        simulationsListScreen.SetActive(false);

        decisionSimulationsListScreen.SetActive(false);
        confidenceSimulationsListScreen.SetActive(false) ;
        groupSimulationsListScreen.SetActive(false);

        decisionMakingEasyScreen.SetActive(false);
        decisionMakingMediumScreen.SetActive(false);
        decisionMakingEasy_MostRecentResult.SetActive(false);

        confidenceBuildingEasyScreen.SetActive(false);
        confidenceBuildingMediumScreen.SetActive(false );
        confidenceBuildingEasy_MostRecentResult.SetActive(false);

        groupInteractionEasyScreen.SetActive(false);
        groupInteractionMediumScreen.SetActive(false);
        groupInteractionEasy_MostRecentResult.SetActive(false);

        bottomNavigationScreen.SetActive(false);

        // Converting Hexa to Color to be assigned
        ColorUtility.TryParseHtmlString(selectedColorHexa, out selectedIconColor);
        ColorUtility.TryParseHtmlString(unselectedColorHexa, out unselectedIconColor);

        // Starting splash screen display
        StartCoroutine(displaySplashScreen());
    }

    // -------------------------------------------------------------------------- //
    //                       COROUTINE FUNCTIONS AND WORKING                      //
    // -------------------------------------------------------------------------- //

    private IEnumerator displaySplashScreen()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(splashDisplayTime);

        splashScreenPanel.SetActive(false);
        splashDisplayed = true;
        toLogin();
    }

    private IEnumerator addDelay_loginExitAnim()
    {
        animScript.loginPanel_exitAnimation();
        yield return new WaitForSeconds(0.5f);

        signupScreenPanel.SetActive(true);
        loginScreenPanel.SetActive(false);
        animScript.signupPanel_startAnimation();
    }

    private IEnumerator addDelay_signUpExitAnim()
    {
        animScript.signupPanel_exitAnimation();
        yield return new WaitForSeconds(0.5f);

        loginScreenPanel.SetActive(true);
        signupScreenPanel.SetActive(false);
        animScript.loginPanel_startAnimation();
    }

    // -------------------------------------------------------------------------- //
    //                       Simple Navigation and Visibility                     //
    // -------------------------------------------------------------------------- //

    public void toLogin()
    {
        loginScreenPanel.SetActive(true);

        // Animation Call
        animScript.loginPanel_startAnimation();
    }

    public void loggedOut()
    {
        splashScreenPanel.SetActive(false);        
        signupScreenPanel.SetActive(false);

        forgotPasswordPanel.SetActive(false);
        codeVerificationPanel.SetActive(false);
        newPasswordPanel.SetActive(false);

        welcomeP1Panel.SetActive(false);
        welcomeP2Panel.SetActive(false);
        welcomeP3Panel.SetActive(false);

        questionnaireAgePanel.SetActive(false);
        questionnaireGenderPanel.SetActive(false);
        questionnaireComfort1Panel.SetActive(false);
        questionnaireComfort2Panel.SetActive(false);
        questionnaireComfort3Panel.SetActive(false);
        questionnaireTrigger1Panel.SetActive(false);
        questionnaireTrigger2Panel.SetActive(false);
        questionnaireTrigger3Panel.SetActive(false);
        interestsPanel.SetActive(false);

        simulationsListScreen.SetActive(false);
        decisionSimulationsListScreen.SetActive(false);
        confidenceSimulationsListScreen.SetActive(false);
        groupSimulationsListScreen.SetActive(false);

        decisionMakingEasyScreen.SetActive(false);
        decisionMakingMediumScreen.SetActive(false);

        confidenceBuildingEasyScreen.SetActive(false);
        confidenceBuildingMediumScreen.SetActive(false);

        groupInteractionEasyScreen.SetActive(false);
        groupInteractionMediumScreen.SetActive(false);

        bottomNavigationScreen.SetActive(false);

        // Main Screen to go to
        loginScreenPanel.SetActive(true);
        animScript.loginPanel_startAnimation();
    }

    public void termsAndConditionsNotAgreedTo()
    {
        splashScreenPanel.SetActive(false);
        signupScreenPanel.SetActive(false);
        loginScreenPanel.SetActive(false);

        forgotPasswordPanel.SetActive(false);
        codeVerificationPanel.SetActive(false);
        newPasswordPanel.SetActive(false);

        welcomeP1Panel.SetActive(true);
        welcomeP2Panel.SetActive(false);
        welcomeP3Panel.SetActive(false);

        questionnaireAgePanel.SetActive(false);
        questionnaireGenderPanel.SetActive(false);
        questionnaireComfort1Panel.SetActive(false);
        questionnaireComfort2Panel.SetActive(false);
        questionnaireComfort3Panel.SetActive(false);
        questionnaireTrigger1Panel.SetActive(false);
        questionnaireTrigger2Panel.SetActive(false);
        questionnaireTrigger3Panel.SetActive(false);
        interestsPanel.SetActive(false);

        simulationsListScreen.SetActive(false);
        decisionSimulationsListScreen.SetActive(false);
        confidenceSimulationsListScreen.SetActive(false);
        groupSimulationsListScreen.SetActive(false);

        decisionMakingEasyScreen.SetActive(false);
        decisionMakingMediumScreen.SetActive(false);

        confidenceBuildingEasyScreen.SetActive(false);
        confidenceBuildingMediumScreen.SetActive(false);

        groupInteractionEasyScreen.SetActive(false);
        groupInteractionMediumScreen.SetActive(false);

        bottomNavigationScreen.SetActive(false);
    }

    public void ifQuestionnaireNotFilled()
    {
        try
        {
            Debug.Log("QuestionnaireNotFilled function called");
            questionnaireGenderPanel.SetActive(true);
            //StartCoroutine(DelayedActivation());
            Debug.Log("Questionnaire Gender panel called");

            splashScreenPanel.SetActive(false);
            signupScreenPanel.SetActive(false);
            loginScreenPanel.SetActive(false);

            forgotPasswordPanel.SetActive(false);
            codeVerificationPanel.SetActive(false);
            newPasswordPanel.SetActive(false);

            welcomeP1Panel.SetActive(false);
            welcomeP2Panel.SetActive(false);
            welcomeP3Panel.SetActive(false);

            //questionnaireGenderPanel.SetActive(true);
            //Debug.Log("Questionnaire Gender panal called");

            questionnaireAgePanel.SetActive(false);
            questionnaireComfort1Panel.SetActive(false);
            questionnaireComfort2Panel.SetActive(false);
            questionnaireComfort3Panel.SetActive(false);
            questionnaireTrigger1Panel.SetActive(false);
            questionnaireTrigger2Panel.SetActive(false);
            questionnaireTrigger3Panel.SetActive(false);
            interestsPanel.SetActive(false);

            simulationsListScreen.SetActive(false);
            decisionSimulationsListScreen.SetActive(false);
            confidenceSimulationsListScreen.SetActive(false);
            groupSimulationsListScreen.SetActive(false);

            decisionMakingEasyScreen.SetActive(false);
            decisionMakingMediumScreen.SetActive(false);

            confidenceBuildingEasyScreen.SetActive(false);
            confidenceBuildingMediumScreen.SetActive(false);

            groupInteractionEasyScreen.SetActive(false);
            groupInteractionMediumScreen.SetActive(false);

            bottomNavigationScreen.SetActive(false);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

    }

    IEnumerator DelayedActivation()
    {
        yield return new WaitForSeconds(1f);
        questionnaireGenderPanel.SetActive(true);
        Debug.Log("Questionnaire Gender panel called AFTER DELAY");
    }

    public void ifInterestsNotFilled()
    {
        splashScreenPanel.SetActive(false);
        signupScreenPanel.SetActive(false);
        loginScreenPanel.SetActive(false);

        forgotPasswordPanel.SetActive(false);
        codeVerificationPanel.SetActive(false);
        newPasswordPanel.SetActive(false);

        welcomeP1Panel.SetActive(false);
        welcomeP2Panel.SetActive(false);
        welcomeP3Panel.SetActive(false);

        questionnaireAgePanel.SetActive(false);
        questionnaireGenderPanel.SetActive(false);
        questionnaireComfort1Panel.SetActive(false);
        questionnaireComfort2Panel.SetActive(false);
        questionnaireComfort3Panel.SetActive(false);
        questionnaireTrigger1Panel.SetActive(false);
        questionnaireTrigger2Panel.SetActive(false);
        questionnaireTrigger3Panel.SetActive(false);
        interestsPanel.SetActive(true);

        simulationsListScreen.SetActive(false);
        decisionSimulationsListScreen.SetActive(false);
        confidenceSimulationsListScreen.SetActive(false);
        groupSimulationsListScreen.SetActive(false);

        decisionMakingEasyScreen.SetActive(false);
        decisionMakingMediumScreen.SetActive(false);

        confidenceBuildingEasyScreen.SetActive(false);
        confidenceBuildingMediumScreen.SetActive(false);

        groupInteractionEasyScreen.SetActive(false);
        groupInteractionMediumScreen.SetActive(false);

        bottomNavigationScreen.SetActive(false);
    }

    // -----------------------     Login Signup Nav     ------------------------ //

    public void fromLoginToSignUp() 
    {        
        // Adding coroutine to incorporate delay
        StartCoroutine(addDelay_loginExitAnim());
        // Coroutine does not block the executing code. So if panels were being set as active before animatiuon finished, then animation wouldn't show. 
        // loginAnimScript.signupPanel_startAnimation();
    }

    public void fromSignUpToLogin()
    {
        //animScript.loginPanel_startAnimation();
        StartCoroutine(addDelay_signUpExitAnim());
        //loginScreenPanel.SetActive(true);
        //signupScreenPanel.SetActive(false);
    }

    // --------------------- Forgot Password Screens Nav  ------------------------ //

    public void fromLoginToForgotPassword()
    {
        forgotPasswordPanel.SetActive(true);
        loginScreenPanel.SetActive(false);
    }

    public void fromForgotPasswordToCodeVerification()
    {
        codeVerificationPanel.SetActive(true);
        forgotPasswordPanel.SetActive(false);
    }

    public void fromCodeVerificationToNewPassword()
    {
        newPasswordPanel.SetActive(true);
        codeVerificationPanel.SetActive(false);
    }

    public void fromNewPasswordToLogin()
    {
        loginScreenPanel.SetActive(true);
        newPasswordPanel.SetActive(false);
    }

    // ---------------------     Welcome Screens Nav     ------------------------ //

    public void fromSignUpToWelcomeP1()
    { 
        welcomeP1Panel.SetActive(true);
        signupScreenPanel.SetActive(false) ;
    }

    public void fromWelcomeP1ToWelcomeP2()
    {
        welcomeP2Panel.SetActive(true);
        welcomeP1Panel.SetActive(false);
    }
    public void fromWelcomeP2ToWelcomeP3()
    {
        welcomeP3Panel.SetActive(true);
        welcomeP2Panel.SetActive(false);
    }

    public void fromWelcomeP3ToQ1GenderPanel()
    {
        questionnaireGenderPanel.SetActive(true);
        welcomeP3Panel.SetActive(false);
    }

    public void goBackFromWelcomeP3ToWelcomeP2()
    {
        welcomeP2Panel.SetActive(true);
        welcomeP3Panel.SetActive(false);
    }
    
    public void goBackFromWelcomeP2ToWelcomeP1()
    {
        welcomeP1Panel.SetActive(true);
        welcomeP2Panel.SetActive(false);
    }

    // -------------------     Interests + Questionnaire Nav     -------------------- //

    public void fromQuestionGenderToQuestionAge()
    {
        questionnaireAgePanel.SetActive(true);
        questionnaireGenderPanel.SetActive(false);        
    }

    public void fromQuestionAgeToQuestionComfort1()
    {
        questionnaireComfort1Panel.SetActive(true);
        questionnaireAgePanel.SetActive(false);
    }

    public void fromQuestionComfort1ToQuestionComfort2()
    {
        questionnaireComfort2Panel.SetActive(true);
        questionnaireComfort1Panel.SetActive(false);
    }

    public void fromQuestionComfort2ToQuestionComfort3()
    {
        questionnaireComfort3Panel.SetActive(true);
        questionnaireComfort2Panel.SetActive(false);
    }

    public void fromQuestionComfort3ToQuestionTrigger1()
    {
        questionnaireTrigger1Panel.SetActive(true);
        questionnaireComfort3Panel.SetActive(false);
    }

    public void fromQuestionTrigger1ToQuestionTrigger2()
    {
        questionnaireTrigger2Panel.SetActive(true);
        questionnaireTrigger1Panel.SetActive(false);
    }

    public void fromQuestionTrigger2ToQuestionTrigger3()
    {
        questionnaireTrigger3Panel.SetActive(true);
        questionnaireTrigger2Panel.SetActive(false);
    }

    public void fromQuestionTrigger3ToInterestsPanel()
    {
        interestsPanel.SetActive(true);
        questionnaireTrigger3Panel.SetActive(false);
    }

    public void fromInterestsToDashboard()
    {
        dashboardPanel.SetActive(true);
        bottomNavigationScreen.SetActive(true);
        interestsPanel.SetActive(false);
    }

    // Go back functions starting

    public void goBackFromQuestionTrigger3ToQuestionTrigger2()
    {
        questionnaireTrigger3Panel.SetActive(false);
        questionnaireTrigger2Panel.SetActive(true);
    }

    public void goBackFromQuestionTrigger2ToQuestionTrigger1()
    {
        questionnaireTrigger2Panel.SetActive(false);
        questionnaireTrigger1Panel.SetActive(true);
    }

    public void goBackFromQuestionTrigger1ToQuestionComfort3()
    {
        questionnaireTrigger1Panel.SetActive(false);
        questionnaireComfort3Panel.SetActive(true);
    }

    public void goBackFromQuestionComfort3ToQuestionComfort2()
    {
        questionnaireComfort3Panel.SetActive(false);
        questionnaireComfort2Panel.SetActive(true);
    }

    public void goBackFromQuestionComfort2ToQuestionComfort1()
    {
        questionnaireComfort2Panel.SetActive(false);
        questionnaireComfort1Panel.SetActive(true);
    }

    public void goBackFromQuestionComfort1ToQuestionAge()
    {
        questionnaireComfort1Panel.SetActive(false);
        questionnaireAgePanel.SetActive(true);
    }

    public void goBackFromQuestionAgeToQuestionGender()
    {
        questionnaireAgePanel.SetActive(false);
        questionnaireGenderPanel.SetActive(true);
    }

    // -----------------------     Simulations Navigation     ------------------------- //

    public void fromSimulationsToDecisionMaking()
    {
        decisionSimulationsListScreen.SetActive(true);
        simulationsListScreen.SetActive(false);
    }

    public void fromSimulationsToConfidenceBuilding()
    {
        confidenceSimulationsListScreen.SetActive(true);
        simulationsListScreen.SetActive(false);
    }

    public void fromSimulationsToGroupInteraction()
    {
        groupSimulationsListScreen.SetActive(true);
        simulationsListScreen.SetActive(false);
    }

    // ---------------------------------------------- //
    // ---------------  Decision Making  ------------ //
    // ---------------------------------------------- //

    public void fromDecisionListToEasy()
    {
        decisionMakingEasyScreen.SetActive(true);
        decisionSimulationsListScreen.SetActive(false );
        bottomNavigationScreen.SetActive(false);
    }

    public void fromDecisionEasyToDecisionEasyResult()
    {
        decisionMakingEasy_MostRecentResult.SetActive(true);
        decisionMakingEasyScreen.SetActive(false);
    }

    public void fromDecisionListToMedium()
    {
        decisionMakingMediumScreen.SetActive(true);
        decisionSimulationsListScreen.SetActive(false);
        bottomNavigationScreen.SetActive(false);
    }

    public void fromDecisionMediumToDecisionMediumResult()
    {
        decisionMakingMedium_MostRecentResult.SetActive(true);
        decisionMakingMediumScreen.SetActive(false);
    }

    public void goBackFromDecisionListToSimulations()
    {
        simulationsListScreen.SetActive(true);
        decisionSimulationsListScreen.SetActive(false) ;
    }

    public void goBackFromDecisionEasyToDecisionList()
    {
        decisionSimulationsListScreen.SetActive(true);
        bottomNavigationScreen.SetActive(true );

        decisionMakingEasyScreen.SetActive(false);
    }

    public void goBackFromDecisionMediumToDecisionList()
    {
        decisionSimulationsListScreen.SetActive(true);
        bottomNavigationScreen.SetActive(true);

        decisionMakingMediumScreen.SetActive(false);
    }

    public void goBackFromDecisionEasyResultToDecisionEasy()
    {
        decisionMakingEasyScreen.SetActive(true);
        decisionMakingEasy_MostRecentResult.SetActive(false);
    }

    public void goBackFromDecisionMediumResultToDecisionMedium()
    {
        decisionMakingMediumScreen.SetActive(true);
        decisionMakingMedium_MostRecentResult.SetActive(false);
    }

    // ---------------------------------------------- //
    // -------------  Confidence Building  ---------- //
    // ---------------------------------------------- //

    public void fromConfidenceListToEasy()
    {
        confidenceBuildingEasyScreen.SetActive(true);
        confidenceSimulationsListScreen.SetActive(false);
        bottomNavigationScreen.SetActive(false);
    }

    public void fromConfidenceEasyToConfidenceEasyResult()
    {
        confidenceBuildingEasy_MostRecentResult.SetActive(true);
        confidenceBuildingEasyScreen.SetActive(false);
    }

    public void fromConfidenceListToMedium()
    {
        confidenceBuildingMediumScreen.SetActive(true);
        confidenceSimulationsListScreen.SetActive(false);
        bottomNavigationScreen.SetActive(false);
    }

    public void goBackFromConfidenceListToSimulations()
    {
        simulationsListScreen.SetActive(true);
        confidenceSimulationsListScreen.SetActive(false);
    }

    public void goBackFromConfidenceEasyToConfidenceList()
    {
        confidenceSimulationsListScreen.SetActive(true);
        bottomNavigationScreen.SetActive(true);

        confidenceBuildingEasyScreen.SetActive(false);
    }

    public void goBackFromConfidenceMediumToConfidenceList()
    {
        confidenceSimulationsListScreen.SetActive(true);
        bottomNavigationScreen.SetActive(true);

        confidenceBuildingMediumScreen.SetActive(false);
    }

    public void goBackFromConfidenceEasyResultToConfidencEasy()
    {
        confidenceBuildingEasyScreen.SetActive(true);
        confidenceBuildingEasy_MostRecentResult.SetActive(false);
    }


    // ---------------------------------------------- //
    // --------------  Group Interaction  ----------- //
    // ---------------------------------------------- //

    public void fromGroupListToEasy()
    {
        groupInteractionEasyScreen.SetActive(true);
        groupSimulationsListScreen.SetActive(false);
        bottomNavigationScreen.SetActive(false);
    }

    public void fromGroupEasyToGroupEasyResult()
    {
        groupInteractionEasy_MostRecentResult.SetActive(true);
        groupInteractionEasyScreen.SetActive(false);
    }

    public void fromGroupListToMedium()
    {
        groupInteractionMediumScreen.SetActive(true);
        groupSimulationsListScreen.SetActive(false);
        bottomNavigationScreen.SetActive(false);
    }

    public void goBackFromGroupListToSimulations()
    {
        simulationsListScreen.SetActive(true);
        groupSimulationsListScreen.SetActive(false);
    }

    public void goBackFromGroupEasyToGroupList()
    {
        groupSimulationsListScreen.SetActive(true);
        bottomNavigationScreen.SetActive(true);

        groupInteractionEasyScreen.SetActive(false);
    }

    public void goBackFromGroupMediumToGroupList()
    {
        groupSimulationsListScreen.SetActive(true);
        bottomNavigationScreen.SetActive(true);

        groupInteractionMediumScreen.SetActive(false);
    }

    public void goBackFromGroupEasyResultToGroupEasy()
    {
        groupInteractionEasyScreen.SetActive(true);
        groupInteractionEasy_MostRecentResult.SetActive(false);
    }

    // -------------------     Bottom Navigation Functionality     -------------------- //

    public void toSimulations()
    {
        simulationLogo.color = selectedIconColor;

        homeBodyLogo.color = unselectedIconColor;
        homeRoofLogo.color = unselectedIconColor;
        leaderboardLogo.color = unselectedIconColor;
        profileBodyLogo.color = unselectedIconColor;
        profileHeadLogo.color = unselectedIconColor;

        // Settings only Main simulations Screen active
        simulationsListScreen.SetActive(true);

        // Setting all other pages as false in case it comes from any of these
        dashboardPanel.SetActive(false );

        confidenceSimulationsListScreen.SetActive(false );
        decisionSimulationsListScreen.SetActive(false ) ;
        groupSimulationsListScreen.SetActive (false );
    }

    public void toDashboard()
    {
        homeBodyLogo.color = selectedIconColor;
        homeRoofLogo.color = selectedIconColor;

        simulationLogo.color = unselectedIconColor;        
        leaderboardLogo.color = unselectedIconColor;
        profileBodyLogo.color = unselectedIconColor;
        profileHeadLogo.color = unselectedIconColor;

        // Settings only Main simulations Screen active
        dashboardPanel.SetActive(true);
        bottomNavigationScreen.SetActive(true);

        // Setting all other pages as false in case it comes from any of these
        simulationsListScreen.SetActive(false);

        confidenceSimulationsListScreen.SetActive(false);
        decisionSimulationsListScreen.SetActive(false);
        groupSimulationsListScreen.SetActive(false);
    }

    public void toLeaderboard()
    {
        leaderboardLogo.color= selectedIconColor;

        homeBodyLogo.color = unselectedIconColor;
        homeRoofLogo.color = unselectedIconColor;
        simulationLogo.color = unselectedIconColor;
        profileBodyLogo.color = unselectedIconColor;
        profileHeadLogo.color = unselectedIconColor;

        // Settings only Leaderboard Screen active
        leaderboardPanel.SetActive(true);
        bottomNavigationScreen.SetActive(true);

        // Setting all other pages as false in case it comes from any of these
        simulationsListScreen.SetActive(false);
        dashboardPanel.SetActive(false);

        confidenceSimulationsListScreen.SetActive(false);
        decisionSimulationsListScreen.SetActive(false);
        groupSimulationsListScreen.SetActive(false);
    }
}
