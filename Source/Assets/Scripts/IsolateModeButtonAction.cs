// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolateModeButtonAction : MonoBehaviour {

    private AudioSource audio;
    private GameObject isolateMode;

    private const string BRAIN_PARTS_NAME = "BrainParts";
    private GameObject brain;
    // Use this for initialization
    void Start () {
        audio = gameObject.GetComponent<AudioSource>();
        isolateMode = GameObject.Find("IsolateMode");
        if(isolateMode != null)
        {
            isolateMode.SetActive(false);
        }
        brain = GameObject.Find(BRAIN_PARTS_NAME);
    }
	
	void OnSelect()
    {
        isolateMode.SetActive(true);
        brain.GetComponent<IsolateStructures>().InitiateIsolationMode();
    }
}
