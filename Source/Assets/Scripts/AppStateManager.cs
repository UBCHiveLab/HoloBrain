// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using System;

/// <summary>
/// Keeps track of the current state of the experience.
/// </summary>
public class AppStateManager : Singleton<AppStateManager>
{
    /// <summary>
    /// Enum to track progress through the experience.
    /// </summary>
    public enum AppState
    {
        Starting = 0,
        WaitingForAnchor,
        WaitingForStageTransform,
        Ready
    }

    private const string LOADING_SCREEN_GAMEOBJECT_NAME = "LoadingScreen";
    private const string CURSOR_GAMEOBJECT_NAME = "Cursor";
    private const string STATUSUI_GAMEOBJECT_NAME = "StatusUI";
    private const string CORTEX_GAMEOBJECT_NAME = "HologramCollection/Models/Brain/Cortex";
    private const string LABELDISPLAY_GAMEOBJECT_NAME = "LabelDisplay";

    private GameObject loadingScreen;
    private GameObject cursor;

    /// <summary>
    /// Tracks the current state in the experience.
    /// </summary>
    public AppState CurrentAppState { get; set; }

    override protected void Awake()
    {
        base.Awake();
        CurrentAppState = AppState.Starting;
        Debug.Log("AppStateManager: has just started and is WaitingForAnchor");

        loadingScreen = GameObject.Find(LOADING_SCREEN_GAMEOBJECT_NAME);
        cursor = GameObject.Find(CURSOR_GAMEOBJECT_NAME);
        cursor.SetActive(false);
    }

    public void ResetStage()
    {
        // If we fall back to waiting for anchor, everything needed to 
        // get us into setting the target transform state will be setup.
        Debug.Log("AppStateManager: the stage is being reset");
        CurrentAppState = AppState.Starting;
    }

    void Update()
    {
        switch (CurrentAppState)
        {
            case AppState.Starting:
                CurrentAppState = AppState.WaitingForStageTransform;
                loadingScreen.SetActive(true);
                //HTGestureManager.Instance.OverrideFocusedObject = HologramPlacement.Instance.gameObject;
                SpatialMappingManager.Instance.gameObject.SetActive(true);
                SpatialMappingManager.Instance.DrawVisualMeshes = true; // true;
                SpatialMappingManager.Instance.StartObserver();
                break;
            case AppState.WaitingForStageTransform:
                // Now if we have the stage transform we are ready to go.
                if (HologramPlacement.Instance.GotTransform)
                {
                    Debug.Log("got transform in appstatemanager");
                    CurrentAppState = AppState.Ready;
                    LoadUI();
                    Debug.Log("done with loadui");
                    //HTGestureManager.Instance.OverrideFocusedObject = null;
                    //GameObject.Find("ControlsUI").gameObject.SetActive(true);
                }
                break;
        }
    }

    private void LoadUI()
    {
        try
        {
            Debug.Log("loading screen");
            loadingScreen.SetActive(false);
            Debug.Log("cursor");
            cursor.SetActive(true);
        } catch(NullReferenceException e)
        {
            Debug.Log(e.Message);
        }
    }
}