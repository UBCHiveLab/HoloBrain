// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwapHighlightFeedback : MonoBehaviour {

    public Sprite StartIcon;
    public SpriteRenderer HighlightFrame;
    public string StartVoiceCommand;
    public string EndVoiceCommand;
    bool StartIconisOn;
    GameObject VoiceToolTip;
    private const string VOICE_COMMAND_TOOLTIP_NAME = "VoiceCommandTooltip";
    private const string HIGHLIGHT_FRAME = "HighlightFrame";


    // Use this for initialization
    void Start () {
        HighlightFrame = transform.Find(HIGHLIGHT_FRAME).GetComponent<SpriteRenderer>();
        HighlightFrame.enabled = false;
    }

    private void OnEnable()
    {
        if (VoiceToolTip == null)
        {
            StartIconisOn = true;

            if (gameObject.transform.Find(VOICE_COMMAND_TOOLTIP_NAME) != null)
            {
                VoiceToolTip = gameObject.transform.Find(VOICE_COMMAND_TOOLTIP_NAME).gameObject;
            }
        }
    }

    public void ToggleButtonImage()
    {
        if (StartIconisOn)
        {
            if (HighlightFrame != null)
            {
                HighlightFrame.enabled = true;

            }
            if (EndVoiceCommand != null && VoiceToolTip != null)
            {
                Debug.Log("end voice command should change");
                VoiceToolTip.GetComponent<Tooltip>().Text = EndVoiceCommand;
                Debug.Log("end voice command changed");


            }
            StartIconisOn = false;
        } else
        {

            HighlightFrame.enabled = false;

            if (StartVoiceCommand != null && VoiceToolTip != null)
            {
                VoiceToolTip.GetComponent<Tooltip>().Text = StartVoiceCommand;

            }
            StartIconisOn = true;
        }
       
    }  

    public void ResetButtonState()
    {
        //if the reset button is pressed , return button icons to their original state
        HighlightFrame.enabled = false;
        Debug.Log("in reset button " + gameObject.name);
        StartIconisOn = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = StartIcon;
        if (VoiceToolTip != null)
        {
            VoiceToolTip.GetComponent<Tooltip>().Text = StartVoiceCommand;
        }
    }

}
