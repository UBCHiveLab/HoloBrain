// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolateButtonAction : MonoBehaviour {

    private bool buttonSelected;
    private Dictionary<string, string> isolateIconsToGameObjectName;
    private const string BRAIN_PARTS_NAME = "Brain";
    private const string ISOLATE_MENU_NAME = "isolateMode";
    List<GameObject> isolateButtons;
    private GameObject IsolateMenu;
    private GameObject brainStructures;
    private string PartToIsolate;
    // Use this for initialization
    void Start () {
        buttonSelected = false;
        brainStructures = GameObject.Find(BRAIN_PARTS_NAME);
        IsolateMenu = GameObject.Find(ISOLATE_MENU_NAME);
        Debug.Log("in the on start of: " + gameObject.name);
        isolateButtons = GetStructureButtons();
    }

    List<GameObject> GetStructureButtons()
    {
        GameObject menu = GameObject.Find("isolateMode");
        List<GameObject> result = new List<GameObject>();
        Transform[] basal = menu.transform.Find("Basal/BasalStructures").GetComponentsInChildren<Transform>(true);
        Transform[] limbic = menu.transform.Find("Limbic/LimbicStructures").GetComponentsInChildren<Transform>(true);
        Transform[] vessels = menu.transform.Find("Vessel/VesselStructures").GetComponentsInChildren<Transform>(true);
        Debug.Log("declared arrays");
        foreach(Transform cur in basal)
        {
            result.Add(cur.gameObject);
        }
        foreach (Transform cur in limbic)
        {
            result.Add(cur.gameObject);
        }
        foreach (Transform cur in vessels)
        {
            result.Add(cur.gameObject);
        }
        Debug.Log("pushed arrays to list");
        result.Add(menu.transform.Find("Cerebellum").gameObject);
        result.Add(menu.transform.Find("DTI").gameObject);
        Debug.Log("returning result");
        return result;
    }
	
    public void OnSelect()
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
            gameObject.GetComponent<ButtonAppearance>().SetButtonActive();
        }
        else
        {
            RemoveBrainPart(PartToIsolate);
            buttonSelected = false;
            gameObject.GetComponent<ButtonAppearance>().ResetButton();
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
           for (int i=0; i < isolateButtons.Count; i++)
        {
            if (isolateButtons[i].GetComponent<ButtonEnabledFeedback>() != null)
            {
                isolateButtons[i].GetComponent<ButtonEnabledFeedback>().ToggleOpacity(select);
            }
        }
    }
}
