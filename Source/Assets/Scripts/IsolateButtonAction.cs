// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolateButtonAction : MonoBehaviour
{

    private bool buttonSelected, buttonSelected2;
    private Dictionary<string, string> isolateIconsToGameObjectName;

    private const string BRAIN_1 = "Brain";
    private const string BRAIN_PARTS_1 = "BrainParts";
    private const string BRAIN_PARTS_2 = "BrainParts2";
    private const string BRAIN_SELECTION_CONTROLLER = "selectBrainController";
    private const string ISOLATE_MENU_NAME = "IsolateMode";

    private GameObject IsolateMenu;
    private GameObject brain_structures_1, brain_structures_2;
    private GameObject selectBrainControlGameObject;

    private string PartToIsolate;
    private string __selectedBrain;

    private GameObject SelectedBrainStructures
    {
        get
        {
            __selectedBrain = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            return (__selectedBrain == BRAIN_1) ? (brain_structures_1) : (brain_structures_2);
        }
    }

    // Use this for initialization
    void Start()
    {
        buttonSelected = false;
        buttonSelected2 = false;
        brain_structures_1 = GameObject.Find(BRAIN_PARTS_1);
        brain_structures_2 = GameObject.Find(BRAIN_PARTS_2);
        selectBrainControlGameObject = GameObject.FindWithTag(BRAIN_SELECTION_CONTROLLER);
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

        else if (checkIfBrainPartAdded())
        {

            AddBrainPart(PartToIsolate);
           // buttonSelected = true;

        }
        else
        {
            RemoveBrainPart(PartToIsolate);
            buttonSelected = false;
        }

    }

    private bool checkIfBrainPartAdded()
    {
        if (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain == BRAIN_1)
        {
            if (!buttonSelected)
            {
                buttonSelected = true;
                return true;
            } else
            {
                buttonSelected = false;
                return false;
            }
        } else
        {
            if (!buttonSelected2)
            {
                buttonSelected2 = true;
                return true;
            }
            else
            {
                buttonSelected2 = false;
                return false;
            }
        }
    }

    private void AddBrainPart(string PartName)
    {
        Assert.IsTrue(PartName != null);
        Debug.Log(PartName);
        SelectedBrainStructures.GetComponent<IsolateStructures>().TryToIsolate(PartName);

    }

    private void RemoveBrainPart(string PartName)
    {
        SelectedBrainStructures.GetComponent<IsolateStructures>().TryToReturnFromIsolate(PartName);

    }

    public void AddAllParts()
    {
        Debug.Log("In handle add all button");

        SelectedBrainStructures.GetComponent<IsolateStructures>().AddAllParts();
        SelectAllButtons(true);
    }

    public void RemoveAllParts()
    {
        Debug.Log("In handle remove all button");
        SelectedBrainStructures.GetComponent<IsolateStructures>().RemoveAllParts();
        SelectAllButtons(false);
    }

    public void SelectAllButtons(bool select)
    {
        for (int i = 0; i < IsolateMenu.transform.childCount; i++)
        {
            if (IsolateMenu.transform.GetChild(i).gameObject.GetComponent<ButtonEnabledFeedback>() != null)
            {
                IsolateMenu.transform.GetChild(i).gameObject.GetComponent<ButtonEnabledFeedback>().ToggleOpacity(select);
            }
        }
    }

    public bool getButtonStatus()
    {
        if (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain == BRAIN_1)
        {
            return buttonSelected;
        } else
        {
            return buttonSelected2;
        }
    }
}