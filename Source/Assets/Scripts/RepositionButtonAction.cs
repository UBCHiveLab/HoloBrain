// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionButtonAction : MonoBehaviour
{
    private const string BRAIN_1_NAME = "Brain";
    private const string BRAIN_2_NAME = "Brain2";

    private const string BRAIN_SELECTION_CONTROLLER = "selectBrainController";

    GameObject brain_1, brain_2;
    GameObject selectBrainControlGameObject;

    private string __selectedBrain;
    private StateAccessor stateAccessor;

    private GameObject SelectedBrain
    {
        get
        {
            __selectedBrain = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            return (__selectedBrain == BRAIN_1_NAME) ? (brain_1) : (brain_2);
        }
    }


    void Awake()
    {
        brain_1 = GameObject.Find(BRAIN_1_NAME);
        brain_2 = GameObject.Find(BRAIN_2_NAME);

        selectBrainControlGameObject = GameObject.FindWithTag(BRAIN_SELECTION_CONTROLLER);
    }

    // Use this for initialization
    private void Start() {
        stateAccessor = StateAccessor.Instance;
        
    }

    // Update is called once per frame
    void Update () {
        if (stateAccessor.IsInCompareMode())
        {
            float x = brain_1.transform.position.x + 0.75f;
            float y = brain_1.transform.position.y;
            float z = brain_1.transform.position.z;
            brain_2.transform.position = new Vector3(x, y, z);
        }
		
	}

    public void OnSelect()
    {
        if (!stateAccessor.IsInCompareMode())
            SelectedBrain.GetComponent<HologramPlacement>().ResetStage();
        else
        {
            brain_1.GetComponent<HologramPlacement>().ResetStage();
        }
    }


}
