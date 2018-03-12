// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedBrainButtonController : UIStateModel
{

    public Sprite IconBothBrains;
    public string VoiceCommandSelectedBothBrains;
    public Sprite IconBrain_1;
    public string VoiceCommandSelectedBrain_1;
    public Sprite IconBrain_2;
    public string VoiceCommandSelectedBrain_2;

    GameObject VoiceToolTip;
    private const string VOICE_COMMAND_TOOLTIP_NAME = "VoiceCommandTooltip";

    public PlayMakerFSM fsm;
    private Sprite SelectedBrainIconToRender;
    private string SelectedVoiceCommand;
    private FsmEnum selectedBrainEnum;

    public void IncrementAndRenderSelectedBrainButton() {
        selectedBrainEnum = fsm.FsmVariables.GetFsmEnum("SelectedBrain");
        switch ((SelectedBrain)selectedBrainEnum.Value) {
            case SelectedBrain.Both_Brains: selectedBrainEnum.Value = SelectedBrain.Brain_1; break;
            case SelectedBrain.Brain_1: selectedBrainEnum.Value = SelectedBrain.Brain_2; break;
            case SelectedBrain.Brain_2: selectedBrainEnum.Value = SelectedBrain.Both_Brains; break;
        }
        RenderSelectedBrainButtonImage();
    }

    private void OnEnable()
    {
        if (VoiceToolTip == null) {
            VoiceToolTip = gameObject.transform.Find(VOICE_COMMAND_TOOLTIP_NAME).gameObject;
        }
    }

    public void RenderSelectedBrainButtonImage()
    {
        selectedBrainEnum = fsm.FsmVariables.GetFsmEnum("SelectedBrain");
        switch ((SelectedBrain)selectedBrainEnum.Value) {
            case SelectedBrain.Both_Brains:
                SelectedBrainIconToRender = IconBothBrains;
                SelectedVoiceCommand = VoiceCommandSelectedBothBrains;
                break;
            case SelectedBrain.Brain_1:
                SelectedBrainIconToRender = IconBrain_1;
                SelectedVoiceCommand = VoiceCommandSelectedBrain_1;
                break;
            case SelectedBrain.Brain_2:
                SelectedBrainIconToRender = IconBrain_2;
                SelectedVoiceCommand = VoiceCommandSelectedBrain_2;
                break;
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = SelectedBrainIconToRender;
        VoiceToolTip.GetComponent<Tooltip>().Text = SelectedVoiceCommand;
    }  /*

    public void ResetButtonState()
    {
        //if the reset button is pressed , return button icons to their original state
        Debug.Log("in reset button " + gameObject.name);
        SelectedBrainName = SelectedBrain.Both_Brains;
        gameObject.GetComponent<SpriteRenderer>().sprite = StartIcon;
        VoiceToolTip.GetComponent<Tooltip>().Text = VoiceCommandBothBrains;
    }*/

}
