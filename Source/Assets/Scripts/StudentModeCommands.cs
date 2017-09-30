// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentModeCommands : MonoBehaviour {
    private const string BRAIN_UI_GAMEOBJECT_NAME = "BrainUI";
    private const string CONTROLS_UI_GAMEOBJECT_NAME = "ControlsUI";
    
    private GameObject brainUI;
    private GameObject controlsUI;
    private Vector3 visibleBrainUIScale;
    private Vector3 visibleControlsUIScale;
    private Vector3 hiddenUIScale;
    private bool controlsUiIsHidden;
    private bool statusUiIsHidden;

	// Use this for initialization
	void Start () {
        brainUI = GameObject.Find(BRAIN_UI_GAMEOBJECT_NAME);
        controlsUI = GameObject.Find(CONTROLS_UI_GAMEOBJECT_NAME);
        visibleBrainUIScale = brainUI.transform.localScale;
        visibleControlsUIScale = controlsUI.transform.localScale;
        hiddenUIScale = new Vector3(0, 0, 0);
        SetUIVisibilityUI(false);
    }

    void OnSelect()
    {
    }

    public void SetUIVisibilityUI(bool showUI)
    {
        ToggleControlsUI(showUI);
        ToggleStatusUI(showUI);
    }

    public void ToggleControlsUI(bool showUI)
    {
        controlsUiIsHidden = !showUI;
        try
        {
            controlsUI.transform.localScale = controlsUiIsHidden ? hiddenUIScale : visibleControlsUIScale;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("StudentModeCommands: Could not find UI");
        }
    }

    public void ToggleStatusUI(bool showUI)
    {
        statusUiIsHidden = !showUI;
        try
        {
            brainUI.transform.localScale = statusUiIsHidden ? hiddenUIScale : visibleBrainUIScale;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("StudentModeCommands: Could not find UI");
        }
    }

    public bool CurrentlyInStudentMode()
    {
        return controlsUiIsHidden;
    }
}
