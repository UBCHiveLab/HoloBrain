// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubButtonManager : MonoBehaviour {

    public GameObject SubButton1;
    public GameObject SubButton2;
    bool GazeOn;
    int TransitionCounter;

	// Use this for initialization
	void Start () {
        TransitionCounter=10;
        GazeOn = false;
        SubButton1.GetComponent<SpriteRenderer>().enabled = false;
        SubButton1.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
        SubButton2.GetComponent<SpriteRenderer>().enabled = false;
        SubButton2.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
        SubButton1.GetComponent<BoxCollider>().enabled = false;
        SubButton2.GetComponent<BoxCollider>().enabled = false;
    }

   
    // Update is called once per frame
    void Update () {
        //disable the subbuttons when the gaze has been off for more than 10 frames
        if (TransitionCounter == 0)
        {
            if (!GazeOn)
            {
                //hide every thing and call a function to disable the script
                SubButton1.GetComponent<SpriteRenderer>().enabled = false;
                SubButton1.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
                SubButton1.GetComponent<BoxCollider>().enabled = false;
                SubButton2.GetComponent<SpriteRenderer>().enabled = false;
                SubButton2.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
                SubButton2.GetComponent<BoxCollider>().enabled = false;
                GameObject.Find("scale-icon").GetComponent<ScaleButtonAction>().DiasableMenuManager();

            }
        }
        else
        {
            TransitionCounter--;
        }
	}

    void OnStartGaze()
    {
        TransitionCounter = 10;

        GazeOn = true;
        SubButton1.GetComponent<SpriteRenderer>().enabled = true;
        SubButton1.GetComponent<BoxCollider>().enabled = true;
        SubButton2.GetComponent<SpriteRenderer>().enabled = true;
        SubButton2.GetComponent<BoxCollider>().enabled = true;
    }

    void OnEndGaze()
    {
        TransitionCounter = 10;
        GazeOn = false;
    }
    public void SetGazeOn(bool gaze)
    {
        GazeOn = gaze;
    }
   
}
