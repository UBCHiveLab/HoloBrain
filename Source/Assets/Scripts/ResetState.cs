// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : Singleton<ResetState> {
    private const string MRI_COLLECTION = "MRICollection";
    private const string BRAIN_STRUCTURE_GROUPING = "BrainParts";

    private GameObject brain;
    private GameObject MRICollection;
    private CustomMessages customMessages;
	private AudioSource soundFX;
    private StateAccessor stateAccessor;
    // Use this for initialization
    void Start () {
        customMessages = CustomMessages.Instance;
        stateAccessor = StateAccessor.Instance;
        // Assign the ToggleExplodeMessageReceived() function to be a message handler for ToggleExplode messages
        // MessageHandlers is a dictionary with TestMessageID's as keys and MessageCalback's as values
        if (customMessages != null)
        {
            customMessages.MessageHandlers[CustomMessages.TestMessageID.ResetState] = this.ResetStateMessageReceived;
        }
        Debug.Log("++++++++++++++++++++++Hellow!");
        //brain =  GameObject.Find(BRAIN_STRUCTURE_GROUPING);
        brain = this.gameObject;
        Debug.Log(brain);
        soundFX = this.gameObject.GetComponent<AudioSource>();
        MRICollection = GameObject.Find("Brain").transform.Find(MRI_COLLECTION).gameObject;
    }

    void OnSelect()
    {
        ResetEverything();
    }

    public void ResetStateMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();
        ResetStateAndInteractions();
    }

    public void ResetEverything()
    {
        Debug.Log("*****************************Hello2");
        Debug.Log(brain);
        if (customMessages != null)
        {
            customMessages.SendResetStateMessage();
        }
        ResetStateAndInteractions();
        soundFX.Play();
    }

    public void ResetStateAndInteractions()
    {
        if (ResetMode())
        {
            ResetInteractions();
        }
        else
        {
            Debug.Log("Reset failed because the Default mode could not be entered.");
        }
    }

    public bool ResetMode()
    {
        if (stateAccessor.ChangeMode(StateAccessor.Mode.Default))
        {
            brain.GetComponent<IsolateStructures>().ResetIsolate();
            MRICollection.GetComponent<MRIManager>().ResetMRI();
            return true;
        }

        return false;
    }

    public void ResetInteractions()
    {
        brain.GetComponent<RotateStructures>().ResetRotation();
        brain.GetComponent<ScaleToggler>().ResetScale();
        brain.GetComponent<ExplodingCommands>().ResetExplode();

        for (int i = 0; i < brain.transform.childCount; i++)
        {
            try
            {
                brain.transform.GetChild(i).GetComponent<HighlightAndLabelCommands>().ResetHighlightAndLocking();
            }
            catch (System.NullReferenceException)
            {
                Debug.Log(brain.transform.GetChild(i).name + " has no HighlightAndLabelCommands script");
            }
        }
    }
}
