// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HolobrainConstants;

public class ExplodeButtonAction : CommandToExecute
{
    public List<ButtonAppearance> buttonsToDisable;
    public GameObject brain;
    override public void Start()
    {
        if (brain == null)
        {
            brain = GameObject.Find(Names.BRAIN_GAMEOBJECT_NAME);
        }
        Debug.Log("Rotate button brain variable is pointing to " + brain.name);
        base.Start();
    }


    //this is bad coupling. there are some prereqs to explode command but switchroomui will switch the buttons anyways
    override protected Action Command()
    {
        return delegate
        {
            if (buttonsToDisable != null)
            {
                //turning off rotate, enabled isolate unless rotating
                if (brain.GetComponent<ExplodingCommands>().Exploded() && !brain.GetComponent<RotateStructures>().isRotating)
                {
                    foreach (ButtonAppearance ba in buttonsToDisable)
                    {
                        ba.SetButtonEnabled ();
                    }
                }
                //rotating, isolate disabled
                else
                {
                    foreach (ButtonAppearance ba in buttonsToDisable)
                    {
                        ba.SetButtonDisabled();
                    }
                }
            }
            brain.GetComponent<ExplodingCommands>().ToggleExplode();
        };
    }
}
