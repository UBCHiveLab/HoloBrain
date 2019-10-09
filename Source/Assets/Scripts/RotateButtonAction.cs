// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateButtonAction : CommandToExecute {

    public GameObject brain;
 
    // Use this for initialization
    override public void Start () {
        if (brain == null)
        {
            brain = GameObject.Find("Brain");
        }
        Debug.Log("Rotate button brain variable is pointing to " + brain.name);
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    override protected Action Command()
    {
        //do the action
        return delegate
        {
            brain.GetComponent<RotateStructures>().ToggleRotate();
        };
    }

}
