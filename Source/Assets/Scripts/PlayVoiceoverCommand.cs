using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayVoiceoverCommand : CommandToExecute {

    public AudioClip audio;
    public MuteButtonAction muteButton;

    AudioSource source;

	// Use this for initialization
	override public void Start () {
        source = GetComponent<AudioSource>();
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   override protected Action Command()
    {
        return delegate
        {
            if(!muteButton.IsMuted())
                source.PlayOneShot(audio, 2.0f);
        };
    }

}
