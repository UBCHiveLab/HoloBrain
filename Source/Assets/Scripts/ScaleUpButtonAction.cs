// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpButtonAction : MonoBehaviour {

    //private const string BRAIN_PARTS_NAME = "BrainParts";
    public GameObject scaleObject;
   
    // Use this for initialization
    void Start () {
       
    }

    public void OnSelect()
    {
        //GameObject.Find(BRAIN_PARTS_NAME).GetComponent<ScaleToggler>().ScaleUp();
        scaleObject.GetComponent<ScaleToggler>().ScaleUp();

    }
   
}
