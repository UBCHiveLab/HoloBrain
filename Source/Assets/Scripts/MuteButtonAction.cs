using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ButtonAppearance))]
public class MuteButtonAction : CommandToExecute {


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
    override public void Start()
    {
        prompt = GetComponent<VoiceCommandPrompt>();
        mutedState = false;
        base.Start();
    }

    override protected Action Command()
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
            }
            else
            {
                if (prompt != null)
                {
                    prompt.ChangePrompt("Unmute");
                }
                Mute();
            }
        };
    }
}
