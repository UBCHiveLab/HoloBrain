// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightAndLabelCommands : MonoBehaviour {
    public bool isLocked { get; private set; }
    public bool hasPin { get; private set; }
    private bool isGazedAt = false;

    private CustomMessages customMessages;
    private AudioSource soundFX;

   // public GameObject pin;
    public GameObject labelZone;
    public HighlightAndLabelCommands otherHalf;
    public Sprite labelSprite;

    // Colour of the part that this script instance is attached to. 
    public Color defaultColour;
    public Color gazeColour;
    public Color highlightedColour;

    string mode;


    private void Start()
    {
        mode = PlayerPrefs.GetString("mode");
        customMessages = CustomMessages.Instance;
        soundFX = gameObject.GetComponent<AudioSource>();
        //gameObject.GetComponent<Renderer>().material.color = defaultColour;

        isLocked = false;
        hasPin = false;

      /*  if (!transform.name.Contains("(Clone)")) {
            pin.GetComponent<PinPositionManager>().SetBrainSectionTransform(transform);
            pin.SetActive(false);
        }*/
    }


    void OnSelect()
    {
        if (!StateAccessor.Instance.CurrentlyInStudentMode() && StateAccessor.Instance.AbleToTakeAnInteraction())
        {
            if (customMessages != null)
            {
                CustomMessages.Instance.SendToggleHighlightMessage(this.name, isLocked);
            }
            ToggleLockedHighlight();
            //blah
        }
    }
 
    void OnStartGaze()
    {
        if (StateAccessor.Instance.AbleToTakeAnInteraction() && mode != "student")
        {
            isGazedAt = true;
            HighlightObject();
        }
    }

    void OnEndGaze()
    {
        isGazedAt = false;
        HighlightObject();
    }

    public void ToggleHighlightMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        //msg.ReadInt64();

        //string toggledStructure = msg.ReadString();
        //Debug.Log("ToggleHighlight message received for " + this.name + " from " + toggledStructure);

        // The message sends us the old toggle states
        //if (toggledStructure.Equals(this.name))
        //{
            Debug.Log("Toggling highlight on " + this.name);
            isLocked = msg.ReadByte() == 1;
            ToggleLockedHighlight();
        //}
    }

    public void TurnOffLockedHighlight()
    {
        if (isLocked)
        {
            ToggleLockedHighlight();
        }
    }

    public void TurnOnLockedHighlight()
    {
        if (!isLocked)
        {
            ToggleLockedHighlight();
        }
    }

    public void ToggleLockedHighlight(bool allBrainParts = false)
    {
        isLocked = !isLocked;

        if (soundFX != null && !allBrainParts)
        {
            soundFX.Play();
        }
        
      /*  if (!(StateAccessor.Instance.CurrentlyIsolatedOrIsolating() || StateAccessor.Instance.CurrentlyInMRIMode()))
        {
            UpdatePins();
        }*/

        //if the selection is locked, increase the glow. If it's unlocked, return the glow to normal
        if (isLocked)
        {
            gameObject.GetComponent<Renderer>().material.color = highlightedColour;
        }
        else
        {
            HighlightObject();
        }
    }

    private void HighlightObject()
    {
        UpdateLabel();

        if (!isLocked)
        {
            if (isGazedAt)
            {
                // Make the object glow
                gameObject.GetComponent<Renderer>().material.color = gazeColour;
            }
            else
            {
                // remove the object's glow
                gameObject.GetComponent<Renderer>().material.color = defaultColour;
            }
        }
    }

    private void UpdateLabel()
    {
        if (labelZone.activeSelf)
        {
            if (isGazedAt && !isLocked)
            {
                labelZone.GetComponentInParent<CameraCanvasStabilizer>().IsBeingRendered = true;
                labelZone.GetComponent<SpriteRenderer>().sprite = labelSprite;
            }
            else
            {
                labelZone.GetComponentInParent<CameraCanvasStabilizer>().IsBeingRendered = false;
                labelZone.GetComponent<SpriteRenderer>().sprite = null;
            }
        }
    }

    private void UpdatePins()
    {
        if (otherHalf != null)
        {
            //if the other has a pin, do nothing
            if (otherHalf.hasPin)
            {
                return;
            }
            //else if this one is locked
            else
            {
                //update this pin (this section has a pin to add or remove)
                //TogglePin();
                //if this section isn't locked, but the other half is (we know it doesn't have a pin at this point)
                if (!isLocked && otherHalf.isLocked)
                {
                    //add the pin to the other
                    //otherHalf.TogglePin();
                }
            }
        }
        else
        {
           // TogglePin();
        }


    }

    /*public void TogglePin()
    {
        pin.SetActive(isLocked);
        pin.transform.parent.GetComponent<ActivePinsManager>().UpdateActivePin(isLocked, pin);
        hasPin = isLocked;
    }*/

    public void ResetHighlightAndLocking()
    {
        isLocked = false;
        isGazedAt = false;
        hasPin = false;
        gameObject.GetComponent<Renderer>().material.color = defaultColour;
       // pin.SetActive(false);
    }

}
