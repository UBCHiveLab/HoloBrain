// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonAction : MonoBehaviour {

    void OnSelect()
    {
        //if the button is enabled
        if (gameObject.GetComponent<ButtonCommands>().buttonIsEnabled)
        {
            //do the action
        }
    }
}
