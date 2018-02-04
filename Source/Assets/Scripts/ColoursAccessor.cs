// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class ColoursAccessor : MonoBehaviour {

    private GameObject brainParts;
    private const string BRAIN_PARTS = "BrainParts";
	// Use this for initialization
	void Start () {
        brainParts = GameObject.Find(BRAIN_PARTS);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   /* public void setBrainToGazeColour()
    {
        for (int i = 0; i < brainParts.transform.childCount; i++)
        {
            try
            {
                brainParts.transform.GetChild(i).GetComponent<HighlightAndLabelCommands>().setPartToGazeColour();

            }
            catch(NullReferenceException e)
            {
                Debug.Log("ColoursAccessor: This brain part does not have a HighlightAndLabelCommands script attached to it");
            }
        }
    }*/

    public void ToggledLockedHighlightOnBrain()
    {
        for (int i = 0; i < brainParts.transform.childCount; i++)
        {
            try
            {
                brainParts.transform.GetChild(i).GetComponent<HighlightAndLabelCommands>().ToggleLockedHighlight();
            }
            catch (NullReferenceException e)
            {
                Debug.Log("ColoursAccessor: This brain part does not have a HighlightAndLabelCommands script attached to it");
            }
        }
    }

    public void TurnOnHighlightOnBrain()
    {
        for (int i = 0; i < brainParts.transform.childCount; i++)
        {
            try
            {
                brainParts.transform.GetChild(i).GetComponent<HighlightAndLabelCommands>().TurnOnLockedHighlight();
            }
            catch (NullReferenceException e)
            {
                Debug.Log("ColoursAccessor: This brain part does not have a HighlightAndLabelCommands script attached to it");
            }
        }
    }

    /* public void setBrainToDefaultColour()
     {
         for (int i = 0; i < brainParts.transform.childCount; i++)
         {
             try
             {
                 brainParts.transform.GetChild(i).GetComponent<HighlightAndLabelCommands>().setPartToDefaultColour();

             }
             catch (NullReferenceException e)
             {
                 Debug.Log("ColoursAccessor: This brain part does not have a HighlightAndLabelCommands script attached to it");
             }
         }
     }*/
}
