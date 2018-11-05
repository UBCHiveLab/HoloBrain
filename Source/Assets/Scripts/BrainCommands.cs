// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing.Tests;
using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainCommands : MonoBehaviour {

    CustomMessages customMessages;
    Dictionary<string, Transform> modelNameToTransform;

    void Start()
    {
        //Links the gameObject for the label to the current object 
        // to be able to easily show and hide it at runtime
        //label = transform.GetChild(0).FindChild("Canvas").gameObject;

        while(customMessages == null)
        {
            Debug.Log("CustomMessages was null");
            customMessages = CustomMessages.Instance;
        }
        customMessages.MessageHandlers[CustomMessages.TestMessageID.ToggleHighLight] = this.ToggleMessageReceived;

        modelNameToTransform = new Dictionary<string, Transform>();
        foreach (Transform child in this.transform)
        {
            try
            {
                string name = child.name;
                Debug.Log("Adding child to dictionary: " + name);
                modelNameToTransform.Add(name, child);
            }
            catch (System.ArgumentException e)
            {
                Debug.Log("The child could not be added because the string was already used as a key for the dictionary");
            }
        }
    }

    private void Update()
    {
        //if the label is active (visible)
        //if (label.activeSelf)
        //{
        //make the label face the camera
        //    label.transform.rotation = Quaternion.LookRotation(label.transform.position - Camera.main.transform.position, Vector3.up);
        //}

        //counter++;
        //if(counter % COUNTER_FREQUENCY == 0)
        //{
        //    //CustomMessages.SendTestMessage(counter/COUNTER_FREQUENCY);
        //    customMessages.SendToggleHighlightMessage( (counter / COUNTER_FREQUENCY) % NUMBER_OF_CHILDREN);
        //}
    }

    void TestMessageReceived(NetworkInMessage msg)
    {
        Debug.Log("Test message was received");
    }

    void ToggleMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();

        string s = msg.ReadString();
        Debug.Log("Toggle message was received: '"+s+"'");

        modelNameToTransform[s].GetComponent<HighlightAndLabelCommands>().ToggleLockedHighlight();
    }

    void OnSelect()
    {
        ////locks or unlocks the selection
        //isLocked = !isLocked;

        ////if the selection is locked, increase the glow. If it's unlocked, return the glow to normal
        //if (isLocked)
        //{
        //    gameObject.GetComponent<Renderer>().material.SetFloat("_MKGlowPower", glowPower + increasedGlow);
        //}
        //else
        //{
        //    gameObject.GetComponent<Renderer>().material.SetFloat("_MKGlowPower", glowPower);
        //}
    }
 
    void OnStartGaze()
    {
        ////if the selection isn't locked, make the object glow and activate the label
        //if (!isLocked)
        //{
        //    gameObject.GetComponent<Renderer>().material.SetFloat("_MKGlowPower", glowPower);
        //    //label.SetActive(true);
        //}
    }
    void OnEndGaze()
    {
        ////if the selection isn't locked, remove the object's glow and deactivate the label
        //if (!isLocked)
        //{
        //    gameObject.GetComponent<Renderer>().material.SetFloat("_MKGlowPower", 0.0f);
        //    //label.SetActive(false);
        //}
    }

}
