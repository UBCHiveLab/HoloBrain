// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿///////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  GazeMarker instructions
//  
//  In order to add the GazeMarker, follow these instructions. Please note that the location of the gaze marker
//  is not shared properly and would need fixing.
//  
//  IN UNITY
//  1. Add the GazeMarker prefab to the HolographicBrainProject scene as child of the "Brain" game object.
//  2. Link this script (GazeMarkerManager.cs) to the "BrainParts" game object.
//  3. Slide the "GazeMarker" game object into the "Marker" field of this script (on BrainParts game object)
//      using the inspector.
//  4. Link the "GazeMarkerCommands.cs" script to every child of the "BrainParts" game object except the cortex.
//  
//  IN VISUAL STUDIO
//  Uncomment every line that is under a comment that says "UNCOMMENT THIS FOR GAZE MARKER" in the following scripts:
//      -HTGestureManager.cs
//      -IsolateStructures.cs
//      -VoiceControls.cs
//      -RotateStructures.cs
//  
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeMarkerManager : MonoBehaviour {

    public GameObject Marker;
    private GameObject pointedPart;
    private MeshRenderer markerRenderer;
    private HTGazeManager gazeManager;

    private StateAccessor state;
    private CustomMessages customMessages;
    private AudioSource soundFX;

    // Use this for initialization
    void Start () {
        customMessages = CustomMessages.Instance;
        if (customMessages != null)
        {
            customMessages.MessageHandlers[CustomMessages.TestMessageID.SetPositionOfGazeMarker] = SetPositionOfGazeMarkerMessageReceived;
            customMessages.MessageHandlers[CustomMessages.TestMessageID.ClearGazeMarker] = ClearGazeMarkerMessageReceived;
        }

        markerRenderer = Marker.GetComponent<MeshRenderer>();
        markerRenderer.enabled = false;

        state = this.GetComponent<StateAccessor>();
        gazeManager = transform.GetComponentInParent<HTGazeManager>();
        soundFX = gameObject.GetComponent<AudioSource>();
    }

    public void OnEmptyTap()
    {
        TryToRemoveGazeMarker();
    }

    public bool TryToMoveGazeMarker()
    {
        if (!state.CurrentlyInStudentMode() && state.CurrentlyIsolatedOrIsolating())
        {
            if (gazeManager.Hit) {
                if (customMessages != null)
                {
                    CustomMessages.Instance.SendSetGazeMarkerPositionMessage(gazeManager.Position, gazeManager.HitInfo.transform.name);
                }
                
                PlaceMarker(gazeManager.Position, gazeManager.HitInfo.transform.name);
                return true;
            }
        }
        return false;
    }

    public void TryToRemoveGazeMarker()
    {
        if (!state.CurrentlyInStudentMode())
        {
            if (customMessages != null)
            {
                CustomMessages.Instance.SendClearGazeMarkerMessage();
            }

            ClearMarkerLocally();
        }
    }

    public void SetPositionOfGazeMarkerMessageReceived(NetworkInMessage msg)
    {
        msg.ReadInt64();

        PlaceMarker(customMessages.ReadVector3(msg), msg.ReadString());
    }

    public void ClearGazeMarkerMessageReceived(NetworkInMessage msg)
    {
        ClearMarkerLocally();
    }

    void PlaceMarker(Vector3 markerPosition, string partName)
    {
        if (pointedPart != null)
        {
            pointedPart.GetComponent<GazeMarkerCommands>().MarkerIsPointing(false);
        }
        pointedPart = GameObject.Find(partName);
        pointedPart.GetComponent<GazeMarkerCommands>().MarkerIsPointing(true);

        Marker.transform.position = markerPosition;
        markerRenderer.enabled = true;

        soundFX.Play();
    }

    public void ClearMarkerLocally()
    {
        if (pointedPart != null)
        {
            pointedPart.GetComponent<GazeMarkerCommands>().MarkerIsPointing(false);
            pointedPart = null;
        }

        markerRenderer.enabled = false;

        if (state.CurrentlyIsolatedOrIsolating())
        {
            soundFX.Play();
        }
    }
}
