using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ButtonCommands), typeof(AudioSource))]
public class PlayVoiceoverCommand : MonoBehaviour {

    public AudioClip audio;
    public MuteButtonAction muteButton;

    AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        GetComponent<ButtonCommands>().AddCommand(PlayAudioClipAction());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Action PlayAudioClipAction()
    {
        return delegate
        {
            if(!muteButton.IsMuted())
                source.PlayOneShot(audio, 2.0f);
        };
    }

}
