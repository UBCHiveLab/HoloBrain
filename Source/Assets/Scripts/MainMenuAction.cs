// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAction : MonoBehaviour {

    private const string START_SCENE_NAME = "StartAppScene";

    void OnSelect()
    {
        SceneManager.LoadScene(START_SCENE_NAME);
    }
}
