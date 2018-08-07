// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDownButtonAction : MonoBehaviour {

    //private const string BRAIN_PARTS_NAME = "BrainParts";
    public GameObject scaleObject;

    // Use this for initialization
    void Start () {
		
	}
	
    public void OnSelect()
    {
        //GameObject.Find(BRAIN_PARTS_NAME).GetComponent<ScaleToggler>().ScaleDown();
        scaleObject.GetComponent<ScaleToggler>().ScaleDown();

    }
    void OnStartGaze()
    {
       transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("ScaleButtonsManager").GetComponent<SubButtonManager>().SetGazeOn(true);

    }
    void OnEndGaze()
    {
        transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("ScaleButtonsManager").GetComponent<SubButtonManager>().SetGazeOn(false);
    }

}
