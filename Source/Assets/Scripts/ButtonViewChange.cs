// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonViewChange : MonoBehaviour {

    bool IsPressed;
    // Use this for initialization
    void Start () {
        IsPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TurnOnFullOpacity(bool isFullOpacity)
    {
        if (gameObject.GetComponent<ButtonTapFeedback>() != null)
        {
            if (isFullOpacity)
            {
                gameObject.GetComponent<ButtonTapFeedback>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<ButtonTapFeedback>().enabled = false;
            }
        }
    }
    private void SwapImage()
    {
        if (gameObject.GetComponent<ButtonSwapFeedback>() != null)
        {
            gameObject.GetComponent<ButtonSwapFeedback>().ToggleButtonImage();

        }
    }

    private void ChangeOpacity()
    {
        if (gameObject.GetComponent<ButtonEnabledFeedback>() != null)
        {
            gameObject.GetComponent<ButtonEnabledFeedback>().ToggleOpacity(IsPressed);

        }
    }
    private void ChangeMenu()
    {
        if (gameObject.GetComponent<ToggleMenu>() != null)
        {
            gameObject.GetComponent<ToggleMenu>().ChangeMenu();
        }
    }

    public void changeButtonAppearance()
    {
        IsPressed = !IsPressed;
        //briefly turn on the opacity when the button is tapped
        TurnOnFullOpacity(true);
        //if the button alternates between 2 actions , swap the button icon to show that to the user
        SwapImage();
        //change the button to fully opaque if needed
        ChangeOpacity();
        //change the ui menus appearing if needed
        ChangeMenu();
        //resetButton
    }
}
