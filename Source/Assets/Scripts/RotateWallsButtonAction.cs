// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWallsButtonAction : MonoBehaviour {

    GameObject walls;
    private const int ROTATION_SPEED = 20;
    private AudioSource audio;
    public bool isRotating { get; private set; }

    // Use this for initialization
    void Start () {
        walls = GameObject.Find("MRIWalls");
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (isRotating)
        {
            RotateForOneFrame();
        }

    }

    private void RotateForOneFrame()
    {
        walls.transform.Rotate(new Vector3(0, Time.deltaTime * ROTATION_SPEED, 0));
    }

    public void OnSelect()
    {
        audio.Play();
        ToggleRotate();     
    }

    private void ToggleRotate()
    {
        isRotating = !isRotating;
    }

}
