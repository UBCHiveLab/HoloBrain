// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;
using HoloToolkit.Unity.SpatialMapping;
using HutongGames.PlayMaker;

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
        WaitingForSecondBrainPlacement,
        Ready
    }

    private const string BRAIN_1_NAME = "Brain_1";
    private const string BRAIN_2_NAME = "Brain_2";
    private const string LOADING_SCREEN_GAMEOBJECT_NAME = "LoadingScreen";
    private const string CURSOR_GAMEOBJECT_NAME = "Cursor";
    private const string STATUSUI_GAMEOBJECT_NAME = "StatusUI";
    private const string CORTEX_GAMEOBJECT_NAME = "cortex_low";
    private const string LABELDISPLAY_GAMEOBJECT_NAME = "LabelDisplay";

    private GameObject loadingScreen;
    private GameObject cursor;
    private GameObject brain_1;
    private GameObject brain_2;
    private GameObject cortex_1;
    private GameObject cortex_2;
    private GameObject labelDisplay;

    public PlayMakerFSM fsm;

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
        brain_1 = GameObject.FindWithTag(BRAIN_1_NAME);
        brain_2 = GameObject.FindWithTag(BRAIN_2_NAME);
        cortex_1 = brain_1.transform.Find(CORTEX_GAMEOBJECT_NAME).gameObject;
        cortex_2 = brain_2.transform.Find(CORTEX_GAMEOBJECT_NAME).gameObject;
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
                    HTGestureManager.Instance.OverrideFocusedObject = brain_1;
                    SpatialMappingManager.Instance.gameObject.SetActive(true);
                    SpatialMappingManager.Instance.DrawVisualMeshes = true;
                    SpatialMappingManager.Instance.StartObserver();
                }
                else
                {
                    labelDisplay.SetActive(false);
                    cortex_1.SetActive(false);
                    cortex_2.SetActive(false);
                }
                break;
            case AppState.WaitingForStageTransform:
                // Now if we have the stage transform we are ready to go.
                Debug.Log("0");
                string fsmStateName = fsm.ActiveStateName;
                if (fsmStateName == "WaitingToPlaceBrain_1" && brain_1.GetComponent<HologramPlacement>().GotTransform) {
                    Debug.Log("1");
                    brain_2.SetActive(true);
                    Debug.Log("2");
                    HTGestureManager.Instance.OverrideFocusedObject = brain_2;
                    Debug.Log("3");
                    cortex_2.SetActive(true);
                    Debug.Log("4");
                    fsm.SendEvent("PlacedBrain_1");
                } 
                else if (fsmStateName == "WaitingToPlaceBrain_2" && brain_2.GetComponent<HologramPlacement>().GotTransform) {
                    fsm.SendEvent("PlacedBrain_1");
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
        cortex_1.SetActive(true);
    }
}