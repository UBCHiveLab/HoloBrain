// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDownButtonAction : MonoBehaviour
{

    private const string BRAIN_PARTS_1_NAME = "BrainParts";
    private const string BRAIN_PARTS_2_NAME = "BrainParts2";

    private const string BRAIN_1_Name = "Brain";
    private StateAccessor stateAccessor;

    GameObject currentBrain, brain_1, brain_2;
    GameObject selectBrainControlGameObject;
    private string selectedBrainName;


    // Use this for initialization
    void Start () {
        // Corrected: finding on start would be better than finding on every select
        // TODO: try to do this for getComponent as well
        brain_1 = GameObject.Find(BRAIN_PARTS_1_NAME);
        brain_2 = GameObject.Find(BRAIN_PARTS_2_NAME);

        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
        stateAccessor = StateAccessor.Instance;
    }
	
    public void OnSelect()
    {
        //TODO: faster technique would be to change all the bindings in the BrainSelectControl class right when the selected brain is changed

        if (stateAccessor.IsInCompareMode())
        {
            brain_1.GetComponent<ScaleToggler>().ScaleDown();
            brain_2.GetComponent<ScaleToggler>().ScaleDown();
        }
        else
        {
            selectedBrainName = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            currentBrain = (selectedBrainName == BRAIN_1_Name) ? (brain_1) : (brain_2);

            Debug.Log("Scale Down button brain variable is pointing to " + currentBrain.name);

            currentBrain.GetComponent<ScaleToggler>().ScaleDown();
        }
    }
    void OnStartGaze()
    {
       transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("ScaleButtonsManager").GetComponent<SubButtonManager>().SetGazeOn(true);

    }
    void OnEndGaze()
    {
        transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("ScaleButtonsManager").GetComponent<SubButtonManager>().SetGazeOn(false);
    }

}
