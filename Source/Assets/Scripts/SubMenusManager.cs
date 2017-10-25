// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenusManager : MonoBehaviour {

    private Dictionary<string, List<string>> ButtonNamesToCorrespondingMenus;
    private const string SUBSECTION_UI_NAME = "SubsectionUI";
    private const string BUTTONS_MENU_NAME = "Buttons";
    private const string BASAL_GANGLIA_MODES_NAME = "BasalGangliaModes";
    private const string ISOLATE_MODE_NAME = "IsolateMode";
    private const string MRI_MODE_NAME = "MRIMode";

    private GameObject SubsectionMenu;
    private GameObject ButtonsMenu;
    private GameObject BasalGangliaMenu;
    private GameObject IsolateModeMenu;
    private GameObject MRIMenu;
    private GameObject MenuButton;
    private List<string> CurrentActiveMenu;
    private List<string> MenusToActivate;

    // Use this for initialization
    void Start () {

        //map button names to the corresponsing menus that should be enabled when the button is pressed
        ButtonNamesToCorrespondingMenus = new Dictionary<string, List<string>>
        {
            { "basal-icon", new List<string> { "BasalGangliaModes" } },
            { "isolate-mode-icon", new List<string> { "IsolateMode" } },
            { "subsections-button", new List<string> { "SubsectionUI" } },
            { "structures-icon", new List<string> { "BasalGangliaModes", "Buttons" } },
            { "mri-icon", new List<string> { "BasalGangliaModes", "MRIMode" } },
            { "exit-mode-icon", new List<string> { "BasalGangliaModes", "Buttons" } },

        };
                
        SubsectionMenu = GameObject.Find(SUBSECTION_UI_NAME);
        ButtonsMenu = GameObject.Find(BUTTONS_MENU_NAME);
        BasalGangliaMenu = GameObject.Find(BASAL_GANGLIA_MODES_NAME);
        IsolateModeMenu = GameObject.Find(ISOLATE_MODE_NAME);
        MRIMenu= GameObject.Find(MRI_MODE_NAME);

        EnableDefaultMenus();

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ToggleMenuUI(string currentMenu)
    {
       //deactivate the currently active UI menus
        foreach (var item in CurrentActiveMenu)
        {
            //reset the button state
            ResetChildButtonState(GameObject.Find("ControlsUI").transform.Find(item).gameObject);
            //de activate the menu
            GameObject.Find("ControlsUI").transform.Find(item).gameObject.SetActive(false);
        }
        MenusToActivate = ButtonNamesToCorrespondingMenus[currentMenu];
        //activate the menus corresponding to the button tapped
        foreach (var item in MenusToActivate)
        {
            GameObject.Find("ControlsUI").transform.Find(item).gameObject.SetActive(true);
        }
        CurrentActiveMenu = MenusToActivate;
       
    }

    public void EnableDefaultMenus()
    {
        ButtonsMenu.SetActive(true);
        BasalGangliaMenu.SetActive(true);
        //SubsectionMenu.SetActive(false);
        IsolateModeMenu.SetActive(false);
        MRIMenu.SetActive(false);
        CurrentActiveMenu = new List<string> { "BasalGangliaModes", "Buttons" };
    }

    private void ResetChildButtonState(GameObject MenuToDeactivate)
    {
        //Return all the menu buttons to the original state
        for(int i=0; i < MenuToDeactivate.transform.childCount; i++)
        {
            MenuButton = MenuToDeactivate.transform.GetChild(i).gameObject;
            if (MenuButton.GetComponent<ButtonCommands>())
            {
                MenuButton.GetComponent<ButtonCommands>().ReturnButtonToOriginalState();
            }
        }
    }

}
