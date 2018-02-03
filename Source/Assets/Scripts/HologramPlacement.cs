// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;
using HoloToolkit.Unity.SpatialMapping;

public class HologramPlacement : Singleton<HologramPlacement>
{
    /// <summary>
    /// Tracks if we have been sent a transform for the model.
    /// The model is rendered relative to the actual anchor.
    /// </summary>
    public bool GotTransform { get; private set; }

    /// <summary>
    /// When the experience starts, we disable all of the rendering of the model.
    /// </summary>
    List<MeshRenderer> disabledRenderers = new List<MeshRenderer>();
    private const string MRI_COLLECTION = "MRICollection";
    private MRIManager mriManager;
    private string mode;
    private StudentModeCommands uiVisibilityCommands;

    void Start()
    {
        mriManager = this.gameObject.transform.Find(MRI_COLLECTION).gameObject.GetComponent<MRIManager>();
        mode = PlayerPrefs.GetString("mode");
        uiVisibilityCommands = GameObject.Find("StatusUI").GetComponent<StudentModeCommands>();

        if (mode != "solo")
        {

            // When we first start, we need to disable the model to avoid it obstructing the user picking a hat.
            DisableModel();
            // We care about getting updates for the model transform.
            CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.StageTransform] = this.OnStageTransfrom;

            // And when a new user join we will send the model transform we have.
            SharingSessionTracker.Instance.SessionJoined += Instance_SessionJoined;

            // And if the users want to reset the stage transform.
            CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.ResetStage] = this.OnResetStage;
        }
        else
        {
            HTGestureManager.Instance.OverrideFocusedObject = this.gameObject;
        }
    }


    /// <summary>
    /// Resets the stage transform, so users can place the target again.
    /// </summary>
    public void ResetStage()
    {
        GotTransform = false;
        SpatialMappingManager.Instance.DrawVisualMeshes = true;
        HideUI();

        if (mode != "solo") { 
            AppStateManager.Instance.ResetStage();
            // Other devices in the experience need to know about this as well.
            CustomMessages.Instance.SendResetStage();
        }
        else
        {
            HTGestureManager.Instance.OverrideFocusedObject = gameObject;

        }
        mriManager.UpdateClippingForRepositioning(GotTransform);


    }

    /// <summary>
    /// When a new user joins we want to send them the relative transform for the model if we have it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_SessionJoined(object sender, SharingSessionTracker.SessionJoinedEventArgs e)
    {
        if (GotTransform)
        {
            CustomMessages.Instance.SendStageTransform(transform.localPosition, transform.localRotation);
            mriManager.UpdateClippingForRepositioning(GotTransform);
        }
        else
        {
            Debug.Log("HologramPlacement: Transform has not been stored ");
        }
    }

    /// <summary>
    /// Turns off all renderers for the model.
    /// </summary>
    void DisableModel()
    {
        Debug.Log("HologramPlacement: Disabling the model");
        foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.enabled)
            {
                renderer.enabled = false;
                disabledRenderers.Add(renderer);
            }
        }

        foreach (MeshCollider collider in gameObject.GetComponentsInChildren<MeshCollider>())
        {
            collider.enabled = false;
        }
    }

    /// <summary>
    /// Turns on all renderers that were disabled.
    /// </summary>
    void EnableModel()
    {
        Debug.Log("HologramPlacement: Enabling the model");
        foreach (MeshRenderer renderer in disabledRenderers)
        {
            renderer.enabled = true;
        }

        foreach (MeshCollider collider in gameObject.GetComponentsInChildren<MeshCollider>())
        {
            collider.enabled = true;
        }

        disabledRenderers.Clear();
    }
    

    void Update()
    {

        if (mode != "solo")
        {
            if (disabledRenderers.Count > 0)
            {
                if (ImportExportAnchorManager.Instance.AnchorEstablished)
                {
                    // After which we want to start rendering.
                    EnableModel();
                    mriManager.UpdateClippingForRepositioning(GotTransform);
                    ShowUI();
                }
            }
            else if (!GotTransform)
            {
                transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
            }
        }
        else
        {
            if (!GotTransform)
            {
                transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
            }
        }
    }

    Vector3 ProposeTransformPosition()
    {
        Vector3 retval;

        if (mode != "solo")
        {
            // We need to know how many users are in the experience with good transforms.
            Vector3 cumulatedPosition = Camera.main.transform.position;
            int playerCount = 1;

            foreach (RemotePlayerManager.RemoteHeadInfo remoteHead in RemotePlayerManager.Instance.remoteHeadInfos)
            {
                if (remoteHead.Anchored && remoteHead.Active)
                {
                    playerCount++;
                    cumulatedPosition += remoteHead.HeadObject.transform.position;
                }
            }
        }

        //  just put the model 2m in front of the user.
        retval = Camera.main.transform.position + Camera.main.transform.forward * 2;

        //clipPlane.GetComponent<MoveClippingPlane>().updateClippingForRepositioning();

        return retval;
    }

    public void OnSelect()
    {
        if (mode != "student")
        {
            // Note that we have a transform.
            GotTransform = true;
            mriManager.UpdateClippingForRepositioning(GotTransform);
            ShowUI();

            SpatialMappingManager.Instance.DrawVisualMeshes = false;

            if (mode != "solo")
            {
                // And send it to our friends.
                CustomMessages.Instance.SendStageTransform(transform.localPosition, transform.localRotation);
            }
            else
            {
                HTGestureManager.Instance.OverrideFocusedObject = null;
                GameObject controlsUI = GameObject.Find("ControlsUI");
                controlsUI.SetActive(true);
            }
        }
    }

    /// <summary>
    /// When a remote system has a transform for us, we'll get it here.
    /// </summary>
    /// <param name="msg"></param>
    void OnStageTransfrom(NetworkInMessage msg)
    {
        // We read the user ID but we don't use it here.
        msg.ReadInt64(); 
        SpatialMappingManager.Instance.DrawVisualMeshes = false;
        transform.localPosition = CustomMessages.Instance.ReadVector3(msg);
        transform.localRotation = CustomMessages.Instance.ReadQuaternion(msg);

        GotTransform = true;
        mriManager.UpdateClippingForRepositioning(GotTransform);
        if (ImportExportAnchorManager.Instance.AnchorEstablished)
        {
            ShowUI();
        }
    }

    /// <summary>
    /// When a remote system has a transform for us, we'll get it here.
    /// </summary>
    void OnResetStage(NetworkInMessage msg)
    {
        GotTransform = false;
        mriManager.UpdateClippingForRepositioning(GotTransform);
        HideUI();

        AppStateManager.Instance.ResetStage();
    }

    private void HideUI()
    {
        if (uiVisibilityCommands != null)
        {
            uiVisibilityCommands.SetUIVisibilityUI(false);
        }
    }

    private void ShowUI()
    {
        if (uiVisibilityCommands != null)
        {
            uiVisibilityCommands.ToggleStatusUI(GotTransform);
            if (mode != "student")
            {
                uiVisibilityCommands.ToggleControlsUI(GotTransform);
            }
        }
    }

}