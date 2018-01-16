// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButtonAction : MonoBehaviour {

    private const string BRAIN_1_GAME_OBJECT_NAME = "Brain";

    GameObject currentBrain, brain_1, brain_2;
    GameObject selectBrainControlGameObject;
    private string selectedBrainName;

    // Use this for initialization
    void Start () {
        brain_1 = GameObject.Find("brainParts");
        brain_2 = GameObject.Find("brainParts2");
        //Debug.Log("Rotate button brain variable is pointing to " + brain.name);

        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public void OnSelect()
    {
        //do the action
        //TODO: faster technique would be to change all the bindings in the BrainSelectControl class right when the selected brain is changed

        selectedBrainName = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
        currentBrain = (selectedBrainName == BRAIN_1_GAME_OBJECT_NAME) ? (brain_1) : (brain_2);

        Debug.Log("Rotate button brain variable is pointing to " + currentBrain.name);

        currentBrain.GetComponent<RotateStructures>().OnSelect();
    }

}
