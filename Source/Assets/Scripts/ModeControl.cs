// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
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
    private const string PRELOAD_OBJ_NAME = "PreLoad";
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
        //This code used to do sharing.GetComponent<AutoJoinSession>().SessionName = PlayerPrefs.GetString(Session), but 
        //AutoJoinSessionAndRoom doesnt auto join session on change of SessionName var anymore
     
        foreach(Session session in SharingStage.Instance.SessionsTracker.Sessions)
        {
            if(session.GetName().GetString() == PlayerPrefs.GetString(SESSION))
            {
                SharingStage.Instance.SessionsTracker.JoinSession(session);
            }
        }
        GameObject.Find(STATUSUI_GAMEOBJECT_NAME).GetComponent<StudentModeCommands>().ToggleControlsUI(false);
    }

    private void HandleProfessorMode()
    {
        GameObject sharing = GameObject.Find(SHARING_OBJ_NAME);
        SharingStage.Instance.SessionsTracker.CreateSession(SharingStage.Instance.SessionName); //before session name was getRandomID().ToString(), not focusing on that now so using default
    }

    private void HandleSoloMode()
    {
    //    GameObject sharing = GameObject.Find(SHARING_OBJ_NAME);
        GameObject loadingScreen = GameObject.Find(LOADING_SCREEN_OBJ_NAME);
        loadingScreen.SetActive(false);
      //  sharing.SetActive(false);
        //GameObject preload = GameObject.Find(PRELOAD_OBJ_NAME);
       // preload.GetComponent<ImportExportAnchorManager>().enabled = false;
        //preload.GetComponent<AppStateManager>().enabled = false;
       // preload.GetComponent<RemotePlayerManager>().enabled = false;
       // preload.GetComponent<LocalPlayerManager>().enabled = false;
    }
}
