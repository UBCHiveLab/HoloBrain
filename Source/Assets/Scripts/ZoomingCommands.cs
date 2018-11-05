// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomingCommands : MonoBehaviour {
    enum Scale
    {
        Small = 1,
        Default = 2,
        Large = 4
    }

    private const float ZOOM_UPDATE_RATE = 0.05f;

    private CustomMessages customMessages;
    private GameObject brain;
    private Vector3 defaultScale;
    private Scale currentZoom;
    private Scale oldZoom;
    private bool zoomChanging = false;

    private AudioSource soundFX;

    // Use this for initialization
    void Start ()
    {
        customMessages = CustomMessages.Instance;
        // Assign the ToggleZoomMessageReceived() function to be a message handler for ToggleZoom messages
        // MessageHandlers is a dictionary with TestMessageID's as keys and MessageCalback's as values
        customMessages.MessageHandlers[CustomMessages.TestMessageID.scaleChange] = this.ToggleZoomMessageReceived;

        brain = GameObject.Find("BrainParts");
        defaultScale = brain.transform.localScale;
        currentZoom = Scale.Default;

        soundFX = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (zoomChanging)
        {
            ZoomTransition(oldZoom, currentZoom);
        }
    }

    void OnSelect()
    {
        soundFX.Play();
        ToggleZoom();
        customMessages.SendScaleChangeMessage((byte) oldZoom);
    }

    void ToggleZoomMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();

        // The message sends us the old zoom. Setting the currentZoom to this old zoom and then calling
        // ToggleZoom() will put the brain in the same zoom state as the one that broadcasted the message
        currentZoom = (Scale) msg.ReadByte();
        ToggleZoom();
    }

    void ToggleZoom()
    {
        oldZoom = currentZoom;
        currentZoom = NextScale(currentZoom);
        zoomChanging = true;
        ZoomTransition(oldZoom, currentZoom);
    }

    private void ZoomTransition(Scale oldZ, Scale newZ)
    {
        if (newZ < oldZ)
        {
            brain.transform.localScale -= defaultScale * ZOOM_UPDATE_RATE;

            if (brain.transform.localScale.x <= defaultScale.x * (float)newZ / 2.0)
            {
                zoomChanging = false;
            }
        }
        else
        {
            brain.transform.localScale += defaultScale * ZOOM_UPDATE_RATE;

            if (brain.transform.localScale.x >= defaultScale.x * (float)newZ / 2.0)
            {
                zoomChanging = false;
            }
        }

        // Once we are done zooming, the bool is set and ZoomTransition will no longer get called
        /*if (brain.transform.localScale == defaultScale * (int) newZ)
        {
            zoomChanging = false;
        }*/
    }

    static Scale NextScale(Scale scale)
    {
        try
        {
            switch (scale)
            {
                case Scale.Small:
                    return Scale.Default;
                case Scale.Default:
                    return Scale.Large;
                case Scale.Large:
                    return Scale.Small;
                default:
                    return Scale.Default;
            }
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            return Scale.Default;
        }

    }
}
