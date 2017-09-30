// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePinsManager : MonoBehaviour {

    private const string BRAIN_PARTS_GAMEOBJECT_NAME = "BrainParts";
    private Transform brain;

    private bool zoomChangingPreviousState;

    private ScaleToggler scaleScript;
    private ExplodingCommands explodeScript;

    private ArrayList activePins;
    private ArrayList activeLabels;

    // Use this for initialization
    void Start () {
        brain = GameObject.Find(BRAIN_PARTS_GAMEOBJECT_NAME).transform;

        scaleScript = brain.GetComponent<ScaleToggler>();
        explodeScript = brain.GetComponent<ExplodingCommands>();
        zoomChangingPreviousState = scaleScript.zoomChanging;

        activeLabels = new ArrayList();
        activePins = new ArrayList();
    }
	
	// Update is called once per frame
	void Update () {
        //rotate pins
        transform.rotation = brain.rotation;

        //rotate labels
        foreach (Transform label in activeLabels)
        {
            label.rotation = Quaternion.LookRotation(label.position - Camera.main.transform.position, Vector3.up);
        }

        //reposition pins (if needed)
        if (scaleScript.zoomChanging || zoomChangingPreviousState || explodeScript.currentState != ExplodingCommands.ExplodingState.Resting)
        {
            foreach (PinPositionManager pin in activePins)
            {
                pin.AdjustPinPosition();
                zoomChangingPreviousState = scaleScript.zoomChanging;
            }
        }
    }
    public void UpdateActivePin(bool active, GameObject pin)
    {
        PinPositionManager pinPosManager = pin.GetComponent<PinPositionManager>();
        Transform label = pin.transform.Find("Canvas");

        if (active)
        {
            activePins.Add(pinPosManager);
            activeLabels.Add(label);
        }
        else
        {
            activePins.Remove(pinPosManager);
            activeLabels.Remove(label);
        }
    }

}
