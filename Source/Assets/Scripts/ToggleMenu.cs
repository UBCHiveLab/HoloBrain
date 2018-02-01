// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour {

    private const string BRAIN_1 = "Brain";
    private GameObject brainParts, brainParts2;

    private const string BRAIN_SELECTION_CONTROLLER = "selectBrainController";

    GameObject brain_structures_1, brain_structures_2;
    GameObject selectBrainControlGameObject;

    private string __selectedBrain;

    private GameObject SelectedBrainParts
    {
        get
        {
            __selectedBrain = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            return (__selectedBrain == BRAIN_1) ? (brainParts) : (brainParts2);
        }
    }


    // Use this for initialization
    void Start () {
        brainParts = GameObject.Find("BrainParts");
        brainParts2 = GameObject.Find("BrainParts2");
    }
    public void ChangeMenu()
    {
        if (gameObject.name == "exit-mode-icon" && (SelectedBrainParts.GetComponent<StateAccessor>().GetCurrentMode() != StateAccessor.Mode.Isolated || SelectedBrainParts.GetComponent<IsolateStructures>().AtLeastOneStructureIsMovingOrResizing) ) 
        {
            return;
        }
        if (gameObject.name == "isolate-mode-icon" && SelectedBrainParts.GetComponent<StateAccessor>().GetCurrentMode() != StateAccessor.Mode.Isolated && SelectedBrainParts.GetComponent<IsolateStructures>().AtLeastOneStructureIsMovingOrResizing)
        {
            return;
        }

        //Send the name of the button pressed to the paretnt to know which UI menus should be enabled
        GameObject.Find("ControlsUI").GetComponent<SubMenusManager>().ToggleMenuUI(gameObject.name);
    }
}
