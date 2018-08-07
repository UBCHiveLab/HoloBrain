// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolateExitButtonAction : MonoBehaviour {

    private const string BRAIN_PARTS_NAME = "BrainParts";
    private GameObject isolateMode;
    GameObject brain;
 
    // Use this for initialization
    void Start()
    {
        brain = GameObject.Find(BRAIN_PARTS_NAME);
    }

    private void OnEnable()
    {
        isolateMode = GameObject.Find("IsolateMode");
    }

    void OnSelect()
    {
        brain.GetComponent<IsolateStructures>().ConcludeIsolationMode();
        if (isolateMode != null)
        {
            isolateMode.SetActive(false);
        }
    }
}
