// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : Singleton<ResetState> {
    private const string BRAIN_STRUCTURE_GROUPING = "Brain";
    private const string MRI_COLLECTION = "MRICollection";

    private GameObject brain;
    private GameObject MRICollection;
    private GameObject ControlsUI;
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

        brain = GameObject.Find(BRAIN_STRUCTURE_GROUPING);
        soundFX = brain.GetComponent<AudioSource>();
        MRICollection = GameObject.Find(MRI_COLLECTION);
        ControlsUI = GameObject.Find("ControlsUI");
    }

    void OnSelect()
    {
        //ResetEverything();
    }

    public void ResetStateMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();
        ResetStateAndInteractions();
    }

    public void ResetEverything()
    {
        if (customMessages != null)
        {
            customMessages.SendResetStateMessage();
        }
        ResetStateAndInteractions();
    }

    public void ResetStateAndInteractions()
    {
        if (ResetMode())
        {
            Debug.Log("Mode reset");
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
            //MRICollection.GetComponent<MRIManager>().ResetMRI();
            return true;
        }

        return false;
    }

    public void ResetInteractions()
    {
        brain.GetComponent<ScaleToggler>().ResetScale();
        brain.GetComponent<ExplodingCommands>().ResetExplode();

        //buttons for explode and collapse should reset too
        foreach(ExplodingCommands ec in ControlsUI.GetComponentsInChildren<ExplodingCommands>(true))
        {
            if (ec.name == "Expand")
            {
                ec.gameObject.SetActive(true);
            } else if(ec.name == "Collapse")
            {
                ec.gameObject.SetActive(false);
            }
        }

        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("Structure"))
        {
            try
            {
                foreach(HighlightAndLabelCommands comp in structure.GetComponentsInChildren<HighlightAndLabelCommands>())
                {
                    comp.ResetHighlightAndLocking();
                }
            }
            catch (System.NullReferenceException)
            {
                Debug.Log(structure.name + " has no HighlightAndLabelCommands script");
            }
        }
    }
}
