// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour {

    private AudioSource soundFX;

    void Start () {
        soundFX = gameObject.GetComponent<AudioSource>();
    }
	
    void OnSelect()
    {
        if (soundFX != null)
        {
            soundFX.Play();
        }
    }
}
