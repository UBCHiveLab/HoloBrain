// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitAction : MonoBehaviour
{
    GameObject digitHandlerScript;
    public string buttonName;
    public LoginUIPositionManager positionManager;

    void Start()
    {
        digitHandlerScript = GameObject.Find("DigitHandler");
    }

    public void OnSelect()
    {
        if (buttonName != "")
        {
            Debug.Log("IN ON SELECT");
            digitHandlerScript.GetComponent<DigitHandler>().OnTap(buttonName);
        }
    }

    void OnStartGaze()
    {
        positionManager.OnGazeEnteredUI();
    }

    void OnEndGaze()
    {
        positionManager.OnGazeExitUI();
    }
}
