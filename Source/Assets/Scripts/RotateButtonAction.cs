// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButtonAction : MonoBehaviour {

    private const string BRAIN_PARTS_1_NAME = "BrainParts";
    private const string BRAIN_PARTS_2_NAME = "BrainParts2";
    private const string BRAIN_1_GAME_OBJECT_NAME = "Brain";

    GameObject currentBrain, brain_1, brain_2;
    GameObject selectBrainControlGameObject;
    private string selectedBrainName;
    private StateAccessor stateAccessor;

    // Use this for initialization
    public void Awake () {
        brain_1 = GameObject.Find(BRAIN_PARTS_1_NAME);
        brain_2 = GameObject.Find(BRAIN_PARTS_2_NAME);
        //Debug.Log("Rotate button brain variable is pointing to " + brain.name);

        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
        stateAccessor = StateAccessor.Instance;
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public void OnSelect()
    {
        //do the action
        //TODO: faster technique would be to change all the bindings in the BrainSelectControl class right when the selected brain is changed
        if (stateAccessor.IsInCompareMode())
        {
            brain_1.GetComponent<RotateStructures>().OnSelect();
            brain_2.GetComponent<RotateStructures>().OnSelect();
        }
        else
        {
            selectedBrainName = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            currentBrain = (selectedBrainName == BRAIN_1_GAME_OBJECT_NAME) ? (brain_1) : (brain_2);

            Debug.Log("Rotate button brain variable is pointing to " + currentBrain.name);

            currentBrain.GetComponent<RotateStructures>().OnSelect();
        }
    }

}
