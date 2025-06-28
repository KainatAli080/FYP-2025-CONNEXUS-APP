using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginNavigation : MonoBehaviour
{
    public void goToDashboard()
    {
        SceneManager.LoadScene("Dashboard");
    }
}
