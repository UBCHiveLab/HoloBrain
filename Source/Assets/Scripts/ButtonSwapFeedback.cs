// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwapFeedback : MonoBehaviour {

    public Sprite StartIcon;
    public Sprite EndIcon;
    public string StartVoiceCommand;
    public string EndVoiceCommand;
    bool StartIconisOn;
    GameObject VoiceToolTip;
    private const string VOICE_COMMAND_TOOLTIP_NAME = "VoiceCommandTooltip";


    // Use this for initialization
    void Start () {
//        StartIconisOn = true;
//        VoiceToolTip = gameObject.transform.FindChild(VOICE_COMMAND_TOOLTIP_NAME).gameObject;
    }

    private void OnEnable()
    {
        if (VoiceToolTip == null)
        {
            StartIconisOn = true;
            VoiceToolTip = gameObject.transform.Find(VOICE_COMMAND_TOOLTIP_NAME).gameObject;
        }
    }

    public void ToggleButtonImage()
    {
        if (StartIconisOn)
        {
            if (EndIcon != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = EndIcon;

            }
            if (EndVoiceCommand != null)
            {
                Debug.Log("end voice command should change");
                VoiceToolTip.GetComponent<Tooltip>().Text = EndVoiceCommand;
                Debug.Log("end voice command changed");


            }
            StartIconisOn = false;
        }else
        {
            if (StartIcon != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = StartIcon;

            }
            if (StartVoiceCommand != null)
            {
                VoiceToolTip.GetComponent<Tooltip>().Text = StartVoiceCommand;

            }
            StartIconisOn = true;
        }
       
    }  

    public void ResetButtonState()
    {
        //if the reset button is pressed , return button icons to their original state
        if (gameObject.name != "compare-icon")
        {
            Debug.Log("in reset button " + gameObject.name);
            StartIconisOn = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = StartIcon;
            VoiceToolTip.GetComponent<Tooltip>().Text = StartVoiceCommand;
        }
    }

}
