// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
using HoloToolkit.Unity.SpatialMapping;

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
    private const string CORTEX_GAMEOBJECT_NAME = "Cortex";
    private const string LABELDISPLAY_GAMEOBJECT_NAME = "LabelDisplay";

    private GameObject loadingScreen;
    private GameObject cursor;
    private GameObject cortex;
    private GameObject labelDisplay;

    /// <summary>
    /// Tracks the current state in the experience.
    /// </summary>
    public AppState CurrentAppState { get; set; }

    void Start()
    {
        CurrentAppState = AppState.WaitingForAnchor;
        Debug.Log("AppStateManager: has just started and is WaitingForAnchor");

        loadingScreen = GameObject.Find(LOADING_SCREEN_GAMEOBJECT_NAME);
        labelDisplay = GameObject.Find(LABELDISPLAY_GAMEOBJECT_NAME);
        cortex = GameObject.Find(CORTEX_GAMEOBJECT_NAME);
        cursor = GameObject.Find(CURSOR_GAMEOBJECT_NAME);
        cursor.SetActive(false);
    }

    public void ResetStage()
    {
        // If we fall back to waiting for anchor, everything needed to 
        // get us into setting the target transform state will be setup.
        Debug.Log("AppStateManager: the stage is being reset");
        CurrentAppState = AppState.WaitingForAnchor;
    }

    void Update()
    {
        switch (CurrentAppState)
        {
            case AppState.WaitingForAnchor:
                // Once the anchor is established we need to run spatial mapping for a 
                // little while to build up some meshes.
                if (ImportExportAnchorManager.Instance.AnchorEstablished)
                {
                    CurrentAppState = AppState.WaitingForStageTransform;
                    LoadUI();
                    HTGestureManager.Instance.OverrideFocusedObject = HologramPlacement.Instance.gameObject;
                    SpatialMappingManager.Instance.gameObject.SetActive(true);
                    SpatialMappingManager.Instance.DrawVisualMeshes = false; // true;
                    SpatialMappingManager.Instance.StartObserver();
                }
                else
                {
                    labelDisplay.SetActive(false);
                    cortex.SetActive(false);
                }
                break;
            case AppState.WaitingForStageTransform:
                // Now if we have the stage transform we are ready to go.
                if (HologramPlacement.Instance.GotTransform)
                {
                    CurrentAppState = AppState.Ready;
                    HTGestureManager.Instance.OverrideFocusedObject = null;
                    transform.Find("ControlsUI").gameObject.SetActive(true);
                }
                break;
        }
    }

    private void LoadUI()
    {
        loadingScreen.SetActive(false);
        cursor.SetActive(true);
        labelDisplay.SetActive(true);
        cortex.SetActive(true);
    }
}