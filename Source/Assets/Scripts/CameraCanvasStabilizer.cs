// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CameraCanvasStabilizer : MonoBehaviour {

    private Vector3 childInitialPositions;
    private Transform child;

    private GazeStabilizer stabilizer;

    private bool isRendered = true;

    public bool IsBeingRendered
    {
        get
        {
            return isRendered;
        }
        set
        {
            isRendered = value;
        }
    }

	// Use this for initialization
	void Start () {
        child = transform.GetChild(0);
        childInitialPositions = child.localPosition;

        stabilizer = GameObject.Find("HologramCollection").GetComponent<GazeStabilizer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRendered)
        {
            if (stabilizer != null)
            {
                child.localPosition = (stabilizer.StableRay.direction - transform.forward) + childInitialPositions;
            }

            child.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }
    }
}
