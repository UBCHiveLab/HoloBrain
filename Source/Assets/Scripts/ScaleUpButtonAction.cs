// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScaleUpButtonAction : CommandToExecute {

    //private const string BRAIN_PARTS_NAME = "BrainParts";
    public GameObject scaleObject;
   
    // Use this for initialization
    override protected Action Command()
    {
        //GameObject.Find(BRAIN_PARTS_NAME).GetComponent<ScaleToggler>().ScaleUp();
        return delegate
        {
            scaleObject.GetComponent<ScaleToggler>().ScaleUp();
        };

    }
   
}
