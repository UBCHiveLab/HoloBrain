// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour {

    private GameObject brain;

	// Use this for initialization
	void Start () {
        brain = GameObject.Find("Brain");
    }
    public void ChangeMenu()
    {
        if (gameObject.name == "exit-mode-icon" && (brain.GetComponent<StateAccessor>().GetCurrentMode() != StateAccessor.Mode.Isolated || brain.GetComponent<IsolateStructures>().AtLeastOneStructureIsMovingOrResizing) ) 
        {
            return;
        }
        if (gameObject.name == "isolate-mode-icon" && brain.GetComponent<StateAccessor>().GetCurrentMode() != StateAccessor.Mode.Isolated && brain.GetComponent<IsolateStructures>().AtLeastOneStructureIsMovingOrResizing)
        {
            return;
        }

        //Send the name of the button pressed to the paretnt to know which UI menus should be enabled
        GameObject.Find("ControlsUI").GetComponent<SubMenusManager>().ToggleMenuUI(gameObject.name);
    }
}
