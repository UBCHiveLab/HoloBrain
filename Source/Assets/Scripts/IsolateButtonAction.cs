// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(ButtonAppearance))]
public class IsolateButtonAction : CommandToExecute {

    private bool buttonSelected;
    private Dictionary<string, string> isolateIconsToGameObjectName;
    private const string BRAIN_PARTS_NAME = "Brain";
    private const string ISOLATE_MENU_NAME = "isolateMode";
    List<GameObject> isolateButtons;
    private GameObject IsolateMenu;
    private GameObject brainStructures;
    private string PartToIsolate;
    // Use this for initialization
    override public void Start () {
        buttonSelected = false;
        brainStructures = GameObject.Find(BRAIN_PARTS_NAME);
        //IsolateMenu = GameObject.Find(ISOLATE_MENU_NAME);
        //isolateButtons = GetStructureButtons();
        base.Start();
    }

    List<GameObject> GetStructureButtons()
    {
        GameObject menu = GameObject.Find("isolateMode");
        List<GameObject> result = new List<GameObject>();
        Component[] basal = menu.transform.Find("ExtendedMenu/Basal/BasalStructures").GetComponentsInChildren<IsolateButtonAction>(true);
        Component[] limbic = menu.transform.Find("ExtendedMenu/Limbic/LimbicStructures").GetComponentsInChildren<IsolateButtonAction>(true);
        Component[] vessels = menu.transform.Find("ExtendedMenu/Vessel/VesselStructures").GetComponentsInChildren<IsolateButtonAction>(true);
        Debug.Log("declared arrays");
        foreach(Component cur in basal)
        {
            result.Add(cur.gameObject);
        }
        foreach (Component cur in limbic)
        {
            result.Add(cur.gameObject);
        }
        foreach (Component cur in vessels)
        {
            result.Add(cur.gameObject);
        }
        result.Add(menu.transform.Find("Cerebellum").gameObject);
        //result.Add(menu.transform.Find("DTI").gameObject);
        Debug.Log("returning result");
        return result;
    }
	
    override protected Action Command()
    {
        return delegate
        {
            PartToIsolate = gameObject.name;
            if (PartToIsolate == "AddAll")
            {
                AddAllParts();
            }

            else if (PartToIsolate == "RemoveAll")
            {
                RemoveAllParts();
            }

            else if (!buttonSelected)
            {
                AddBrainPart(PartToIsolate);
                buttonSelected = true;
            }
            else
            {
                RemoveBrainPart(PartToIsolate);
                buttonSelected = false;
            }
        };
    }

    private void AddBrainPart(string PartName)
    {
        brainStructures.GetComponent<IsolateStructures>().TryToIsolate(PartName);
    }

    private void RemoveBrainPart(string PartName)
    {
        brainStructures.GetComponent<IsolateStructures>().TryToReturnFromIsolate(PartName);
    }

    public void SetButtonSelected(bool selected)
    {
        if(selected)
        {
            GetComponent<ButtonAppearance>().SetButtonActive();
        }
        else
        {
            GetComponent<ButtonAppearance>().ResetButton();
        }
        buttonSelected = selected;
    }

    public void AddAllParts()
    {
        Debug.Log("In handle add all button");
        brainStructures.GetComponent<IsolateStructures>().AddAllParts();
        SelectAllButtons(true);
    }

    public void RemoveAllParts()
    {
        Debug.Log("In handle remove all button");
        brainStructures.GetComponent<IsolateStructures>().RemoveAllParts();
        SelectAllButtons(false);
    }

    public void SelectAllButtons(bool select)
    {
           for (int i=0; i < isolateButtons.Count; i++)
        {
            if (isolateButtons[i].GetComponent<ButtonAppearance>() != null)
            {
                isolateButtons[i].GetComponent<IsolateButtonAction>().SetButtonSelected(select);
            }
        }
    }
}
