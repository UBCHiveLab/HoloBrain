// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIConfig;

public class ControlsUIManager : MonoBehaviour {

    private const float Z_DISTANCE_FROM_PIVOT = 1610.0f;
    private bool gazeIsOnUI;
    private bool isPinned;
    private int gazeDelayCounter;
    private AudioSource soundFX;

    // Use this for initialization
    void Start () {
        gazeIsOnUI = false;
        isPinned = true;
        gazeDelayCounter = 0;
        soundFX = gameObject.GetComponent<AudioSource>();

        Debug.Log("UI Controls manager start");

        ConfigReader cReader = ConfigReader.Instance;
        RegisterUIToConfig(cReader);
        cReader.ReadConfigFile();

        Debug.Log("Finished UI event");
    }

    // Update is called once per frame
    
    void Update()
    {
        if(!isPinned)
        {
            transform.position = Camera.main.transform.position;
            if (gazeDelayCounter == 0)
            {
                if (!gazeIsOnUI)
                {
                    transform.rotation = Quaternion.Lerp(
                        new Quaternion(0, Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w),
                        transform.rotation, 0.9f);
                }
            }
            else
            {
                gazeDelayCounter--;
            }
        }
    }
    

    public void TogglePinUI()
    {
        isPinned = !isPinned;
        soundFX.Play();
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

    public bool GetMenuPinState()
    {
        return isPinned;
    }

    protected void RegisterUIToConfig(ConfigReader reader)
    {
        ConfigListener[] listeners = GetComponentsInChildren<ConfigListener>();

        foreach (ConfigListener listener in listeners)
        {
            reader.ConfigReady += listener.OnConfigReady;
        }
    }
}
