// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGazeStateChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {

        EnableOrDisableFrame(false);

    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnStartGaze()
    {
        //visual change of the button on gaze over
        EnableOrDisableFrame(true);
    }

    void OnEndGaze()
    {
        //visual change of the button on gaze over
        EnableOrDisableFrame(false);
    }
    private void EnableOrDisableFrame(bool frameState)
    {
        if (transform.Find("white-border") != null)
            transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = frameState;
    }
}
