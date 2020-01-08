// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class LabelDisplayManager : MonoBehaviour {
    
    private const float DefaultBrainRadius = 0.077f;
    private float brainRadius;

    //minDistance at which the label display can be rendered
    //is slightly more than the camera's clipping plane.
    private float minDistance;

    //maxDistance at which the label display can be rendered
    // is determined by the canvas plane distance at start.
    private float maxDistance;

    private GameObject brain;
    private ScaleToggler scaleScript;

    private Canvas canvasComp;
    private SpriteRenderer spriteComp;

    private Transform textZone;
    
    private float distanceFromBrain;

    // Use this for initialization
    void Start () {
        canvasComp = gameObject.GetComponent<Canvas>();
        maxDistance = canvasComp.planeDistance;
        minDistance = Camera.main.nearClipPlane + 0.15f;
        
        textZone = transform.Find("Text");
        spriteComp = textZone.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (spriteComp.sprite != null)
        {

            if (distanceFromBrain <= minDistance)
            {
                spriteComp.enabled = false;
            }
            else if (distanceFromBrain >= maxDistance)
            {
                canvasComp.planeDistance = maxDistance;

                spriteComp.enabled = true;
            }
            else
            {
                canvasComp.planeDistance = distanceFromBrain;

                spriteComp.enabled = true;
            }
        }*/
    }
}
