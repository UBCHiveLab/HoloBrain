using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ButtonCommands))]
public class VoiceOverCommand : MonoBehaviour {

    //public Audio;
	// Use this for initialization
	void Start () {
		
	}

    private Action ExecuteVoiceOver()
    {
        return delegate
        {

        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
