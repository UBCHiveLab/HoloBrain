using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ButtonCommands), typeof(ButtonAppearance))]
public class MuteButtonAction : MonoBehaviour {


    private bool mutedState;
    private VoiceCommandPrompt prompt;

    public bool IsMuted()
    {
        return mutedState;
    }

    private void Mute()
    {
        mutedState = true;
    }

    private void UnMute()
    {
        mutedState = false;
    }

    private void ToggleMute()
    {
        mutedState = !mutedState;
    }
    // Use this for initialization
    void Start()
    {
        prompt = GetComponent<VoiceCommandPrompt>();
        GetComponent<ButtonCommands>().AddCommand(MuteAction());
        mutedState = false;
    }

    private Action MuteAction()
    {
        return delegate
        {
            if (mutedState)
            {
                if(prompt != null)
                {
                    prompt.ChangePrompt("Mute");
                }
                UnMute();
                GetComponent<ButtonAppearance>().ResetButton();
            }
            else
            {
                if (prompt != null)
                {
                    prompt.ChangePrompt("Unmute");
                }
                Mute();
                GetComponent<ButtonAppearance>().SetButtonActive();
            }
        };
    }
}
