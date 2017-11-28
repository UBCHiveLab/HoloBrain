// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleButtonAction : MonoBehaviour
{
    //TODO: this line not being used
    private const string BRAIN_PARTS_NAME = "BrainParts";
    private const string SUB_MENU_MANAGER = "ScaleButtonsManager";

    // Use this for initialization
    void Start()
    {
    }

    void OnStartGaze()
    {
        //show the sub menu when the objects parent button is gazed at
        gameObject.GetComponent<BoxCollider>().enabled = false;
        EnableButtonSubMenu(true);

    }
    public void DiasableMenuManager()
    {
        //when there user is no longer gazing on the button or the subbuttons disable the subbuttons
        gameObject.GetComponent<BoxCollider>().enabled = true;
        EnableButtonSubMenu(false);
    }
    void OnEndGaze()
    {
        EnableButtonSubMenu(true);
    }

    private void EnableButtonSubMenu(bool enable)
    {
        transform.Find(SUB_MENU_MANAGER).GetComponent<BoxCollider>().enabled = enable;
        transform.Find(SUB_MENU_MANAGER).GetComponent<SubButtonManager>().enabled = enable;
    }

}
