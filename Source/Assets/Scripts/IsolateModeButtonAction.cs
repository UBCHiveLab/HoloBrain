// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolateModeButtonAction : MonoBehaviour {

    private const string BRAIN_PARTS_1 = "BrainParts";
    private const string BRAIN_PARTS_2 = "BrainParts2";
    GameObject brain_1, brain_2;
    // Use this for initialization
    void Start () {
        brain_1 = GameObject.Find(BRAIN_PARTS_1);
        brain_2 = GameObject.Find(BRAIN_PARTS_2);
    }
	
	void OnSelect()
    {
        brain_1.GetComponent<IsolateStructures>().InitiateIsolationMode();
        if (brain_2)
            brain_2.GetComponent<IsolateStructures>().InitiateIsolationMode();
    }
}
