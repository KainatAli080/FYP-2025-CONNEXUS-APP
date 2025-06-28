using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualAnimationsScipt : MonoBehaviour
{
    [Header("Login Screen Animation Variables")]
    [SerializeField] private RectTransform[] login_slideUp_UIelements;
    [SerializeField] private RectTransform[] login_dropDown_UIelements;
    [SerializeField] private float animationDuration = 5f;                
    [SerializeField] private Ease easeType = Ease.OutQuad;                  

    private Vector2[] login_slideUpElements_originalPositions;
    private Vector2[] login_dropDownElements_originalPositions;

    [Header("Sign Up Screen Animation Variables")]
    [SerializeField] private RectTransform[] signup_slideUp_UIelements;
    [SerializeField] private RectTransform[] signup_dropDown_UIelements;
    [SerializeField] private Ease signUp_easeType = Ease.OutQuad;

    private Vector2[] signUp_slideUpElements_originalPositions;
    private Vector2[] signUp_dropDownElements_originalPositions;

    private void Start()
    {
        // -------------------------------------------------------------------------------------------------- //
        //                                  LOGIN SCREEN STORAGE                                              //
        // -------------------------------------------------------------------------------------------------- //
        // Save the original anchored positions Slide Up Elements
        login_slideUpElements_originalPositions = new Vector2[login_slideUp_UIelements.Length];
        for (int i = 0; i < login_slideUp_UIelements.Length; i++)
        {
            login_slideUpElements_originalPositions[i] = login_slideUp_UIelements[i].anchoredPosition;            
            login_slideUp_UIelements[i].anchoredPosition -= new Vector2(0, Screen.height);      // Move elements to an "offscreen" position (e.g., below the screen)
        }
        // Save the original anchored positions Drop Down Elements
        login_dropDownElements_originalPositions = new Vector2[login_dropDown_UIelements.Length];
        for(int i = 0; i < login_dropDown_UIelements.Length; i++)
        {
            login_dropDownElements_originalPositions[i] = login_dropDown_UIelements[i].anchoredPosition;
            login_dropDown_UIelements[i].anchoredPosition += new Vector2(0, Screen.height);     // Move elements to an "offscreen" position (e.g., above the screen)
        }


        // -------------------------------------------------------------------------------------------------- //
        //                                  SIGNUP SCREEN ANIMATIONS                                          //
        // -------------------------------------------------------------------------------------------------- //
        signUp_dropDownElements_originalPositions = new Vector2[signup_dropDown_UIelements.Length];
        for( int i = 0; i < signup_dropDown_UIelements.Length; i++)
        {
            signUp_dropDownElements_originalPositions[i] = signup_dropDown_UIelements[i].anchoredPosition;
            signup_dropDown_UIelements[i].anchoredPosition += new Vector2(0, Screen.height);
        }

        signUp_slideUpElements_originalPositions = new Vector2[signup_slideUp_UIelements.Length];
        for (int i = 0; i < signup_slideUp_UIelements.Length; i++)
        {
            signUp_slideUpElements_originalPositions[i] = signup_slideUp_UIelements[i].anchoredPosition;
            signup_slideUp_UIelements[i].anchoredPosition -= new Vector2(0, Screen.height);
        }
    }

    // -------------------------------------------------------------------------------------------------- //
    // --------------------------------- LOGIN SCREEN START ANIMATIONS  ------------------------------------- //
    // -------------------------------------------------------------------------------------------------- //

    public void loginPanel_startAnimation()
    {
        login_dropDownElements_animation();
        login_slideUpElements_animation();
    }

    private void login_dropDownElements_animation()
    {
        for(int i = 0; i < login_dropDown_UIelements.Length; i++)
        {
            login_dropDown_UIelements[i].DOAnchorPos(login_dropDownElements_originalPositions[i], animationDuration).SetEase(easeType);
        }
    }

    private void login_slideUpElements_animation()
    {
        // Animate each element to its original position
        for (int i = 0; i < login_slideUp_UIelements.Length; i++)
        {
            login_slideUp_UIelements[i].DOAnchorPos(login_slideUpElements_originalPositions[i], animationDuration).SetEase(easeType);
        }
    }

    // ----------------------------------------------------------------------------------------------------- //
    // --------------------------------- LOGIN SCREEN EXIT ANIMATIONS  ------------------------------------- //
    // ----------------------------------------------------------------------------------------------------- //

    public void loginPanel_exitAnimation()
    {
        login_slideBackDownElements_animation();
        login_dropBackUpElements_animation();
    }

    private void login_slideBackDownElements_animation()
    {
        // Animate each element back to the "offscreen" position
        for (int i = 0; i < login_slideUp_UIelements.Length; i++)
        {
            login_slideUp_UIelements[i].DOAnchorPos(login_slideUpElements_originalPositions[i] - new Vector2(0, Screen.height), animationDuration).SetEase(easeType);
        }
    }

    private void login_dropBackUpElements_animation()
    {
        for(int i = 0; i<login_dropDown_UIelements.Length; i++)
        {
            login_dropDown_UIelements[i].DOAnchorPos(login_dropDownElements_originalPositions[i] + new Vector2(0, Screen.height), animationDuration).SetEase(easeType);
        }
    }

    // -------------------------------------------------------------------------------------------------------- //
    //                                  SIGNUP SCREEN START ANIMATIONS                                          //
    // -------------------------------------------------------------------------------------------------------- //

    public void signupPanel_startAnimation()
    {
        signup_dropDownElements_animation();
        signup_slideUpElements_animation();
    }

    private void signup_dropDownElements_animation()
    {
        for (int i = 0; i < signup_dropDown_UIelements.Length; i++)
        {
            signup_dropDown_UIelements[i].DOAnchorPos(signUp_dropDownElements_originalPositions[i], animationDuration).SetEase(signUp_easeType);
        }
    }

    private void signup_slideUpElements_animation()
    {
        // Animate each element to its original position
        for (int i = 0; i < signup_slideUp_UIelements.Length; i++)
        {
            signup_slideUp_UIelements[i].DOAnchorPos(signUp_slideUpElements_originalPositions[i], animationDuration).SetEase(signUp_easeType);
        }
    }

    // -------------------------------------------------------------------------------------------------------- //
    //                                  SIGNUP SCREEN END ANIMATIONS                                            //
    // -------------------------------------------------------------------------------------------------------- //

    public void signupPanel_exitAnimation()
    {
        signup_slideBackDownElements_animation();
        signup_dropBackUpElements_animation();
    }

    private void signup_slideBackDownElements_animation()
    {
        // Animate each element back to the "offscreen" position
        for (int i = 0; i < signup_slideUp_UIelements.Length; i++)
        {
            signup_slideUp_UIelements[i].DOAnchorPos(signUp_slideUpElements_originalPositions[i] - new Vector2(0, Screen.height), animationDuration).SetEase(easeType);
        }
    }

    private void signup_dropBackUpElements_animation()
    {
        for (int i = 0; i < signup_dropDown_UIelements.Length; i++)
        {
            signup_dropDown_UIelements[i].DOAnchorPos(signUp_dropDownElements_originalPositions[i] + new Vector2(0, Screen.height), animationDuration).SetEase(easeType);
        }
    }
}
