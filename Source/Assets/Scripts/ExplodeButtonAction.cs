// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeButtonAction : MonoBehaviour
{
    private const string BRAIN_PARTS_NAME = "Brain";

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //this is bad coupling. there are some prereqs to explode command but switchroomui will switch the buttons anyways
    public void OnSelect()
    {
        GameObject.Find(BRAIN_PARTS_NAME).GetComponent<ExplodingCommands>().ToggleExplode();
    }
}
