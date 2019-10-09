// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Unity.InputModule;

public class RepositionButtonAction : CommandToExecute {
    
    override protected Action Command()
    {
        return delegate
        {
            Debug.Log("calling reposition");
            HologramPlacement.Instance.ResetStage();
        };
    }
}
