// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;

public class MRIInteractions : MonoBehaviour
{
    private const string CLIP_PLANE = "ClipPlane";
    private const string MRI_IMAGE_WITH_OUTLINE = "MRIImage";
    private const string MRI_IMAGE_NO_OUTLINE = "MRIImageBlank";
    private const string MRI_COLLECTION = "MRICollection";
    private const string MRI_RING = "Ring";

    private GameObject ring;
    private GameObject clipPlane;
    private GameObject ActiveMRIImage;
    private GameObject MRIImageWithOutline;
    private GameObject MRIImageWithNoOutline;
    private GameObject MRICollection;

    private Color ringDefaultColour;
    public Color ringAnimateColour;

    private bool isSectionSelected = false;

    public MoveClippingPlane.Orientation sliceOrientation;

    private CustomMessages customMessages;
    private MRIManager mriManager;
    private string mode;

    public GameObject wallIcon;
    
    

    void Awake()
    {
        clipPlane = GameObject.Find(CLIP_PLANE);
        MRICollection = GameObject.Find(MRI_COLLECTION);

    }
    // Use this for initialization
    void Start()
    {
        mode = PlayerPrefs.GetString("mode");
        customMessages = CustomMessages.Instance;
        mriManager = MRIManager.Instance;
        ring = transform.Find(MRI_RING).gameObject;
        MRIImageWithOutline = transform.Find(MRI_IMAGE_WITH_OUTLINE).gameObject;
        MRIImageWithNoOutline = transform.Find(MRI_IMAGE_NO_OUTLINE).gameObject;
        ActiveMRIImage = MRIImageWithOutline;
        ringDefaultColour = ring.GetComponent<Renderer>().material.color;
        InitializeMRI();

        if ((ring == null) || (clipPlane == null) || (MRIImageWithOutline == null) || (MRIImageWithNoOutline == null))
        {
            Debug.Log("MRI Interactions: MRI functionality won't load because objects haven't been defined in scene. Must have " + MRI_RING + "," + MRI_IMAGE_NO_OUTLINE + ", " + MRI_IMAGE_WITH_OUTLINE );
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnStartGaze()
    {
        if (!isSectionSelected)
        {
            SetLineAnimation(true);
        }
    }

    void OnEndGaze()
    {
        SetLineAnimation(false);
    }

    void OnSelect()
    {
        MRIImageWithOutline.SetActive(!MRIImageWithOutline.activeSelf);
        /*
        if (!isSectionSelected && mode != "student")
        {
            SelectMRI();
        }*/
    }

    public void SelectMRI()
    {
        if (wallIcon != null)
        {
            MRIIconManager.Instance.DeselectAll();
            wallIcon.GetComponent<ButtonSwapHighlightFeedback>().ToggleButtonImage();
        }
        DisplayMRIImage();
        mriManager.DisplaySingleMRI(gameObject);
        if (customMessages != null)
        {
            customMessages.SendToggleMRIMessage(this.name, isSectionSelected);
        }
    }

    public void ChangeMRIImage(bool isOutlinedMRIImage)
    {
        if(isOutlinedMRIImage)
        {
            MRIImageWithNoOutline.SetActive(false);
            ActiveMRIImage = MRIImageWithOutline;
        }
        else
        {
            MRIImageWithOutline.SetActive(false);
            ActiveMRIImage = MRIImageWithNoOutline;
        }

        if(isSectionSelected)
        {
            ActiveMRIImage.SetActive(true);
        }
        else
        {
            ActiveMRIImage.SetActive(false);
        }
    }

    public void DisplayMRIImage()
    {
        ActiveMRIImage.SetActive(true);
        ring.SetActive(false);

        isSectionSelected = true;
        //SetLineAnimation(false);
        //ClipPlaneAtMRIPosition();
    }


    public void HideMRIImage()
    {
        ActiveMRIImage.SetActive(false);
        ring.SetActive(true);

        isSectionSelected = false;
        clipPlane.GetComponent<MoveClippingPlane>().TurnOffClipping();
    }

    private void InitializeMRI()
    {
        ActiveMRIImage.SetActive(false);
        MRIImageWithNoOutline.SetActive(false);
        MRIImageWithOutline.SetActive(true);
        ring.SetActive(true);
    }

    public void ClipPlaneAtMRIPosition()
    {
        clipPlane.SetActive(true);
        clipPlane.GetComponent<MoveClippingPlane>().setPlaneOrientation(sliceOrientation);
        clipPlane.GetComponent<MoveClippingPlane>().TurnOnClipping();
        switch (sliceOrientation)
        {
            case MoveClippingPlane.Orientation.horizontal:
                clipPlane.GetComponent<MoveClippingPlane>().changePlaneYPosition(transform.localPosition.y);
                break;
            case MoveClippingPlane.Orientation.vertical:
                clipPlane.GetComponent<MoveClippingPlane>().changePlaneXPosition(transform.localPosition.x);
                break;
        }
    }

    public void SetLineAnimation(bool isAnimating)
    {
        //ring.GetComponent<Animator>().enabled = isAnimating;
        if(isAnimating)
        {
            ring.GetComponent<Renderer>().material.color = ringAnimateColour;
        }
        else
        {
            ring.GetComponent<Renderer>().material.color = ringDefaultColour;
        }
    }
}
