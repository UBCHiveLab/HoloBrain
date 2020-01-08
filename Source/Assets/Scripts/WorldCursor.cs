// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity;


public class WorldCursor : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    public float MaxGazeDistance = 5.0f;

    public GameObject CursorOnHolograms;
    public GameObject CursorOffHolograms;

    private bool handIsShown;
    private HTGazeManager gazeManager;

    void Start()
    {
        // Grab the mesh renderer that's on the same object as this script.
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        handIsShown = false;
        InteractionManager.InteractionSourceDetected += InteractionManager_SourceDetected;
        InteractionManager.InteractionSourceLost += InteractionManager_SourceLost;
        gazeManager = HTGazeManager.Instance;

    }

    void Awake()
    {
        if (CursorOnHolograms == null || CursorOffHolograms == null)
        {
            return;
        }

        // Hide the Cursors to begin with.
        CursorOnHolograms.SetActive(false);
        CursorOffHolograms.SetActive(false);
    }


    void Update()
    {
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = gazeManager.gazeOrigin;
        var gazeDirection = gazeManager.gazeDirection;

        if(gazeManager.Hit)
        {
            if(handIsShown)
            {
                CursorOnHolograms.SetActive(true);
                CursorOffHolograms.SetActive(false);
            }else
            {
                CursorOnHolograms.SetActive(false);
                CursorOffHolograms.SetActive(true);
            }

            // Move thecursor to the point where the raycast hit.
            this.transform.position = gazeManager.Position;

            // Rotate the cursor to hug the surface of the hologram.           
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, gazeManager.Normal);
        }
        else
        {
            // If the raycast did not hit a hologram, hide the cursor mesh.
            CursorOnHolograms.SetActive(false);
            CursorOffHolograms.SetActive(true);

            this.transform.position = headPosition + (gazeDirection * MaxGazeDistance);
            this.transform.up = gazeDirection;
        }
    }

    private void InteractionManager_SourceDetected(UnityEngine.XR.WSA.Input.InteractionSourceDetectedEventArgs args)
    {
        handIsShown = true;
    }

    private void InteractionManager_SourceLost(UnityEngine.XR.WSA.Input.InteractionSourceLostEventArgs args)
    {
        handIsShown = false;
    }
}