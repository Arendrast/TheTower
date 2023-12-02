using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Crystal : Currency
{
    private const string NameCrystalPlayerPref = "Crystal";

    private new void Awake()
    {
        nameVariableInPlayerPref = NameCrystalPlayerPref;
        base.Awake();
    }
}
