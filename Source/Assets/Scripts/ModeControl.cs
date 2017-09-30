// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeControl : MonoBehaviour {

    private const string PROFESSOR_MODE = "professor";
    private const string STUDENT_MODE = "student";
    private const string SOLO_MODE = "solo";
    private const string MODE = "mode";
    private const string SHARING_OBJ_NAME = "Sharing";
    private const string LOADING_SCREEN_OBJ_NAME = "LoadingScreen";
    private const string HOLOGRAM_COLLECTION_OBJ_NAME = "HologramCollection";
    private const string SESSION = "session";
    private const string CONTROLS_UI = "ControlsUI";
    private const string STATUSUI_GAMEOBJECT_NAME = "StatusUI";


    private void Awake()
    {
        if (PlayerPrefs.GetString(MODE) == SOLO_MODE)
        {
            HandleSoloMode();
        }
        else if (PlayerPrefs.GetString(MODE) == PROFESSOR_MODE)
        {
            HandleProfessorMode();  
        }
        else if (PlayerPrefs.GetString(MODE) == STUDENT_MODE)
        {
            HandleStudentMode();
        }

    }

    private void HandleStudentMode()
    {
        GameObject sharing = GameObject.Find(SHARING_OBJ_NAME);
        sharing.GetComponent<AutoJoinSession>().SessionName = PlayerPrefs.GetString(SESSION);
        GameObject.Find(STATUSUI_GAMEOBJECT_NAME).GetComponent<StudentModeCommands>().ToggleControlsUI(false);
    }

    private void HandleProfessorMode()
    {
        GameObject sharing = GameObject.Find(SHARING_OBJ_NAME);
        // sharing.GetComponent<AutoJoinSession>().SessionName = getRandomID().ToString();
    }

    private void HandleSoloMode()
    {
        GameObject sharing = GameObject.Find(SHARING_OBJ_NAME);
        GameObject loadingScreen = GameObject.Find(LOADING_SCREEN_OBJ_NAME);
        loadingScreen.SetActive(false);
        sharing.SetActive(false);
        GameObject hologramCollection = GameObject.Find(HOLOGRAM_COLLECTION_OBJ_NAME);
        hologramCollection.GetComponent<ImportExportAnchorManager>().enabled = false;
        hologramCollection.GetComponent<AppStateManager>().enabled = false;
        hologramCollection.GetComponent<RemotePlayerManager>().enabled = false;
        hologramCollection.GetComponent<LocalPlayerManager>().enabled = false;
    }
}
