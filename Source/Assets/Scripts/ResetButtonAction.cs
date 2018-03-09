// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonAction : MonoBehaviour {
	private const string BRAIN_1_NAME = "BrainParts";
	private const string BRAIN_2_NAME = "BrainParts2";

	private const string BRAIN_SELECTION_CONTROLLER = "selectBrainController";

	GameObject brain_1, brain_2;
    StateAccessor stateAccessor;
	//GameObject selectBrainControlGameObject;

	/*private string __selectedBrain;

	private GameObject SelectedBrain {
		get {
			__selectedBrain = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
			return (__selectedBrain == BRAIN_1_NAME) ? (brain_1) : (brain_2);
		}
	}*/


	void Awake() {
		brain_1 = GameObject.Find(BRAIN_1_NAME);
		brain_2 = GameObject.Find(BRAIN_2_NAME);
        stateAccessor = StateAccessor.Instance;

		//selectBrainControlGameObject = GameObject.FindWithTag(BRAIN_SELECTION_CONTROLLER);
	}

    private const string STRUCTURES_MENU_BUTTONS = "Buttons";
    private const string ControlS_UI = "ControlsUI";
	GameObject ButtonsMenu;
	GameObject ControlsUI;

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {
	}

	public void OnSelect() 
	{
		brain_1.GetComponent<ResetState>().ResetEverything();
        brain_2.GetComponent<ResetState>().ResetEverything();
        ButtonsMenu = GameObject.Find(STRUCTURES_MENU_BUTTONS);
        ControlsUI = GameObject.Find(ControlS_UI);
        //reset the state of the menus and buttons
        ResetUI();
       
    } 

    void ResetUIButtons()
    {
        for (int i=0; i<ButtonsMenu.transform.childCount; i++)
        {
            Debug.Log("in reset ui buttons" + ButtonsMenu.transform.GetChild(i).gameObject.name);
            if (ButtonsMenu.transform.GetChild(i).gameObject.GetComponent<ButtonSwapFeedback>() != null)
            {
                Debug.Log("in reset ui buttons thw swap feed back is not null " + ButtonsMenu.transform.GetChild(i).gameObject.name);
                ButtonsMenu.transform.GetChild(i).gameObject.GetComponent<ButtonSwapFeedback>().ResetButtonState();
            }
        }
    }
    void ResetMenu()
    {
        ControlsUI.GetComponent<SubMenusManager>().EnableDefaultMenus();
    }

    public void ResetUI()
    {
        ResetMenu();
        ResetUIButtons();
    }
}
