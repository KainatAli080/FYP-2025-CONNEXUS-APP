using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Interests
{
    public List<string> interests;

    public Interests(List<string> interests)
    {
        this.interests = interests;
    }
}
