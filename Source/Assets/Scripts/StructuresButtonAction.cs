// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StructuresButtonAction : CommandToExecute {

    private const string BRAIN_PARTS_NAME = "BrainParts";
    GameObject brain;
    private StateAccessor stateAccessor;

    override public void Start()
    {
        stateAccessor = StateAccessor.Instance;
        brain = GameObject.Find(BRAIN_PARTS_NAME);
        base.Start();
    }

    override protected Action Command()
    {
        //go back to the default mode
        return delegate
        {
            if (stateAccessor.GetCurrentMode() == StateAccessor.Mode.Default)
            {
                Debug.Log("the state accessor is default");
                return;
            }
            brain.GetComponent<ResetState>().ResetEverything();
        };
   }
}
