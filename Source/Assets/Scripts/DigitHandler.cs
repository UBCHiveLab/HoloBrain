// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class DigitHandler : MonoBehaviour
{
    private GameObject sharing;
    private string id;
    private GameObject inputField;
    private GameObject errorMessage;
    private bool errorMessageIsActive;

    private string INPUT_FIELD_OBJECT_NAME = "InputField";
    private string SHARING_FIELD_OBJECT_NAME = "Sharing";
    private const string ERROR_MSG_OBJECT_NAME = "ErrorMessage";

    // Use this for initialization
    void Start()
    {
        errorMessageIsActive = false;
        inputField = GameObject.Find(INPUT_FIELD_OBJECT_NAME);
        sharing = GameObject.Find(SHARING_FIELD_OBJECT_NAME);
        errorMessage = GameObject.Find(ERROR_MSG_OBJECT_NAME);
        errorMessage.GetComponent<Image>().enabled = false;
    }

    public void OnTap(string buttonName)
    {
        if (errorMessageIsActive)
        {
            errorMessage.GetComponent<Image>().enabled = false;
        }
        Debug.Log(buttonName);
        buttonName = buttonName.Replace("\n", System.String.Empty);

        if (buttonName == "Join Session")
        {
            JoinSession();
        }else if (buttonName == "Backspace")
        {
            id = id.Substring(0, id.Length - 1);
            inputField.GetComponent<InputField>().text = id;
        }
        else
        {
            id += buttonName;
            inputField.GetComponent<InputField>().text = id;
        }
    }

    private void JoinSession()
    {
        Debug.Log("Inside JoinSession()");
        PlayerPrefs.SetString("session", id);
        Debug.Log("in digit handler, the ID entered is" + id);
        PlayerPrefs.SetString("mode", "student");
        if (!SessionExists(id))
        {
            // display error UI
            errorMessage.GetComponent<Image>().enabled = true;
            errorMessageIsActive = true;
            id = "";
            inputField.GetComponent<InputField>().text = "";
            Debug.Log("SESSION DOES NOT EXIST");
        }
        else
        {
            SceneManager.LoadScene("EducationalRoom");
        }
    }

    //TODO: Not sure if the session.GetName() method here is the way we want to do this comparison
    private bool SessionExists(string id)
    {
        Debug.Log("Session ID is " + id);
        bool result = false;
        List<Session> sessions = SharingStage.Instance.SessionsTracker.Sessions;
        foreach(Session session in sessions)
        {
            if(session.GetName().GetString() == id)
            {
                result = true;
            }
        }
        return false;
    }
}

