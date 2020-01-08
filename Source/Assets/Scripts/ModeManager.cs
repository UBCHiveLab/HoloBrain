// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing.Tests;
using UnityEngine;

public class ModeManager : MonoBehaviour {

    GameObject sharing;
    GameObject hologramCollection;

	// Use this for initialization
	void Start () {
        sharing = GameObject.Find("Sharing");
        hologramCollection = GameObject.Find("HologramCollection");

        if (PlayerPrefs.GetString("mode") == "solo")
        {
            Debug.Log("solo");
            sharing.SetActive(false);
            hologramCollection.GetComponent<ImportExportAnchorManager>().enabled = false;
            hologramCollection.GetComponent<AppStateManager>().enabled = false;
            hologramCollection.GetComponent<RemotePlayerManager>().enabled = false;
            hologramCollection.GetComponent<LocalPlayerManager>().enabled = false;
        } else if (PlayerPrefs.GetString("mode") == "student")
        {
            Debug.Log("in student mode");
        } else if (PlayerPrefs.GetString("mode") == "professor")
        {
            Debug.Log("prof mode");
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
