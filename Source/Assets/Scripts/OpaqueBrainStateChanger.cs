// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpaqueBrainStateChanger : MonoBehaviour {
    private const string CORTEX_MODEL_NAME = "cortex_low";
    private const string VENTRICLE_MODEL_NAME = "ventricle";
    private const string BRAIN_ANIMATOR_BOOL = "BrainActive";
    private const int TOGGLE_COUNTER_LIMIT = 100;
    private const float VENTRICLE_LARGE_SCALE = 1f;
    private const float VENTRICLE_SMALL_SCALE = 0.25f;

    private CustomMessages customMessages;
    private Animator cortexAnimator;
    private BoxCollider cortexCollider;
    private GameObject ventricle;
    private bool brainIsOpaque;
    private AudioSource soundFX;

	// Use this for initialization
	void Start () {
        customMessages = CustomMessages.Instance;
        // Assign the ToggleZoomMessageReceived() function to be a message handler for ToggleZoom messages
        // MessageHandlers is a dictionary with TestMessageID's as keys and MessageCalback's as values
        if (customMessages != null)
        {
            customMessages.MessageHandlers[CustomMessages.TestMessageID.ToggleOpacity] = this.ToggleOpacityMessageReceived;
        }

        ventricle = GameObject.Find(VENTRICLE_MODEL_NAME);
        cortexAnimator = GameObject.Find(CORTEX_MODEL_NAME).GetComponent<Animator>();
        cortexCollider = GameObject.Find(CORTEX_MODEL_NAME).GetComponent<BoxCollider>();
        soundFX = gameObject.GetComponent<AudioSource>();
        cortexCollider.enabled = false;
        brainIsOpaque = false;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void OnSelect()
    {
        if (customMessages != null)
        {
            customMessages.SendToggleOpacityMessage(brainIsOpaque ? (byte)1 : (byte)0);
        }
        ToggleOpacityState();
    }

    public void ToggleOpacityMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();

        // The message sends us the old opacity
        brainIsOpaque = msg.ReadByte() == 1;
        ToggleOpacityState();
    }

    private void ToggleOpacityState()
    {
        soundFX.Play();
        brainIsOpaque = !brainIsOpaque;
        cortexAnimator.SetBool(BRAIN_ANIMATOR_BOOL, brainIsOpaque);
        if(brainIsOpaque)
        {
            //cortexCollider.enabled = true;
            //ventricle.transform.localScale = new Vector3(VENTRICLE_SMALL_SCALE,VENTRICLE_SMALL_SCALE,VENTRICLE_SMALL_SCALE);
            ventricle.SetActive(false);
        }
        else
        {
           // cortexCollider.enabled = false;
            //ventricle.transform.localScale = new Vector3(VENTRICLE_LARGE_SCALE, VENTRICLE_LARGE_SCALE, VENTRICLE_LARGE_SCALE);
            ventricle.SetActive(true);
        }
    }
}
