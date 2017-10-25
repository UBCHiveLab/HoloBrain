// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeMarkerCommands : MonoBehaviour {

    private bool isPointed = false;

    public void OnSelect()
    {
        if (!GetComponentInParent<IsolateStructures>().AStructureIsMovingOrResizing())
        {
            GetComponentInParent<GazeMarkerManager>().TryToMoveGazeMarker();
        }
    }

    public void RemoveMarkerFromStructure()
    {
        if (isPointed)
        {
            GetComponentInParent<GazeMarkerManager>().TryToRemoveGazeMarker();
        }
    }

    public void MarkerIsPointing(bool pointing)
    {
        isPointed = pointing;
    }
}
