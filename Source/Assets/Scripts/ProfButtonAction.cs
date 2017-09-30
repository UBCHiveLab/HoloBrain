// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfButtonAction : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(" in Prof button start");

    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnSelect()
    {
        string buttonText = gameObject.GetComponentInChildren<Text>().text;
        Debug.Log(" in Prof button");
        Debug.Log(buttonText);
        PlayerPrefs.SetString("mode", "prof");
        SceneManager.LoadScene("StudentOrSolo");


    }
}
