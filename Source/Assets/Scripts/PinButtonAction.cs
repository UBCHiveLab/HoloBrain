// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinButtonAction : MonoBehaviour {

    private ControlsUIManager UIManager;
    private AudioSource audio;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        UIManager = transform.GetComponentInParent<ControlsUIManager>();
	}
   

    public void OnSelect()
    {
        audio.Play();
        UIManager.TogglePinUI();
    }
    private void OnEnable()
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
    }

}
