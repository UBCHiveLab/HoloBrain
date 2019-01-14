// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CortexFader : MonoBehaviour {

    private const float DefaultBrainRadius = 0.1f;
    private float brainRadius;
    private float distanceFromBrain;
    
    private float minDistance;
    private float maxDistance;

    public float fadeStartDistance;
    public float fadeDistance;

    private GameObject brain;
    private ScaleToggler scaleScript;

    private MeshRenderer cortexRenderComp;
    private Color cortexOriginalColor;

    // Use this for initialization
    void Start()
    {
        maxDistance = fadeStartDistance;
        minDistance = maxDistance - fadeDistance;

        brain = GameObject.Find("Brain");
        scaleScript = brain.GetComponent<ScaleToggler>();

        cortexRenderComp = brain.transform.Find("Cortex").GetComponent<MeshRenderer>();
        cortexOriginalColor = cortexRenderComp.material.color;

        UpdateBrainRadius();
    }

    // Update is called once per frame
    void Update()
    {
        if (scaleScript.zoomChanging)
        {
            UpdateBrainRadius();
        }

        distanceFromBrain = Vector3.Distance(brain.transform.position, Camera.main.transform.position) - brainRadius;

        if (distanceFromBrain <= minDistance)
        {
            if(cortexRenderComp.enabled)
                cortexRenderComp.enabled = false;
        }
        else if (distanceFromBrain >= maxDistance)
        {
            if (!cortexRenderComp.enabled)
                cortexRenderComp.enabled = true;

            cortexRenderComp.material.color = cortexOriginalColor;
        }
        else
        {
            if (!cortexRenderComp.enabled)
                cortexRenderComp.enabled = true;

            cortexRenderComp.material.color = cortexOriginalColor * ((distanceFromBrain - minDistance) / fadeDistance);
        }
        
    }

    private void UpdateBrainRadius()
    {
        brainRadius = DefaultBrainRadius * brain.transform.localScale.x;
    }
}
