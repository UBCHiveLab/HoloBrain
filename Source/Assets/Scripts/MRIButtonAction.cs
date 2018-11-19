// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;

public class MRIButtonAction : MonoBehaviour {
    private GameObject mriCollection_1, mriCollection_2;
    private const string MRI_COLLECTION_NAME = "MRICollection";
	private const string BRAIN_1 = "Brain";
	private const string BRAIN_2 = "Brain2";
    private const string BRAIN_PARTS_NAME = "BrainParts";
    private const string BRAIN_PARTS_NAME_2 = "BrainParts2";
    private AudioSource soundFx;

    private const string BRAIN_SELECTION_CONTROLLER = "selectBrainController";

    GameObject brain_1, brain_2, brainparts_1, brainparts_2;
    GameObject selectBrainControlGameObject;

	private string __selectedBrain;

	private GameObject SelectedMRICollection {
		get {
			__selectedBrain = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
			return (__selectedBrain == BRAIN_1) ? (mriCollection_1) : (mriCollection_2);
		}
	}

	// Use this for initialization
	void Start () {
        brain_1 = GameObject.Find(BRAIN_1);
        brain_2 = GameObject.Find(BRAIN_2);
        brainparts_1 = GameObject.Find(BRAIN_PARTS_NAME);
        brainparts_2 = GameObject.Find(BRAIN_PARTS_NAME_2);
        soundFx = GetComponent<AudioSource>();

        mriCollection_1 = brain_1.transform.Find(MRI_COLLECTION_NAME).gameObject;
        if (brain_2 != null)
            mriCollection_2 = brain_2.transform.Find(MRI_COLLECTION_NAME).gameObject;

        selectBrainControlGameObject = GameObject.FindWithTag(BRAIN_SELECTION_CONTROLLER);
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void OnSelect()
    {
        brainparts_1.GetComponent<ResetState>().ResetEverything();
        brainparts_2.GetComponent<ResetState>().ResetEverything();
        //if the button is enabled
        if (gameObject.GetComponent<ButtonCommands>().buttonIsEnabled)
        {
            //transform.Find("Ring").gameObject.SetActive(true);
            //do the action
            mriCollection_1.GetComponent<MRIManager>().ProcessMRIButtonAction();
            mriCollection_2.GetComponent<MRIManager>().ProcessMRIButtonAction();
            soundFx.Play();
        }
    }

}
