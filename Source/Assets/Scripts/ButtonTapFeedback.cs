// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTapFeedback : MonoBehaviour {

    //make the button fully opaque breifly when the button is tapped

    int opacityTimer;
    Color FullOpacityColor;
    Color PartialOpacityColor;
    // Use this for initialization
    void Start () {
        opacityTimer = 10;
        FullOpacityColor = new Color(1, 1, 1, 1);
        PartialOpacityColor = new Color(1, 1, 1, 0.8f);
    }

    private void OnEnable()
    {
        //reset the timer every time the button is tapped
        opacityTimer = 10;
        gameObject.GetComponent<SpriteRenderer>().color = FullOpacityColor;
    }

    // Update is called once per frame
    void Update () {
        if (opacityTimer == 0)
        {
            //once the timer reaches zero change the opacity back to original and disable the script
            gameObject.GetComponent<SpriteRenderer>().color = PartialOpacityColor;
            //call the method to disable
            gameObject.GetComponent<ButtonCommands>().TurnOnFullOpacity(false);
        }
        else
        {
            opacityTimer--;
        }
	}
}
