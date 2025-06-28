using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class FirebaseFunctions : MonoBehaviour
{
    public static FirebaseFunctions instance;
    public AppNavigationScript _NavScript;

    // Firebase Variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    private FirebaseAuth firebaseAuth;
    private FirebaseUser firebaseUser;
    private FirebaseDatabase firebaseDatabase;      // this is where our DB actually is
    private DatabaseReference databaseReference;    // This is where we will the root we need (the parent we wanna access)

    // Registeration Variable
    [Header("Registeration")]
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField passwordValidation;
    [SerializeField] private TextMeshProUGUI passwordWarningMessage;
    [SerializeField] private TextMeshProUGUI passwordValidationWarningMessage;

    // Login Variables
    [Header("Login")]
    [SerializeField] private TMP_InputField loginEmail;
    [SerializeField] private TMP_InputField loginPassword;
    [SerializeField] private TextMeshProUGUI loginPasswordWarningMessage;

    [Header("Static Variables")]
    public static DashboardMetrics dashboardMetrics;

    int tc;
    int qf;
    int interests;

    private void Awake()
    {
        tc = PlayerPrefs.GetInt("Terms And Conditions", -1); 
        qf = PlayerPrefs.GetInt("QF", 0);
        interests = PlayerPrefs.GetInt("Interests", 0);

        if (instance == null)
        {
            instance = this;

            // Checking if all of Firebases dependies are available
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                Debug.Log("Dependies reolved");
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // If everything chhecks out
                    InitializeFirebase();   
                    CheckIfUserLoggedIn();
                }
                else
                {
                    Debug.Log("Could not resolve all of Firebases dependencies.");
                }                
            });
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        dashboardMetrics = new DashboardMetrics();
    }
    private void InitializeFirebase()
    {
        Debug.Log("Initializing Firebase...");

        if (FirebaseApp.DefaultInstance == null)
        {
            Debug.Log("FirebaseApp.DefaultInstance is null.");
            return;
        }
        Debug.Log("FirebaseApp.DefaultInstance is valid.");

        firebaseAuth = FirebaseAuth.DefaultInstance;
        if (firebaseAuth == null)
        {
            Debug.Log("FirebaseAuth is null.");
            return;
        }
        Debug.Log("FirebaseAuth is valid.");

        firebaseDatabase = FirebaseDatabase.DefaultInstance;
        if (firebaseDatabase == null)
        {
            Debug.Log("firebaseDatabase is null.");
            return;
        }

        databaseReference = firebaseDatabase.RootReference;
        if (databaseReference == null)
        {
            Debug.Log("databaseReference is null. Ensure Firebase is initialized.");
        }
        else
        {
            Debug.Log("Firebase database reference initialized successfully.");
        }

        CheckIfUserLoggedIn();
    }

    private void CheckIfUserLoggedIn()
    {
        Debug.Log("Checking id User is logged in...");
        if (firebaseAuth != null)
        {
            firebaseUser = firebaseAuth.CurrentUser;

            if (firebaseUser != null)
            {
                Debug.Log("User is already logged in: " + firebaseUser.Email);
                // Checking if questionnaire filled
                // If not, directing to questionnaire panel

                // StartCoroutine(waitForSplashscreenBeforeChecking());

            }
            else
            {
                Debug.Log("No user is logged in. Redirect to login screen.");
                _NavScript.toLogin();
            }
        }
        else
        {
            Debug.Log("Firebase Auth Empty");
        }
    }

    // Will fix this later, pretty suure the issue is in threads, UI needs execution on Main Execution so
    // this must not be getting called on the main thread, even without the coroutine
    private IEnumerator waitForSplashscreenBeforeChecking()
    {
        Debug.LogError("Coroutine Started: Checking terms and conditions and questionnaire");
        yield return new WaitForSeconds(0f);
        if (tc == -1)
        {
            Debug.Log("Terms and Conditions not agreed to, directing to terms and condition page.");
            // meaning not agreed
            _NavScript.termsAndConditionsNotAgreedTo();
        }
        else
        {
            Debug.Log("Terms and Conditions agreed to, checking questionnaire.");
            // meaning agreed, so checking questionnaire                    
            if (qf == 0)
            {
                // meaning not filled
                Debug.Log("Questionnaire NOT filled, redirecting to questionnaire");
                _NavScript.ifQuestionnaireNotFilled();
                // Trying to execute it on mainthread, Unity sometimes blocks UI changes on BG threads (and idk if this is running on main or BG)
                
            }
            else
            {
                // meaning filled, checking interests
                Debug.Log("Questionnaire filled. Checking to interests");
                if (interests == 0)
                {
                    //meaning not saved
                    Debug.Log("Interests not saved, redierecting to interests");
                    _NavScript.ifInterestsNotFilled();
                }
                else
                {
                    // meaning saved
                    Debug.Log("Interests saved, redirecting to Dashboard");
                    _NavScript.toDashboard();
                }
            }
        }
    }


    // --------------------------------------------------------------------------------- //
    //                              Registeration                                        //
    // --------------------------------------------------------------------------------- //

    public void dummy_registerButtonClicked()
    {
        _NavScript.fromSignUpToWelcomeP1();
    }

    public void registerButtonClicked()
    {
        // Only allowing registeration if all fields are filled
        if (username.text == "")
        {
            passwordValidationWarningMessage.text = "Username Missing...";
            return;
        }
        else if (email.text == "")
        {
            passwordValidationWarningMessage.text = "Email is Missing...";
            return;
        }
        else if (password.text == "")
        {
            passwordValidationWarningMessage.text = "Password is Missing...";
            return;
        }
        else if (passwordValidation.text == "")
        {
            passwordValidationWarningMessage.text = "Confirmation Password is Missing...";
            return;
        }
        else
        {
            StartCoroutine(registerNewUser(username.text, email.text, password.text, passwordValidation.text));
            //StartCoroutine(testCode());
        }
    }

    private IEnumerator registerNewUser(string _username, string _email, string _password, string _passwordValidation)
    {
        if (_password != _passwordValidation)
        {
            passwordWarningMessage.text = "Passwords Do Not Match.";
            yield break;
        }
        else
        {
            // Calling Firebase Auth Sign Up Function
            var RegisterTask = firebaseAuth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            // Now we wait until the task completes before perfroming next ones
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted || RegisterTask.IsFaulted);

            if (RegisterTask.Exception != null)
            {
                // if there were any errors, we handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;

                if (firebaseException != null)
                {
                    AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                    string message = "Register Failed";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email already in use";
                            break;
                    }
                    passwordValidationWarningMessage.text = message;
                }
                else
                {
                    Debug.LogWarning("An unexpected error occurred.");
                }
            }
            else
            {
                // User successfully created
                // Now we get the results
                firebaseUser = RegisterTask.Result.User;

                // Creating User node in Firebase Realtime DB
                if (firebaseUser != null)
                {
                    UserProfile userProfile = new UserProfile() { DisplayName = _username };
                    var profileTask = firebaseUser.UpdateUserProfileAsync(userProfile);
                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted || profileTask.IsFaulted);

                    if (profileTask.Exception != null)
                    {
                        // Handling any errors
                        Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                        FirebaseException firebaseException = profileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseException.ErrorCode;
                        passwordWarningMessage.text = "Username Set Failed.";
                    }
                    else
                    {
                        passwordWarningMessage.text = "User registered";

                        // --------------------------------------------------------------------------- //
                        //                          Firebase Real-time Database                        // 
                        // --------------------------------------------------------------------------- //

                        User newUser = new User
                        {
                            Username = _username,
                            Email = _email,
                            UID = firebaseUser.UserId
                        };
                        string jsonUser = JsonUtility.ToJson(newUser);
                        Debug.Log("Json User: " + jsonUser);
                        Debug.Log("Firebase UID: " + firebaseUser.UserId);
                        //var dbTask = databaseReference.Child("Users").Child(firebaseUser.UserId).SetRawJsonValueAsync(jsonUser);
                        var usersNode = databaseReference?.Child("Users");
                        if (usersNode == null)
                        {
                            Debug.LogError("Child('Users') returned null.");
                            yield break;
                        }

                        var userNode = usersNode.Child(firebaseUser.UserId);
                        if (userNode == null)
                        {
                            Debug.LogError($"Child('{firebaseUser.UserId}') returned null.");
                            yield break;
                        }
                        var dbTask = userNode.SetRawJsonValueAsync(jsonUser);

                        yield return new WaitUntil(() => dbTask.IsCompleted || dbTask.IsFaulted);

                        if (dbTask.Exception != null)
                        {
                            Debug.LogWarning($"Failed to add user to database: {dbTask.Exception}");
                            passwordWarningMessage.text = "Failed to save user data.";
                        }
                        else
                        {
                            Debug.Log("User data added to the database.");
                            passwordWarningMessage.text = "User registered successfully!";
                            yield return new WaitForSeconds(3);
                            _NavScript.fromSignUpToWelcomeP1();
                        }

                        // --------------------------------------------------------------------------- //
                        // --------------------------------------------------------------------------- //
                        // --------------------------------------------------------------------------- //
                    }
                }
                else
                {
                    Debug.LogError("firebaseUser is null. Registration might have failed.");
                    passwordWarningMessage.text = "Something went wrong. Please try again.";
                }
            }
        }
    }

    // --------------------------------------------------------------------------------- //
    //                                     Login                                         //
    // --------------------------------------------------------------------------------- //

    public void dummy_loginButtonClicked()
    {
        _NavScript.toDashboard();
    }

    public void loginButtonClicked()
    {
        //if (loginEmail.text == "")
        //{
        //    loginPasswordWarningMessage.text = "Email is Missing...";
        //    return;
        //}
        //else if (loginPassword.text == "")
        //{
        //    loginPasswordWarningMessage.text = "Password is Missing...";
        //    return;
        //}
        //else
        {
            //StartCoroutine(loginUser(loginEmail.text, loginPassword.text));
            StartCoroutine(loginUser("kainat55@gmail.com", "kainat55"));
        }
    }

    //private IEnumerator loginUser(string _email, string _password)
    //{
    //    Debug.Log("Email: " + _email + ", Password: " + _password);
    //    var loginTask = firebaseAuth.SignInWithEmailAndPasswordAsync(_email, _password);
    //    yield return new WaitUntil(predicate: () => loginTask.IsCompleted || loginTask.IsFaulted);

    //    if (loginTask.Exception != null)
    //    {
    //        // Handling any errors
    //        Debug.LogWarning(message: $"Failed login task with {loginTask.Exception.Message}");
    //        FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;

    //        AuthError errorCode = (AuthError)firebaseException.ErrorCode;
    //        string message = "Login Failed";
    //        switch (errorCode)
    //        {
    //            case AuthError.MissingEmail:
    //                message = "Missing Email";
    //                break;
    //            case AuthError.MissingPassword:
    //                message = "Missing Password";
    //                break;
    //            case AuthError.WeakPassword:
    //                message = "Wrong Password";
    //                break;
    //            case AuthError.InvalidEmail:
    //                message = "Email invalid";
    //                break;
    //            case AuthError.UserNotFound:
    //                message = "User does not exist";
    //                break;
    //        }
    //        loginPasswordWarningMessage.text = message;
    //    }
    //    else
    //    {
    //        firebaseUser = loginTask.Result.User;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})", firebaseUser.DisplayName, firebaseUser.Email);
    //        loginPasswordWarningMessage.text = "Logged In.";
    //        RetrieveDashboardMetrics();
    //        yield return new WaitForSeconds(3);
    //        Debug.Log("Dashboard metrics: Confidence: " + dashboardMetrics.ConfidenceAverage);
    //        _NavScript.toDashboard();
    //    }
    //}

    private IEnumerator loginUser(string _email, string _password)
    {
        Debug.Log("Email: " + _email + ", Password: " + _password);
        var loginTask = firebaseAuth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(() => loginTask.IsCompleted || loginTask.IsFaulted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning($"Failed login task with {loginTask.Exception.Message}");
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;

            string message = "Login Failed";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email"; break;
                case AuthError.MissingPassword:
                    message = "Missing Password"; break;
                case AuthError.WeakPassword:
                    message = "Wrong Password"; break;
                case AuthError.InvalidEmail:
                    message = "Email invalid"; break;
                case AuthError.UserNotFound:
                    message = "User does not exist"; break;
            }

            loginPasswordWarningMessage.text = message;
        }
        else
        {
            firebaseUser = loginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", firebaseUser.DisplayName, firebaseUser.Email);
            loginPasswordWarningMessage.text = "Logged In.";

            // Calling the async function through wrapper
            RetrieveDashboardMetricsWrapper();

            yield return new WaitForSeconds(3);

            Debug.Log("Dashboard metrics: Confidence: " + dashboardMetrics.ConfidenceAverage);
            _NavScript.toDashboard();
        }
    }


    IEnumerator testCode()
    {
        var testTask = databaseReference.Child("TestNode").SetValueAsync("TestValue");
        yield return new WaitUntil(() => testTask.IsCompleted || testTask.IsFaulted);

        if (testTask.IsCompleted)
        {
            Debug.Log("TestNode created successfully.");
        }
        else
        {
            Debug.LogError("Failed to create TestNode: " + testTask.Exception);
        }
    }

    // --------------------------------------------------------------------------------- //
    //                                  LOGOUT                                           //
    // --------------------------------------------------------------------------------- //

    public void LogoutUser()
    {
        firebaseAuth.SignOut();
        Debug.Log("User logged out.");
        _NavScript.loggedOut();
    }

    // --------------------------------------------------------------------------------- //
    //                                    Storing                                        //
    // --------------------------------------------------------------------------------- //

    public void storeUserScenarioChoice(string scenarioName)
    {
        firebaseUser = firebaseAuth.CurrentUser;
        if (firebaseUser == null)
        {
            Debug.LogError("No user is logged in.");
            return;
        }

        string userId = firebaseUser.UserId;

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "Chosen Scenarios", scenarioName }
        };

        databaseReference.Child("Users").Child(userId).UpdateChildrenAsync(updates)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Scenario name stored successfully!");
                }
                else
                {
                    Debug.LogError("Failed to store scenario name: " + task.Exception);
                }
            });
    }

    public void SaveQuestionnaire(int[] answers, List<int> trigger3)
    {
        if (firebaseUser == null)
        {
            Debug.LogError("No user is logged in.");
            return;
        }

        // Convert QuestionnaireData to a Dictionary
        Dictionary<string, object> questionnaireData = new Dictionary<string, object>
        {
            { "Answer1", answers[0] },
            { "Answer2", answers[1] },
            { "Answer3", answers[2] },
            { "Answer4", answers[3] },
            { "Answer5", answers[4] },
            { "Answer6", answers[5] },
            { "Answer7", answers[6] },
            { "Trigger3", trigger3 }  // Lists are stored as arrays in Firebase
        };

        // Update Firebase with properly structured data
        databaseReference.Child("Users").Child(firebaseUser.UserId).Child("Questionnaire")
            .UpdateChildrenAsync(questionnaireData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("Questionnaire saved successfully.");
                }
                else
                {
                    Debug.LogError("Failed to save questionnaire: " + task.Exception);
                }
            });
    }

    public void SaveIntroversionScore(int score)
    {
        if (firebaseUser == null)
        {
            Debug.LogError("No user is logged in.");
            return;
        }

        string path = "Users/" + firebaseUser.UserId;

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "Introversion Score", score }
        };

        databaseReference.Child(path).UpdateChildrenAsync(updates)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Introversion value stored successfully.");
                }
                else
                {
                    Debug.LogError("Failed to store introversion value: " + task.Exception);
                }
            });
    }

    public void SaveInterests(List<string> Interest)
    {
        // Wrap list in a serializable class
        //Interests data = new Interests(Interest);

        // Convert List<string> to Dictionary for Firebase
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "Interests", Interest }
        };

        // Update only the interests field in the user's data
        var updateTask = databaseReference.Child("Users")
            .Child(firebaseUser.UserId)
            .UpdateChildrenAsync(updates)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("Interests updated successfully.");
                }
                else
                {
                    Debug.LogError("Failed to update interests: " + task.Exception);
                }
            });
    }

    // --------------------------------------------------------------------------------- //
    //                                Retrieving Data                                    //
    // --------------------------------------------------------------------------------- //

    public async Task<SpeechAnalysisResult> RetrieveConfidenceEasyData()
    {
        if (firebaseUser == null)
        {
            Debug.LogError("No user logged in.");
        }

        DataSnapshot snapshot = await databaseReference.Child("Users")
            .Child(firebaseUser.UserId)
            .Child("Latest Results")
            .Child("CB_Easy")
            .GetValueAsync();

        if (!snapshot.Exists)
        {
            Debug.Log("No speech analysis data found in 'CB_Easy'.");
            return null; // Return null if no data exists
        }

        string jsonData = snapshot.GetRawJsonValue();
        SpeechAnalysisResult analysisResult = JsonConvert.DeserializeObject<SpeechAnalysisResult>(jsonData);

        if (analysisResult == null)
        {
            Debug.LogError("Failed to deserialize speech analysis result.");
            return null;
        }

        return analysisResult; // Return the deserialized object
    }

    public async Task<SpeechAnalysisResult> RetrieveGroupMediumData()
    {
        if (firebaseUser == null)
        {
            Debug.LogError("No user logged in.");
        }

        DataSnapshot snapshot = await databaseReference.Child("Users")
            .Child(firebaseUser.UserId)
            .Child("Latest Results")
            .Child("GI_Medium")
            .GetValueAsync();

        if (!snapshot.Exists)
        {
            Debug.Log("No speech analysis data found in 'GI_Medium'.");
            return null; // Return null if no data exists
        }

        string jsonData = snapshot.GetRawJsonValue();
        SpeechAnalysisResult analysisResult = JsonConvert.DeserializeObject<SpeechAnalysisResult>(jsonData);

        if (analysisResult == null)
        {
            Debug.LogError("Failed to deserialize speech analysis result.");
            return null;
        }

        return analysisResult; // Return the deserialized object
    }

    public async Task<SpeechAnalysisResult> RetrieveDecisionMediumData()
    {
        if (firebaseUser == null)
        {
            Debug.LogError("No user logged in.");
        }

        DataSnapshot snapshot = await databaseReference.Child("Users")
            .Child(firebaseUser.UserId)
            .Child("Latest Results")
            .Child("DM_Medium")
            .GetValueAsync();

        if (!snapshot.Exists)
        {
            Debug.Log("No speech analysis data found in 'DM_Medium'.");
            return null; // Return null if no data exists
        }

        string jsonData = snapshot.GetRawJsonValue();
        SpeechAnalysisResult analysisResult = JsonConvert.DeserializeObject<SpeechAnalysisResult>(jsonData);

        if (analysisResult == null)
        {
            Debug.LogError("Failed to deserialize speech analysis result.");
            return null;
        }

        return analysisResult; // Return the deserialized object
    }

    // using callback since firebase calls are asynchronous
    public string RetrieveUsername()
    {
        firebaseUser = firebaseAuth.CurrentUser;

        string username = string.Empty;
        return username;
    }

    public void GetUsername(System.Action<string> onUsernameRetrieved)
    {
        firebaseUser = firebaseAuth.CurrentUser;

        databaseReference.Child("Users").Child(firebaseUser.UserId).Child("Username").GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    string username = task.Result.Value.ToString();
                    Debug.Log("Username retrieved: " + username);
                    onUsernameRetrieved?.Invoke(username); // Callback function
                }
                else
                {
                    Debug.LogError("Failed to retrieve username: " + task.Exception);
                    onUsernameRetrieved?.Invoke(null);
                }
            });
    }

    //public async Task RetrieveDashboardMetrics()
    //{
    //    Debug.Log("RetrieveDashboardMetrics function called");

    //    if (firebaseUser == null)
    //    {
    //        Debug.LogError("No user logged in.");
    //        return;
    //    }

    //    string userId = firebaseUser.UserId;
    //    Debug.Log("UserID: " + userId);

    //    await databaseReference.Child("Users")
    //        .Child(userId)
    //        .Child("Dashboard Metrics")
    //        .GetValueAsync().ContinueWith(task =>
    //        {
    //            if (task.IsFaulted)
    //            {
    //                Debug.LogError("Error retrieving DashboardMetrics: " + task.Exception);
    //                return;
    //            }

    //            if (task.IsCompleted)
    //            {
    //                DataSnapshot snapshot = task.Result;
    //                if (snapshot.Exists)
    //                {
    //                    dashboardMetrics = JsonUtility.FromJson<DashboardMetrics>(snapshot.GetRawJsonValue());
    //                    Debug.Log("DashboardMetrics retrieved successfully.");
    //                }
    //            }
    //        });
    //}

    // Testing if this one works fisrt: try 2

    public async Task RetrieveDashboardMetrics()
    {
        Debug.Log("RetrieveDashboardMetrics function called");

        if (firebaseUser == null)
        {
            Debug.LogError("No user logged in.");
            return;
        }

        string userId = firebaseUser.UserId;
        Debug.Log("UserID: " + userId);

        try
        {
            DataSnapshot snapshot = await databaseReference
                .Child("Users")
                .Child(userId)
                .Child("Dashboard Metrics")
                .GetValueAsync();

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                if (!string.IsNullOrEmpty(json))
                {
                    dashboardMetrics = JsonUtility.FromJson<DashboardMetrics>(json);
                    Debug.Log("DashboardMetrics retrieved successfully.");
                }
                else
                {
                    Debug.LogWarning("DashboardMetrics JSON was null or empty.");
                }
            }
            else
            {
                Debug.LogWarning("No DashboardMetrics data found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error retrieving DashboardMetrics: " + ex.Message);
        }
    }

    // To help with the async call
    private async void RetrieveDashboardMetricsWrapper()
    {
        await RetrieveDashboardMetrics();
    }

    public void GetUserPoints(System.Action<int> onPointsRetrieved)
    {
        firebaseUser = firebaseAuth.CurrentUser;

        databaseReference.Child("Users").Child(firebaseUser.UserId).Child("Points").GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to retrieve points: " + task.Exception);
                    onPointsRetrieved?.Invoke(0); // default if failed
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists && int.TryParse(snapshot.Value.ToString(), out int points))
                    {
                        Debug.Log("Points retrieved: " + points);
                        onPointsRetrieved?.Invoke(points);
                    }
                    else
                    {
                        Debug.LogWarning("Points value missing or invalid.");
                        onPointsRetrieved?.Invoke(0);
                    }
                }
            });
    }


    // --------------------------------------------------------------------------------- //
    //                                Password Reset                                     //
    // --------------------------------------------------------------------------------- //

    public bool SendPasswordResetEmail(string userEmail)
    {
        firebaseAuth.SendPasswordResetEmailAsync(userEmail).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Reset email send canceled.");
                return false;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Error while sending reset email: " + task.Exception);
                return false;
            }

            Debug.Log("Password reset email sent successfully to: " + userEmail);
            return true;
        });
        return false;
    }
}
