using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This allows Unity’s JsonUtility to serialize the User class into a JSON format, making it compatible with Firebase Realtime Database.
[Serializable]
public class User
{
    [SerializeField] private string username;
    [SerializeField] private string email;
    [SerializeField] private string uid;

    public string Username { get => username; set => username = value; }
    public string Email { get => email; set => email = value; }
    public string UID { get => uid; set => uid = value; }
    // Attempting to modify UID will cause a compile-time error
}