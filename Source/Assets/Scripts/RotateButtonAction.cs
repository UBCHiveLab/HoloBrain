// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButtonAction : MonoBehaviour {

    public GameObject brain;
 

    // Use this for initialization
    void Start () {
        if (brain == null)
        {
            brain = GameObject.Find("Brain");
        }
        Debug.Log("Rotate button brain variable is pointing to " + brain.name);

    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public void OnSelect()
    { 
            //do the action
            brain.GetComponent<RotateStructures>().OnSelect(); 
    }

}
