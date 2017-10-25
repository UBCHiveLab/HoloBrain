// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component will show its game object when its parent is being gazed at for a certain amount of time.
/// </summary>
public class Tooltip : MonoBehaviour
{
    private static class Constants
    {
        public const string DefaultText = "Say \"Adjust\"";
        public const float ShowDelay = 1f;
        public const float HideDelay = 0.5f;
    }

    [Tooltip("Text displayed by the tooltip.")]
    public string Text = Constants.DefaultText;

    [Tooltip("Game object displaying the text label.")]
    public GameObject Label;

    [Tooltip("The text container.")]
    public Text TextCanvas;

    [Tooltip("The background of the tooltip.")]
    public GameObject Background;

    private HSGazeObserver parentsGazeObserver;
    private bool isInit;
    private float widthRatio;

    private void Start()
    {
        parentsGazeObserver = GetComponentInParent<HSGazeObserver>();
        parentsGazeObserver.FocusEntered += ShowIfFocusedWithDelay;
        parentsGazeObserver.FocusExited += HideIfNotFocusedWithDelay;

        ChangeVisibility(false);
    }

    private void Update()
    {
        var labelWidth = ((RectTransform)Label.transform).rect.width;

        // We have to do this in the update because the HorizontalLayoutGroup's width is not initialized yet when
        // the start is called.
        if (!isInit)
        {
            if (labelWidth == 0)
            {
                return;
            }

            // This takes a reference ratio to resize the background to fit the text. This ratio is relative 
            // to the initial tooltips background and label initial sizes.
            widthRatio = Background.transform.localScale.x / labelWidth;
            isInit = true;
        }

        TextCanvas.text = Text;

        // Adjusts the width of the background to fit the text length
        Background.transform.localScale = new Vector3(widthRatio * labelWidth, Background.transform.localScale.y, Background.transform.localScale.z);
    }

    private void OnDestroy()
    {
        parentsGazeObserver = GetComponentInParent<HSGazeObserver>();

        if (parentsGazeObserver != null)
        {
            parentsGazeObserver.FocusEntered -= ShowIfFocusedWithDelay;
            parentsGazeObserver.FocusExited -= HideIfNotFocusedWithDelay;
        }
    }

    /// <summary>
    /// Shows the tooltip after a delay of ShowDelay if the parent is focused.
    /// </summary>
    private void ShowIfFocusedWithDelay()
    {
        Invoke("ShowIfFocused", Constants.ShowDelay);
    }

    /// <summary>
    /// Hides the tooltip after a delay of HideDelay if the parent is not focused.
    /// </summary>
    private void HideIfNotFocusedWithDelay()
    {
        Invoke("HideIfNotFocused", Constants.HideDelay);
    }

    /// <summary>
    /// Shows the toolbar if the parent is focused.
    /// </summary>
    private void ShowIfFocused()
    {
        if (!parentsGazeObserver.IsGazed)
        {
            return;
        }

        ChangeVisibility(true);
    }

    /// <summary>
    /// Shows the toolbar if the parent is not focused.
    /// </summary>
    private void HideIfNotFocused()
    {
        if (parentsGazeObserver.IsGazed)
        {
            return;
        }

        ChangeVisibility(false);
    }

    /// <summary>
    /// Changes the visibility of the tooltip.
    /// </summary>
    private void ChangeVisibility(bool visibility)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = visibility;
        }

        foreach (var renderer in GetComponentsInChildren<Canvas>())
        {
            renderer.enabled = visibility;
        }
    }
}
