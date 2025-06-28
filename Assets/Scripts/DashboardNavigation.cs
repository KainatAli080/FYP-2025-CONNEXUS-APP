using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashboardNavigation : MonoBehaviour
{
    public void goToLogin()
    {
        SceneManager.LoadScene("Login Scene");
    }
}
