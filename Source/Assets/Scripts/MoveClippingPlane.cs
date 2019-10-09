// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClippingPlane : MonoBehaviour {

    //Number of frames to wait before adjusting tranform
    public Material brainMaterial;
    private Vector3 animationVerticalStartPosition;
    private Vector3 animationVerticalStartRotation;
    private Vector3 animationHorizontalStartPosition;
    private Vector3 animationHorizontalStartRotation;
    private Vector3 defaultPosition;
    private Vector3 defaultRotation;
    private bool isClipping = false;
    private GameObject BrainObject;
    private const string BRAIN = "Brain";
    private const float PLANE_ANIMATION_OFFSET = -.5f;
    private const float PLANE_DEFAULT_OFFSET = -30f;
    private bool isMovingX = false;
    private bool isMovingY = false;
    private float targetPosition;
    private float currentPosition;
    private float movementRate = 0.01f;

    public enum Orientation { horizontal, vertical};

    private Orientation planeOrientation = Orientation.vertical;

	// Use this for initialization
	void Start () {
        BrainObject = GameObject.Find(BRAIN);
    }

    void OnEnable()
    {
        //OnEnable is called before Start in Unit's event order. We need to try and get the brain object in OnEnable
        //because of plane repositioning in HologramPlacement when the brain is first loaded
        if (BrainObject != null)
        {
            setDefaultPosition();
        }
        else
        {
            try
            {
                BrainObject = GameObject.Find(BRAIN);
                setDefaultPosition();
            }
            catch(NullReferenceException e)
            {
                Debug.Log("MoveClippingPlane: Brain has not been loaded yet");
            }
        }
    }

    private void setDefaultPosition()
    {
        transform.localPosition = new Vector3(0 - PLANE_ANIMATION_OFFSET, 0, 0);
        transform.localEulerAngles = new Vector3(0, 0, 90);

        animationVerticalStartPosition = transform.localPosition;
        animationVerticalStartRotation = transform.localEulerAngles;

        animationHorizontalStartPosition = new Vector3(0, 0 - PLANE_ANIMATION_OFFSET, 0);
        animationHorizontalStartRotation = new Vector3(0, 0, 180);

        defaultPosition = new Vector3(0 - PLANE_DEFAULT_OFFSET, 0, 0);
        defaultRotation = new Vector3(0, 0, 90);
    }
	
	// Update is called once per frame
	void Update () {
        if (isClipping)
        {
            if(isMovingX)
            {
                moveTowardsNewXPosition();
            }
            else if(isMovingY)
            {
                moveTowardsNewYPosition();
            }
            //clipAtCurrentPosition();
        }
    }

    public void changePlaneXPosition(float xPosition)
    {
        Debug.Log("changing plane position on x: " + xPosition);
        transform.localEulerAngles = animationVerticalStartRotation;
        isMovingX = true;
        isMovingY = false;
        targetPosition = xPosition;
        currentPosition = animationVerticalStartPosition.x;
        transform.localPosition = animationVerticalStartPosition;
    }


    public void changePlaneYPosition(float yPosition)
    {
        Debug.Log("changing plane position on y: " + yPosition);
        transform.localEulerAngles = animationHorizontalStartRotation;
        isMovingY = true;
        isMovingX = false;
        targetPosition = yPosition;
        currentPosition = animationHorizontalStartPosition.y;
        transform.localPosition = animationHorizontalStartPosition;
    }

    public void moveTowardsNewXPosition()
    {
        if /*((currentPosition <= targetPosition) || */(Mathf.Abs(currentPosition - targetPosition) < movementRate)
        {
            isMovingX = false;
            transform.localPosition = new Vector3(targetPosition, animationVerticalStartPosition.y, animationVerticalStartPosition.z);
        }
        else
        {
            if(targetPosition > currentPosition)
            {
                currentPosition = currentPosition + movementRate;
            }
            else if(targetPosition < currentPosition){
                currentPosition = currentPosition - movementRate;
            }
            transform.localPosition = new Vector3(currentPosition, animationVerticalStartPosition.y, animationVerticalStartPosition.z);
        }
    }

    public void moveTowardsNewYPosition()
    {
        if /*((currentPosition <= targetPosition) || */(Mathf.Abs(currentPosition - targetPosition) < movementRate)
        {
            isMovingY = false;
            transform.localPosition = new Vector3(animationVerticalStartPosition.x, targetPosition, animationVerticalStartPosition.z);
        }
        else
        {
            if (targetPosition > currentPosition)
            {
                currentPosition = currentPosition + movementRate;
            }
            else if (targetPosition < currentPosition)
            {
                currentPosition = currentPosition - movementRate;
            }
            transform.localPosition = new Vector3(animationVerticalStartPosition.x, currentPosition, animationVerticalStartPosition.z);
        }
    }

    public void resetPlanePosition()
    {
        setPlaneOrientation(Orientation.vertical);
        transform.localPosition = defaultPosition;
        transform.localEulerAngles = defaultRotation;
    }

    public void TurnOffClipping()
    {
        resetPlanePosition();
        clipAtCurrentPosition();
        isClipping = false;
        isMovingX = false;
        isMovingY = false;

        gameObject.SetActive(false);
    }

    public void TurnOnClipping()
    {
        isClipping = true;
    }

    public void setPlaneOrientation(Orientation planeOrientation)
    {
        this.planeOrientation = planeOrientation;

        switch (planeOrientation)
        {
            case Orientation.horizontal:
                transform.localEulerAngles = animationHorizontalStartRotation;
                break;
            case Orientation.vertical:
                transform.localEulerAngles = animationVerticalStartRotation;
                break;
        }       
    }

    private void clipAtCurrentPosition()
    {
        Vector3 normal = transform.up;
        
        Shader.SetGlobalVector("_PlaneD", transform.position);
        Shader.SetGlobalVector("_PlaneN", normal);

        brainMaterial.SetVector("_PlaneD", transform.position);
        brainMaterial.SetVector("_PlaneN", normal);
    }
}
