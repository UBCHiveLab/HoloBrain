// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIPositionManager : MonoBehaviour {

    private bool gazeIsOnUI;
    private int gazeDelayCounter;

    private void OnLevelWasLoaded(int level)
    {
        float angle = PlayerPrefs.GetFloat("adjustmentAngle");
        transform.Rotate(Vector3.up, angle);
    }

    // Use this for initialization
    void Start () {
        gazeIsOnUI = true;
        gazeDelayCounter = 0;
    }

    // Update is called once per frame
    void Update () {
        transform.position = Camera.main.transform.position;
        if (gazeDelayCounter == 0)
        {
            if (!gazeIsOnUI)
            {
                transform.rotation = Quaternion.Lerp(
                    new Quaternion(0, Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w),
                    transform.rotation, 0.95f);
            }
        }
        else
        {
            gazeDelayCounter--;
        }
    }

    public void OnGazeEnteredUI()
    {
        gazeIsOnUI = true;
    }

    public void OnGazeExitUI()
    {
        gazeIsOnUI = false;
        gazeDelayCounter = 30;
    }

}
