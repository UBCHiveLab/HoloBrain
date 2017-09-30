// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnabledFeedback : MonoBehaviour {

    //When the button is pressed change its colour to indicate that the action is happening
    Color FullOpacityColor;
    Color PartialOpacityColor;

    // Use this for initialization
    void Start () {
        FullOpacityColor = new Color(1, 1, 1, 1);
        PartialOpacityColor = new Color(1, 1, 1, 0.39f);
    }

    //change the opacity of the button image
    public void ToggleOpacity(bool IsFullyOpaque)
    { 
        gameObject.GetComponent<SpriteRenderer>().color = IsFullyOpaque ? FullOpacityColor : PartialOpacityColor;
    }
}
