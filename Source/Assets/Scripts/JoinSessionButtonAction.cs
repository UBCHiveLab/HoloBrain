// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JoinSessionButtonAction : MonoBehaviour {

    private string SessionNumber;
    private GameObject InputField;
	// Use this for initialization
	void Start () {
        InputField = GameObject.Find("InputField");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnSelect()
    {
        SessionNumber = InputField.GetComponentInChildren<Text>().text;
        if (SessionNumber != null)
        {
            PlayerPrefs.SetString("SessionToJoin", SessionNumber);

        }
        else
        {
            Debug.Log("Session number not entred");
        }
    }
}
