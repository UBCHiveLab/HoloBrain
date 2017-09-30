// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolateButtonAction : MonoBehaviour {

    private bool buttonSelected;
    private Dictionary<string, string> isolateIconsToGameObjectName;
    private const string BRAIN_PARTS_NAME = "BrainParts";
    private const string ISOLATE_MENU_NAME = "IsolateMode";
    private GameObject IsolateMenu;
    private GameObject brainStructures;
    private string PartToIsolate;
    // Use this for initialization
    void Start () {
        buttonSelected = false;
        brainStructures = GameObject.Find(BRAIN_PARTS_NAME);
        IsolateMenu = GameObject.Find(ISOLATE_MENU_NAME);
        isolateIconsToGameObjectName = new Dictionary<string, string>
        {
            {"putamen-icon" , "right_putamen"},
            {"caudate-icon", "right_caudate"},
            { "globus-icon", "right_globus_pallidus" },
            { "nigra-icon",  "right_substantia_nigra" },
            { "subthalamic-icon" , "right_subthalamic"},
            { "thalamus-icon","thalamus"},
            {"add-icon","AddAll"},
            {"remove-icon","RemoveAll"},
        };
        Debug.Log("in the on start of: " + gameObject.name);
    }
	
    public void OnSelect()
    {
      
        PartToIsolate = isolateIconsToGameObjectName[gameObject.name];
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
       
    }
    private void AddBrainPart(string PartName)
    {
        brainStructures.GetComponent<IsolateStructures>().TryToIsolate(PartName);

    }

    private void RemoveBrainPart(string PartName)
    {
        brainStructures.GetComponent<IsolateStructures>().TryToReturnFromIsolate(PartName);

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
           for (int i=0; i < IsolateMenu.transform.childCount; i++)
        {
            if (IsolateMenu.transform.GetChild(i).gameObject.GetComponent<ButtonEnabledFeedback>() != null)
            {
                IsolateMenu.transform.GetChild(i).gameObject.GetComponent<ButtonEnabledFeedback>().ToggleOpacity(select);
            }
        }
    }
}
