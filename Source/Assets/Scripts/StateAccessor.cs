// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAccessor : Singleton<StateAccessor> {

    private const string MRICollection = "MRICollection";
    private const string BRAIN_PARTS = "BrainParts";
    private ResetState resetState;
    public enum Mode { Default, Isolated, MRI };

	private GameObject brainParts;
	private Mode currentMode;

	// Use this for initialization
	void Start () {
		brainParts = this.gameObject;
        currentMode = Mode.Default;
        resetState = brainParts.GetComponent<ResetState>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public bool ChangeMode(Mode newMode)
    {
        if((currentMode == Mode.Default) || (newMode == Mode.Default))
        {
            if (!this.GetComponent<IsolateStructures>().AtLeastOneStructureIsMovingOrResizing)
            {
                currentMode = newMode;
                return true;
            }
        }

        if( ((currentMode == Mode.MRI) && (newMode == Mode.Isolated)) || ((currentMode == Mode.Isolated) && (newMode == Mode.MRI)) )
        {
            if (!this.GetComponent<IsolateStructures>().AtLeastOneStructureIsMovingOrResizing)
            {
                resetState.ResetEverything();
                currentMode = newMode;
                return true;
            }
        }

        return false;
    }

    public bool AbleToTakeAnInteraction()
    {
        return !(CurrentlyIsolatedOrIsolating() || (CurrentlyInMRIMode()));
    }

    public bool CurrentlyInMRIMode()
    {
        return GameObject.Find(MRICollection).GetComponent<MRIManager>().isCurrentlyInMRIMode();
    }

    public bool CurrentlyIsolatedOrIsolating()
    {
        return this.GetComponent<IsolateStructures>().CurrentlyInIsolationModeOrIsolating();
    }

    public bool CurrentlyInStudentMode()
    {
        return GameObject.Find("StatusUI").GetComponent<StudentModeCommands>().CurrentlyInStudentMode();
    }

   public Mode GetCurrentMode()
    {
        return currentMode;
    }
}