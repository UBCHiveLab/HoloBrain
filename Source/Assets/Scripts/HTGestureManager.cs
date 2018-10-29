// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using System.Collections;

using HoloToolkit.Unity;

/// <summary>
/// GestureManager creates a gesture recognizer and signs up for a tap gesture.
/// When a tap gesture is detected, GestureManager uses GazeManager to find the game object.
/// GestureManager then sends a message to that game object.
/// </summary>
[RequireComponent(typeof(HTGazeManager))]
public class HTGestureManager : Singleton<HTGestureManager>
{
    /// <summary>
    /// To select even when a hologram is not being gazed at,
    /// set the override focused object.
    /// If its null, then the gazed at object will be selected.
    /// </summary>
    public GameObject OverrideFocusedObject
    {
        get; set;
    }

    private UnityEngine.XR.WSA.Input.GestureRecognizer gestureRecognizer;
    private GameObject focusedObject;
    private GameObject brainParts;
    void Start()
    {
        // Create a new GestureRecognizer. Sign up for tapped events.
        gestureRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(UnityEngine.XR.WSA.Input.GestureSettings.Tap);

        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;

        // Start looking for gestures.
        gestureRecognizer.StartCapturingGestures();

        brainParts = GameObject.Find("BrainParts");
    }

    private void GestureRecognizer_TappedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (focusedObject != null)
        {
            focusedObject.SendMessage("OnSelect");
        }
        //UNCOMMENT THIS FOR GAZE MARKER
        else if (brainParts != null)
        {
            brainParts.SendMessage("OnEmptyTap");
        }
    }

    void LateUpdate()
    {
        GameObject oldFocusedObject = focusedObject;

        if (HTGazeManager.Instance.Hit &&
            OverrideFocusedObject == null &&
            HTGazeManager.Instance.HitInfo.collider != null)
        {
            // If gaze hits a hologram, set the focused object to that game object.
            // Also if the caller has not decided to override the focused object.
            focusedObject = HTGazeManager.Instance.HitInfo.collider.gameObject;
        }
        else
        {
            // If our gaze doesn't hit a hologram, set the focused object to null or override focused object.
            focusedObject = OverrideFocusedObject;
        }

        if (focusedObject != oldFocusedObject)
        {
            if (oldFocusedObject != null)
            {
                oldFocusedObject.SendMessageUpwards("OnEndGaze");
            }
            if (focusedObject != null)
            {
                focusedObject.SendMessageUpwards("OnStartGaze");
            }

            // If the currently focused object doesn't match the old focused object, cancel the current gesture.
            // Start looking for new gestures.  This is to prevent applying gestures from one hologram to another.
            gestureRecognizer.CancelGestures();
            gestureRecognizer.StartCapturingGestures();
        }
    }


    void OnDestroy()
    {
        // gestureRecognizer.StopCapturingGestures();
        // gestureRecognizer.TappedEvent -= GestureRecognizer_TappedEvent;
    }
}