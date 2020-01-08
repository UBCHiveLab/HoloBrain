// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine;

/// <summary>
/// Lets you know when the gaze is on the game object or one of its children.
/// </summary>
public class HSGazeObserver : MonoBehaviour
{
    public event Action FocusEntered;
    public event Action FocusExited;
    public Sprite HoverSprite;

    /// <summary>
    /// Indicates if the object is being gazed at.
    /// </summary>
    public bool IsGazed { get; private set; }

    private void Start()
    {
        // Change this to somehow use HTGazeManager
        HTGazeManager.Instance.FocusedObjectChanged += GazeManager_FocusedObjectChanged;
    }

    private void OnDestroy()
    {
        if (HTGazeManager.Instance != null)
        {
            HTGazeManager.Instance.FocusedObjectChanged -= GazeManager_FocusedObjectChanged;
        }
    }

    /// <summary>
    /// Triggers the FocusEntered and FocusExited events.
    /// </summary>
    /// <param name="previousObject">The object that was previously being gazed at.</param>
    /// <param name="newObject">The object that is currently being gazed at.</param>
    private void GazeManager_FocusedObjectChanged(GameObject previousObject, GameObject newObject)
    {
        if (newObject == null)
        {
            if (IsGazed)
            {
                IsGazed = false;
                FocusExited.RaiseEvent();
            }
        }
        else
        {
            if (newObject.transform.IsChildOf(gameObject.transform))
            {
                if (!IsGazed)
                {
                    IsGazed = true;
                    gameObject.GetComponent<SpriteRenderer>().sprite = HoverSprite;
                    FocusEntered.RaiseEvent();
                }
            }
            else if (IsGazed)
            {
                IsGazed = false;
                FocusExited.RaiseEvent();
            }
        }
    }
}
