// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPositionManager : MonoBehaviour {

    private Vector3 relativePosition;
    private Vector3 parentRelativePosition;
    private Transform brainSection;
    private bool isInitialized = false;

	// Use this for initialization
	void Start () {
    }

    private void OnEnable()
    {
       AdjustPinPosition();
    }

    public void SetBrainSectionTransform(Transform section)
    {
        brainSection = section;

        relativePosition = (transform.position - brainSection.position) / brainSection.lossyScale.x;

        isInitialized = true;
    }

    public void AdjustPinPosition()
    {
        if (isInitialized)
        {
            parentRelativePosition = transform.parent.position - brainSection.parent.position;
            transform.localPosition = parentRelativePosition + ((brainSection.localPosition + relativePosition) * brainSection.lossyScale.x);
        }
    }
    
}
