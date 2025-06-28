//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//using Firebase;
//using Firebase.Auth;
//using Firebase.Database;
//using System;
//using DA_Assets.Shared;

//public class UserAuthenticationScript : MonoBehaviour
//{
//    public AppNavigationScript _NavScript;

//    // Firebase Variable
//    [Header("Firebase")]
//    public DependencyStatus dependencyStatus;
//    private FirebaseAuth firebaseAuth;
//    private FirebaseUser firebaseUser;
//    private FirebaseDatabase firebaseDatabase;      // this is where our DB actually is
//    private DatabaseReference databaseReference;    // This is where we will the root we need (the parent we wanna access)

//    // Registeration Variable
//    [Header("Registeration")]
//    [SerializeField] private TMP_InputField username;
//    [SerializeField] private TMP_InputField email;
//    [SerializeField] private TMP_InputField password;
//    [SerializeField] private TMP_InputField passwordValidation;
//    [SerializeField] private TextMeshProUGUI passwordWarningMessage;
//    [SerializeField] private TextMeshProUGUI passwordValidationWarningMessage;

//    // Login Variables
//    [Header("Login")]
//    [SerializeField] private TMP_InputField loginEmail;
//    [SerializeField] private TMP_InputField loginPassword;
//    [SerializeField] private TextMeshProUGUI loginPasswordWarningMessage;

//    private void Awake()
//    {
//        // Checking if all of Firebases dependies are available
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//        {
//            dependencyStatus = task.Result;
//            if (dependencyStatus == DependencyStatus.Available)
//            {
//                // If everything chhecks out
//                InitializeFirebase();
//                // Checking if any user loggedin
//                CheckIfUserLoggedIn();
//            }
//            else 
//            {
//                Debug.Log("Could not resolve all of Firebases dependencies.");
//            }
//        });
//    }

//    private void InitializeFirebase()
//    {
//        Debug.Log("Initializing Firebase...");

//        if (FirebaseApp.DefaultInstance == null)
//        {
//            Debug.Log("FirebaseApp.DefaultInstance is null.");
//            return;
//        }

//        Debug.Log("FirebaseApp.DefaultInstance is valid.");
//        firebaseAuth = FirebaseAuth.DefaultInstance;
//        firebaseDatabase = FirebaseDatabase.DefaultInstance;

//        if (firebaseDatabase == null)
//        {
//            Debug.Log("firebaseDatabase is null.");
//            return;
//        }

//        databaseReference = firebaseDatabase.RootReference;

//        if (databaseReference == null)
//        {
//            Debug.Log("databaseReference is null. Ensure Firebase is initialized.");
//        }
//        else
//        {
//            Debug.Log("Firebase database reference initialized successfully.");
//        }        
//    }

//    private void CheckIfUserLoggedIn()
//    {
//        if(firebaseAuth != null)
//        {
//            firebaseUser = firebaseAuth.CurrentUser;

//            if (firebaseUser != null)
//            {
//                Debug.Log("User is already logged in: " + firebaseUser.Email);
//                // Checking if questionnaire filled
//                // If not, directing to questionnaire panel
//                int tc = PlayerPrefs.GetInt("Terms And Conditions", -1);
//                Debug.Log(tc);
//                if (tc == -1)
//                {
//                    Debug.Log("Terms and Conditions not agreed to, directing to terms and condition page.");
//                    // meaning not agreed
//                    _NavScript.termsAndConditionsNotAgreedTo();
//                }
//                else
//                {
//                    Debug.Log("Terms and Conditions agreed to, checking questionnaire.");
//                    // meaning agreed, so checking questionnaire
//                    int qf = PlayerPrefs.GetInt("QF", 0);
//                    if (qf == 0)
//                    {
//                        // meaning not filled
//                        Debug.Log("Questionnaire NOT filled, redirecting to questionnaire");
//                        _NavScript.ifQuestionnaireNotFilled();
//                    }
//                    else
//                    {
//                        // meaning filled, checking interests
//                        Debug.Log("Questionnaire filled. Checking to interests");
//                        int interests = PlayerPrefs.GetInt("Interests", 0);
//                        if (interests == 0)
//                        {
//                            //meaning not saved
//                            Debug.Log("Interests not saved, redierecting to interests");
//                            _NavScript.ifInterestsNotFilled();
//                        }
//                        else
//                        {
//                            // meaning saved
//                            Debug.Log("Interests saved, redirecting to Dashboard");
//                            _NavScript.toDashboard();
//                        }
//                    }
//                }

//            }
//            else
//            {
//                Debug.Log("No user is logged in. Redirect to login screen.");
//                _NavScript.toLogin();
//            }
//        }
//        else
//        {
//            Debug.Log("Firebase Auth Empty");
//        }
//    }


//    // --------------------------------------------------------------------------------- //
//    //                              Registeration                                        //
//    // --------------------------------------------------------------------------------- //

//    public void dummy_registerButtonClicked()
//    {
//        _NavScript.fromSignUpToWelcomeP1();
//    }

//    public void registerButtonClicked()
//    {
//        // Only allowing registeration if all fields are filled
//        if(username.text == "")
//        {
//            passwordValidationWarningMessage.text = "Username Missing...";
//            return;
//        }
//        else if (email.text == "")
//        {
//            passwordValidationWarningMessage.text = "Email is Missing...";
//            return;
//        }
//        else if (password.text == "")
//        {
//            passwordValidationWarningMessage.text = "Password is Missing...";
//            return;
//        }
//        else if (passwordValidation.text == "")
//        {
//            passwordValidationWarningMessage.text = "Confirmation Password is Missing...";
//            return;
//        }
//        else
//        {
//            StartCoroutine(registerNewUser(username.text, email.text, password.text, passwordValidation.text));
//            //StartCoroutine(testCode());
//        }
//    }

//    private IEnumerator registerNewUser(string _username, string _email, string _password, string _passwordValidation)
//    {
//        if (_password != _passwordValidation)
//        {
//            passwordWarningMessage.text = "Passwords Do Not Match.";
//            yield break;
//        }
//        else 
//        {
//            // Calling Firebase Auth Sign Up Function
//            var RegisterTask = firebaseAuth.CreateUserWithEmailAndPasswordAsync(_email, _password);
//            // Now we wait until the task completes before perfroming next ones
//            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted || RegisterTask.IsFaulted);

//            if (RegisterTask.Exception != null)
//            {
//                // if there were any errors, we handle them
//                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
//                FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;

//                if(firebaseException != null)
//                {
//                    AuthError errorCode = (AuthError)firebaseException.ErrorCode;
//                    string message = "Register Failed";
//                    switch (errorCode)
//                    {
//                        case AuthError.MissingEmail:
//                            message = "Missing Email";
//                            break;
//                        case AuthError.MissingPassword:
//                            message = "Missing Password";
//                            break;
//                        case AuthError.WeakPassword:
//                            message = "Weak Password";
//                            break;
//                        case AuthError.EmailAlreadyInUse:
//                            message = "Email already in use";
//                            break;
//                    }
//                    passwordValidationWarningMessage.text = message;
//                }
//                else
//                {
//                    Debug.LogWarning("An unexpected error occurred.");
//                }
//            }
//            else 
//            {
//                // User successfully created
//                // Now we get the results
//                firebaseUser = RegisterTask.Result.User;                

//                // Creating User node in Firebase Realtime DB
//                if (firebaseUser != null)
//                {
//                    UserProfile userProfile = new UserProfile() { DisplayName = _username};
//                    var profileTask = firebaseUser.UpdateUserProfileAsync(userProfile);
//                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted || profileTask.IsFaulted);

//                    if (profileTask.Exception != null)
//                    {
//                        // Handling any errors
//                        Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
//                        FirebaseException firebaseException = profileTask.Exception.GetBaseException() as FirebaseException;
//                        AuthError errorCode = (AuthError)firebaseException.ErrorCode;
//                        passwordWarningMessage.text = "Username Set Failed.";
//                    }
//                    else 
//                    {
//                        passwordWarningMessage.text = "User registered";

//                        // --------------------------------------------------------------------------- //
//                        //                          Firebase Real-time Database                        // 
//                        // --------------------------------------------------------------------------- //

//                        User newUser = new User
//                        {
//                            Username = _username,
//                            Email = _email,
//                            UID = firebaseUser.UserId
//                        };
//                        string jsonUser = JsonUtility.ToJson(newUser);
//                        Debug.Log("Json User: " + jsonUser);
//                        Debug.Log("Firebase UID: " + firebaseUser.UserId);
//                        //var dbTask = databaseReference.Child("Users").Child(firebaseUser.UserId).SetRawJsonValueAsync(jsonUser);
//                        var usersNode = databaseReference?.Child("Users");
//                        if (usersNode == null)
//                        {
//                            Debug.LogError("Child('Users') returned null.");
//                            yield break;
//                        }

//                        var userNode = usersNode.Child(firebaseUser.UserId);
//                        if (userNode == null)
//                        {
//                            Debug.LogError($"Child('{firebaseUser.UserId}') returned null.");
//                            yield break;
//                        }
//                        var dbTask = userNode.SetRawJsonValueAsync(jsonUser);

//                        yield return new WaitUntil(() => dbTask.IsCompleted || dbTask.IsFaulted);

//                        if (dbTask.Exception != null)
//                        {
//                            Debug.LogWarning($"Failed to add user to database: {dbTask.Exception}");
//                            passwordWarningMessage.text = "Failed to save user data.";
//                        }
//                        else
//                        {
//                            Debug.Log("User data added to the database.");
//                            passwordWarningMessage.text = "User registered successfully!";
//                            yield return new WaitForSeconds(3);
//                            _NavScript.fromSignUpToWelcomeP1();
//                        }

//                        // --------------------------------------------------------------------------- //
//                        // --------------------------------------------------------------------------- //
//                        // --------------------------------------------------------------------------- //
//                    }
//                }
//                else
//                {
//                    Debug.LogError("firebaseUser is null. Registration might have failed.");
//                    passwordWarningMessage.text = "Something went wrong. Please try again.";
//                }
//            }
//        }
//    }

//    // --------------------------------------------------------------------------------- //
//    //                                     Login                                         //
//    // --------------------------------------------------------------------------------- //

//    public void dummy_loginButtonClicked()
//    {
//        _NavScript.toDashboard();
//    }

//    public void loginButtonClicked()
//    {
//        if (loginEmail.text == "")
//        {
//            loginPasswordWarningMessage.text = "Email is Missing...";
//            return;
//        }
//        else if (loginPassword.text == "")
//        {
//            loginPasswordWarningMessage.text = "Password is Missing...";
//            return;
//        }
//        else
//        {
//            StartCoroutine(loginUser(loginEmail.text, loginPassword.text));
//            //StartCoroutine(loginUser("kainat00@gmail.com", "kainat00"));
//        }        
//    }

//    private IEnumerator loginUser(string _email, string _password)
//    {
//        Debug.Log("Email: {email}, Password: {password}");
//        var loginTask = firebaseAuth.SignInWithEmailAndPasswordAsync(_email, _password);
//        yield return new WaitUntil(predicate: () => loginTask.IsCompleted || loginTask.IsFaulted);

//        if (loginTask.Exception != null)
//        {
//            // Handling any errors
//            Debug.LogWarning(message: $"Failed login task with {loginTask.Exception.Message}");
//            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;

//            AuthError errorCode = (AuthError)firebaseException.ErrorCode;
//            string message = "Login Failed";
//            switch (errorCode)
//            {
//                case AuthError.MissingEmail:
//                    message = "Missing Email";
//                    break;
//                case AuthError.MissingPassword:
//                    message = "Missing Password";
//                    break;
//                case AuthError.WeakPassword:
//                    message = "Wrong Password";
//                    break;
//                case AuthError.InvalidEmail:
//                    message = "Email invalid";
//                    break;
//                case AuthError.UserNotFound:
//                    message = "User does not exist";
//                    break;
//            }
//            loginPasswordWarningMessage.text = message;
//        }
//        else
//        {
//            firebaseUser = loginTask.Result.User;
//            Debug.LogFormat("User signed in successfully: {0} ({1})", firebaseUser.DisplayName, firebaseUser.Email);
//            loginPasswordWarningMessage.text = "Logged In.";
//            yield return new WaitForSeconds(3);
//            _NavScript.toDashboard();
//        }
//    }

//    IEnumerator testCode()
//    {
//        var testTask = databaseReference.Child("TestNode").SetValueAsync("TestValue");
//        yield return new WaitUntil(() => testTask.IsCompleted || testTask.IsFaulted);

//        if (testTask.IsCompleted)
//        {
//            Debug.Log("TestNode created successfully.");
//        }
//        else
//        {
//            Debug.LogError("Failed to create TestNode: " + testTask.Exception);
//        }
//    }

//    // --------------------------------------------------------------------------------- //
//    //                                  LOGOUT                                           //
//    // --------------------------------------------------------------------------------- //

//    public void LogoutUser()
//    {
//        firebaseAuth.SignOut();
//        Debug.Log("User logged out.");
//        _NavScript.loggedOut();
//    }

//    // --------------------------------------------------------------------------------- //
//    //                               Storing Scenario                                    //
//    // --------------------------------------------------------------------------------- //

//    public void storeUserScenarioChoice(string scenarioName)
//    {
//        // this gets the currently logged-in user
//        firebaseUser = firebaseAuth.CurrentUser;
//        string userId = "";

//        if (firebaseUser != null)
//        {
//            userId = firebaseUser.UserId; // Retrieving the user ID
//        }

//        var storeScenario = databaseReference?.Child("Users").Child(userId).Child("Chosen Scenarios").SetValueAsync(scenarioName).ContinueWith(task =>
//        {
//            if (task.IsCompleted)
//            {
//                Debug.Log("Scenario name stored successfully!");
//            }
//            else
//            {
//                Debug.LogError("Failed to store scenario name: " + task.Exception);
//            }
//        });
//    }
//}
