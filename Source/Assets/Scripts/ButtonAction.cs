// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour {

    public string buttonName;

    public LoginUIPositionManager positionManager;

    GameObject modeSelectionScript;
	// Use this for initialization
	void Start ()
    {
        modeSelectionScript = GameObject.Find("ModeSelectionScript");
    }

    public void OnSelect()
    {
        if (buttonName != "")
        {
            Debug.Log("IN ON SELECT");
            modeSelectionScript.GetComponent<ModeSelection>().OnTap(buttonName);
        }
    }

    void OnStartGaze()
    {
        if (positionManager != null)
        {
            positionManager.OnGazeEnteredUI();
        }
    }

    void OnEndGaze()
    {
        if (positionManager != null)
        {
            positionManager.OnGazeExitUI();
        }
    }
}
