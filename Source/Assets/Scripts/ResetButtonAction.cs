// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetButtonAction : CommandToExecute {
    private const string BRAIN_PARTS_NAME = "Brain";
    private const string STRUCTURES_MENU_BUTTONS = "Buttons";
    private const string ControlS_UI = "ControlsUI";
    GameObject ButtonsMenu;
    GameObject ControlsUI;

    override protected Action Command()
    {
        return delegate
        {
            GameObject.Find(BRAIN_PARTS_NAME).GetComponent<ResetState>().ResetEverything();
            //ButtonsMenu = GameObject.Find(STRUCTURES_MENU_BUTTONS);
            //ControlsUI = GameObject.Find(ControlS_UI);
            //reset the state of the menus and buttons
            //ResetUI();
        };
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
        //ResetMenu();
        ResetUIButtons();
    }
}
