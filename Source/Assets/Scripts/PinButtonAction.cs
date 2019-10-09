// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PinButtonAction : CommandToExecute {

    private ControlsUIManager UIManager;

    // Use this for initialization
    override public void Start () {
        UIManager = transform.GetComponentInParent<ControlsUIManager>();
        base.Start();
	}

    override protected Action Command()
    {
        return delegate
        {
            if (UIManager != null)
            {
                UIManager.TogglePinUI();
            }
        };
    }
    /*private void OnEnable()
    {
        if(UIManager == null)
        {
            UIManager = transform.GetComponentInParent<ControlsUIManager>();
        }

        if (UIManager.GetMenuPinState())
        {
                Debug.Log("In the on enable pin state is true");
                gameObject.GetComponent<ButtonSwapFeedback>().ToggleButtonImage();
                Debug.Log("In the on enable pin state is true: image toggled");
        }
    }*/

}
