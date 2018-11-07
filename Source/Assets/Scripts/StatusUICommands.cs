// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUICommands : MonoBehaviour {

    private const string SOLO_MODE = "solo";

    private string SessionNumber;
    private string mode;
    private int connectedUsers = 0;

    private Text SessionIDTextBox;
    private Text UserCountTextBox;
    private Text InSessionTextBox;
    
    private GameObject sharing;

	// Use this for initialization
	void Start () {
        mode = PlayerPrefs.GetString("mode");

        SessionIDTextBox = transform.Find("SessionID").GetComponent<Text>();
        UserCountTextBox = transform.Find("UserCountText").GetComponent<Text>();
        InSessionTextBox = transform.Find("InSessionText").GetComponent<Text>();
        SessionNumber = PlayerPrefs.GetString("session");

        Debug.Log("in status ui start: session number is" + SessionNumber);
        sharing = GameObject.Find("Sharing");

        if (mode == SOLO_MODE)
        {
            SessionIDTextBox.text = "";
            UserCountTextBox.text = "";
            InSessionTextBox.text = "Solo session";
            transform.Find("session-text").GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            transform.Find("session-text").GetComponent<SpriteRenderer>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (mode != SOLO_MODE)
        {
            UpdateSessionUsersUI();
        }
    }

    private void UpdateSessionUsersUI()
    {
        try
        {
            SessionIDTextBox.text = SessionNumber;
            connectedUsers = SharingStage.Instance.SessionUsersTracker.CurrentUsers.Count;
            UserCountTextBox.text = connectedUsers.ToString();

            if (connectedUsers == 1)
            {
                InSessionTextBox.text = "Person in session.";
            }
            else
            {
                InSessionTextBox.text = "People in session.";
            }
        }
        catch(NullReferenceException e)
        {
            Debug.Log("Tried getting user count without being in an established session.");
        }
    }
}
