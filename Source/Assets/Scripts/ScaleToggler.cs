// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToggler : MonoBehaviour
{
    enum Scale
    {
        Small = 1,
        Default = 2,
        Large = 4
    }

    private const float SCALE_CHANGE_UPDATE_RATE = 0.05f;

    private GameObject brainGameObject;
    private Vector3 defaultScale;
    private Scale currentZoom;
    private Scale oldZoom;
    public bool zoomChanging { get; private set; }

    private CustomMessages customMessages;
    private AudioSource soundFX;

    // Use this for initialization
    void Start()
    {
        customMessages = CustomMessages.Instance;
        if (customMessages != null)
        {
            // Assign the ToggleZoomMessageReceived() function to be a message handler for ToggleZoom messages
            // MessageHandlers is a dictionary with TestMessageID's as keys and MessageCalback's as values
            customMessages.MessageHandlers[CustomMessages.TestMessageID.scaleChange] = this.ScaleChangeMessageReceived;
        }

        brainGameObject = GameObject.Find("BrainParts");
        defaultScale = brainGameObject.transform.localScale;
        ResetScale();

        soundFX = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (zoomChanging)
        {
            ZoomTransition(oldZoom, currentZoom);

        }
    }

    public void OnSelect()
    {
        if (this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            ToggleScale();
        }
    }

    void ScaleChangeMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();

        oldZoom = currentZoom;
        currentZoom = (Scale) msg.ReadByte();
        zoomChanging = true;
    }

    public void ToggleScale()
    {
        if (!this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            return;
        }

            soundFX.Play();
        oldZoom = currentZoom;
        currentZoom = NextScale(currentZoom);
        zoomChanging = true;

        if (customMessages != null)
        {
            customMessages.SendScaleChangeMessage((byte)currentZoom);
        }
    }

    public void ScaleUp()
    {

        soundFX.Play();

        if (!this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            return;
        }


        switch (currentZoom)
        {
            case Scale.Small:
                oldZoom = currentZoom;
                currentZoom = Scale.Default;
                break;
            case Scale.Default:
                oldZoom = currentZoom;
                currentZoom = Scale.Large;
                break;
            case Scale.Large:
                return;
        }
        zoomChanging = true;

        if (customMessages != null)
        {
            customMessages.SendScaleChangeMessage((byte)currentZoom);
        }
    }

    public void ScaleDown()
    {

        soundFX.Play();

        if (!this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            return;
        }


        switch (currentZoom)
        {
            case Scale.Small:
                return;
            case Scale.Default:
                oldZoom = currentZoom;
                currentZoom = Scale.Small;
                break;
            case Scale.Large:
                oldZoom = currentZoom;
                currentZoom = Scale.Default;
                break;
        }
        zoomChanging = true;

        if (customMessages != null)
        {
            customMessages.SendScaleChangeMessage((byte)currentZoom);
        }
    }

    private void ZoomTransition(Scale oldZ, Scale newZ)
    {
        if (newZ < oldZ)
        {
            brainGameObject.transform.localScale -= defaultScale * SCALE_CHANGE_UPDATE_RATE;

            if (brainGameObject.transform.localScale.x <= defaultScale.x * (float)newZ / 2.0)
            {
                zoomChanging = false;
            }
        }
        else
        {
            brainGameObject.transform.localScale += defaultScale * SCALE_CHANGE_UPDATE_RATE;

            if (brainGameObject.transform.localScale.x >= defaultScale.x * (float)newZ / 2.0)
            {
                zoomChanging = false;
            }
        }
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

    public void ResetScale()
    {
        brainGameObject.transform.localScale = defaultScale;
        oldZoom = Scale.Default;
        currentZoom = Scale.Default;
        zoomChanging = false;
    }
}
