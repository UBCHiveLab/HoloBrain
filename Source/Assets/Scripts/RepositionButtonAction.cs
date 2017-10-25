// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionButtonAction : MonoBehaviour {

    // Use this for initialization
    void Start () {
        //brain = GameObject.Find("Brain");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnSelect()
    {
        GameObject.Find("Brain").GetComponent<HologramPlacement>().ResetStage();
    }


}
