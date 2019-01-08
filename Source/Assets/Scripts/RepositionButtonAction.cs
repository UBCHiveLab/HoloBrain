// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionButtonAction : MonoBehaviour {

    private AudioSource audio;
    private string BRAIN_OBJ_NAME = "Brain";

    // Use this for initialization
    void Start () {
        //brain = GameObject.Find("Brain");
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnSelect()
    {
        audio.Play();
        GameObject.Find(BRAIN_OBJ_NAME).GetComponent<HologramPlacement>().ResetStage();
    }


}
