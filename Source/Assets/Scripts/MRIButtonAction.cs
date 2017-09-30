// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;

public class MRIButtonAction : MonoBehaviour {
    private GameObject MRICollection;
    private const string MRICollectionName = "MRICollection";

	// Use this for initialization
	void Start () {
        MRICollection = GameObject.Find(MRICollectionName);
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void OnSelect()
    {
        //if the button is enabled
        if (gameObject.GetComponent<ButtonCommands>().buttonIsEnabled)
        {
            //do the action
            MRICollection.GetComponent<MRIManager>().ProcessMRIButtonAction();
        }
    }

}
