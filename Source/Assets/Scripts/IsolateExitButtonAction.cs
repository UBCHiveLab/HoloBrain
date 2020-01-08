// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Unity;

public class IsolateExitButtonAction : CommandToExecute {

    public List<ButtonAppearance> buttonsToEnable;
    private const string BRAIN_PARTS_NAME = "Brain";
    GameObject brain;
 
    // Use this for initialization
    override public void Start()
    {
        brain = GameObject.Find(BRAIN_PARTS_NAME);
        base.Start();
    }

    override protected Action Command()
    {
        return delegate
        {
            if(buttonsToEnable != null)
            {
                foreach(ButtonAppearance ba in buttonsToEnable)
                {
                    ba.SetButtonEnabled();
                }
            }
            FindObjectOfType<MoveClippingPlane>().TurnOnClipping();
            brain.GetComponent<IsolateStructures>().ConcludeIsolationMode();
        };
    }
}
