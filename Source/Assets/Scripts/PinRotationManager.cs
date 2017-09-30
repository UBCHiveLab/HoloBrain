// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinRotationManager : MonoBehaviour {

    private Transform brain;

    private ArrayList activeLabels;

    // Use this for initialization
    void Start () {
        brain = GameObject.Find("BrainParts").transform;
        activeLabels = new ArrayList();
    }
	
	// Update is called once per frame
	void Update () {
        //for pins
        transform.rotation = brain.rotation;

        //for labels
        foreach (Transform label in activeLabels)
        {
            //make the label face the camera
            label.rotation = Quaternion.LookRotation(label.position - Camera.main.transform.position, Vector3.up);
        }
    }

    public void UpdateActiveLabel(bool active, Transform label)
    {
        if (active)
            activeLabels.Add(label);
        else
            activeLabels.Remove(label);
    }

}
