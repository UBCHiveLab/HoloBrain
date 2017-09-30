// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCommands : MonoBehaviour {

    public bool buttonIsEnabled { get; private set; }

    private ControlsUIManager controlsUI;

    private const string GAZE_FRAME_NAME = "white-border";
    Color FullOpacityColor;
    Color PartialOpacityColor;
    bool IsPressed;


    private void Start()
    {
        buttonIsEnabled = true;
        IsPressed = false;
        FullOpacityColor = new Color(1, 1, 1, 1);
        PartialOpacityColor = new Color(1, 1, 1, 0.63f);
        controlsUI = transform.GetComponentInParent<ControlsUIManager>();
        
        //disable the white selection frame
        EnableOrDisableFrame(false);
    }

    void OnStartGaze()
    {
        //let the UIManager know that it is being gazed at
        controlsUI.OnGazeEnteredUI();

        //visual change of the button on gaze over
        EnableOrDisableFrame(true);
    }

    void OnEndGaze()
    {
        //let the UIManager know that it is no longer being gazed at
        controlsUI.OnGazeExitUI();

        //visual change of the button on gaze over
        EnableOrDisableFrame(false);
    }

    public void OnSelect()
    {
       
            changeButtonAppearance();
        
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

    //enables or disables the white frame surrounding the button gaze
    private void EnableOrDisableFrame(bool frameState)
    {
        if (transform.Find(GAZE_FRAME_NAME) != null)
            transform.Find(GAZE_FRAME_NAME).GetComponent<SpriteRenderer>().enabled = frameState;
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

    public void ReturnButtonToOriginalState()
    {
        //if the button is at full opacity turn it off
        if (gameObject.GetComponent<ButtonEnabledFeedback>())
        {
            gameObject.GetComponent<ButtonEnabledFeedback>().ToggleOpacity(false);
        }
        //if the frame is gazed at disable the gaze
        EnableOrDisableFrame(false);
        //if the button alternated states return it back to the original poistion
        if (gameObject.GetComponent<ButtonSwapFeedback>())
        {
            gameObject.GetComponent<ButtonSwapFeedback>().ResetButtonState();
        }

    }

}
