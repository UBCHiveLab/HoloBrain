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
    bool StartIconisOn_b1, StartIconisOn_b2;
    GameObject VoiceToolTip;
    private const string VOICE_COMMAND_TOOLTIP_NAME = "VoiceCommandTooltip";
    StateAccessor stateAccessor;
    GameObject selectBrainControlGameObject;


    // Use this for initialization
    void Start ()
    {
        //        StartIconisOn = true;
        //        VoiceToolTip = gameObject.transform.FindChild(VOICE_COMMAND_TOOLTIP_NAME).gameObject;
        stateAccessor = StateAccessor.Instance;
        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
    }

    private void OnEnable()
    {
        if (VoiceToolTip == null)
        {
            StartIconisOn_b1 = true;
            StartIconisOn_b2 = true;
            VoiceToolTip = gameObject.transform.Find(VOICE_COMMAND_TOOLTIP_NAME).gameObject;
            
        }
    }

   void Update()
    {
        if (gameObject.name != "compare-icon")
        {
            if (GetStartIconState())
            {
                if (StartIcon != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = StartIcon;

                }
                if (StartVoiceCommand != null)
                {
                    VoiceToolTip.GetComponent<Tooltip>().Text = StartVoiceCommand;

                }
            } else
            {
                if (EndIcon != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = EndIcon;

                }
                if (EndVoiceCommand != null)
                {
                    VoiceToolTip.GetComponent<Tooltip>().Text = EndVoiceCommand;
                }
            }
        }
    }

    public void ToggleButtonImage()
    {
        if (GetStartIconState())
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
            SetStartIconState(false);
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
            SetStartIconState(true);
        }
       
    }  

    public void ResetButtonState()
    {
        //if the reset button is pressed , return button icons to their original state
        if (gameObject.name != "compare-icon")
        {
            Debug.Log("in reset button " + gameObject.name);
            ResetIconState();
            gameObject.GetComponent<SpriteRenderer>().sprite = StartIcon;
            VoiceToolTip.GetComponent<Tooltip>().Text = StartVoiceCommand;
        }
    }

    bool GetStartIconState()
    {
        if (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain == "Brain" || gameObject.name == "compare-icon")
        {
            return StartIconisOn_b1;
        } else
        {
            return StartIconisOn_b2;
        }
    }

    void SetStartIconState(bool state)
    {
        if (gameObject.name == "compare-icon")
        {
            StartIconisOn_b1 = state;
            return;
        }

        if (stateAccessor.IsInCompareMode())
        {
            StartIconisOn_b1 = state;
            StartIconisOn_b2 = state;
        }
        else if (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain == "Brain")
        {
           StartIconisOn_b1 = state;
        }
        else
        {
            StartIconisOn_b2 = state;
        }
    }

    void ResetIconState()
    {
        StartIconisOn_b1 = true;
        StartIconisOn_b2 = true;
    }

}
