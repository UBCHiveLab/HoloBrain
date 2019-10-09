// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplodeButtonAction : CommandToExecute
{
    private const string BRAIN_PARTS_NAME = "Brain";
    

    //this is bad coupling. there are some prereqs to explode command but switchroomui will switch the buttons anyways
    override protected Action Command()
    {
        return delegate
        {
            GameObject.Find(BRAIN_PARTS_NAME).GetComponent<ExplodingCommands>().ToggleExplode();
        };
    }
}
