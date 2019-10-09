// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IsolateExitButtonAction : CommandToExecute {

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
            brain.GetComponent<IsolateStructures>().ConcludeIsolationMode();
        };
    }
}
