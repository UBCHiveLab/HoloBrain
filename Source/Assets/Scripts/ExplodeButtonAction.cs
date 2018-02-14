// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeButtonAction : MonoBehaviour
{
    private const string BRAIN_1 = "Brain";
    private const string BRAIN_PARTS_1 = "BrainParts";
    private const string BRAIN_PARTS_2 = "BrainParts2";
    private StateAccessor stateAccessor;
    private const string BRAIN_SELECTION_CONTROLLER = "selectBrainController";

    GameObject brain_structures_1, brain_structures_2;
    GameObject selectBrainControlGameObject;

    private string __selectedBrain;

    private GameObject SelectedBrainStructures
    {
        get
        {
            __selectedBrain = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            return (__selectedBrain == BRAIN_1) ? (brain_structures_1) : (brain_structures_2);
        }
    }


    void Awake()
    {
        brain_structures_1 = GameObject.Find(BRAIN_PARTS_1);
        brain_structures_2 = GameObject.Find(BRAIN_PARTS_2);

        selectBrainControlGameObject = GameObject.FindWithTag(BRAIN_SELECTION_CONTROLLER);
        stateAccessor = StateAccessor.Instance;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnSelect()
    {
        if (stateAccessor.IsInCompareMode())
        {
            brain_structures_1.GetComponent<ExplodingCommands>().OnSelect();
            brain_structures_2.GetComponent<ExplodingCommands>().OnSelect();
        } else
            SelectedBrainStructures.GetComponent<ExplodingCommands>().OnSelect();
    }
}
