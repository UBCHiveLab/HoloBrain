// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMRIRotateButtonAction : MonoBehaviour
{

    GameObject brain;


    // Use this for initialization
    void Start()
    {
        brain = GameObject.Find("fMRIBrains");
        Debug.Log("Rotate button brain variable is pointing to " + brain.name);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnSelect()
    {
        //do the action
        brain.GetComponent<RotateStructures>().OnSelect();
    }

}