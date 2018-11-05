// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Sharing.Tests;
using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingCommands : MonoBehaviour {
    private class BrainStructure
    {
        public Transform modelTransform;
        public Vector3 initialPosition;
        public Vector3 explodingDirection;
        public Vector3 furthestPosition;

        public BrainStructure(Transform structure, Vector3 centerOfBrainModel)
        {
            modelTransform = structure;
            initialPosition = modelTransform.localPosition;
            explodingDirection = structure.GetComponent<Renderer>().bounds.center - centerOfBrainModel;
            furthestPosition = initialPosition + explodingDirection * MAX_EXPLODE_DISTANCE_MULTIPLE;
        }

        // This method moves the structure away from or towards the center
        // If the structure reaches the initial or final position, the function returns true to indicate that exploding is done
        public bool MoveBrainStructure(ExplodingState currentState)
        {
            switch (currentState)
            {
                case ExplodingState.ExplodingOut:
                    modelTransform.localPosition += Time.deltaTime / EXPLODING_TRANSITION_TIME_IN_SECONDS * explodingDirection;
                    return (modelTransform.localPosition - furthestPosition).magnitude < Time.deltaTime / EXPLODING_TRANSITION_TIME_IN_SECONDS * explodingDirection.magnitude;
                case ExplodingState.ExplodingIn:
                    modelTransform.localPosition -= Time.deltaTime / EXPLODING_TRANSITION_TIME_IN_SECONDS * explodingDirection;
                    return (modelTransform.localPosition - initialPosition).magnitude < Time.deltaTime / EXPLODING_TRANSITION_TIME_IN_SECONDS * explodingDirection.magnitude;
                default:
                    return false;
            }
        }

        public void SnapToInitialPosition()
        {
            modelTransform.localPosition = initialPosition;
        }

        public void SnapToFurthestPosition()
        {
            modelTransform.localPosition = furthestPosition;
        }
    }

    private const string BRAIN_PARTS_GAMEOBJECT_NAME = "BrainParts";
    private const string CORTEX_GAMEOBJECT_NAME = "cortex_low";
    private const float EXPLODING_TRANSITION_TIME_IN_SECONDS = 1.5f;
    private const float MAX_EXPLODE_DISTANCE_MULTIPLE = 0.8f;
    private readonly List<string> STRUCTURES_THAT_DO_NOT_EXPLODE = new List<string> { "cortex_low", "ventricle", "thalamus" };

    public enum ExplodingState
    {
        Resting,
        ExplodingOut,
        ExplodingIn
    }

    private CustomMessages customMessages;
    private List<BrainStructure> explodingStructures;
    private GameObject cortex;
    public ExplodingState currentState { get; private set; }
    public ExplodingState lastState { get; private set; }

    private AudioSource soundFX;

    // Initialization
    void Start () {
        customMessages = CustomMessages.Instance;
        // Assign the ToggleExplodeMessageReceived() function to be a message handler for ToggleExplode messages
        // MessageHandlers is a dictionary with TestMessageID's as keys and MessageCalback's as values
        if (customMessages != null)
        {
            customMessages.MessageHandlers[CustomMessages.TestMessageID.ToggleExplode] = this.ToggleExplodeMessageReceived;
        }

        cortex = GameObject.Find(CORTEX_GAMEOBJECT_NAME);
        GameObject brain = GameObject.Find(BRAIN_PARTS_GAMEOBJECT_NAME);
        Vector3 centerOfBrainModel = brain.transform.Find(CORTEX_GAMEOBJECT_NAME).GetComponent<Renderer>().bounds.center;
       
        // Create and populate the list of brain structures
        // We add up all the initial positions of the structures to later determine the middle
        explodingStructures = new List<BrainStructure>();
        for (int i = 0;i<brain.transform.childCount;i++)
        {
            Transform currentStructure = brain.transform.GetChild(i);
            if (!STRUCTURES_THAT_DO_NOT_EXPLODE.Contains(currentStructure.name))
            {
                explodingStructures.Add(new BrainStructure(currentStructure, centerOfBrainModel));
            }
        }

        ResetExplode();
        soundFX = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        // If the brain is currently exploding in or out, the explode function gets called
        switch(currentState)
        {
            case ExplodingState.ExplodingOut:
            case ExplodingState.ExplodingIn:
                Explode();
                break;
            case ExplodingState.Resting:
                break;
        }
	}

    public void OnSelect()
    {
        if (this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            SendExplodingMessage();
            ToggleExplode();
        }
    }

    public void ToggleExplodeMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();

        // The message sends us the old explode state
        lastState = (ExplodingState)msg.ReadByte();
        ToggleExplode();
    }

    public void TryToExplode()
    {
        if (this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            // We can only explode if the last state was exploding in
            if (lastState == ExplodingState.ExplodingIn)
            {
                SendExplodingMessage();
                ToggleExplode();
            }
        }
    }

    public void TryToCollapse()
    {
        if (this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            // We can only collapse if the last state was exploding out
            if (lastState == ExplodingState.ExplodingOut)
            {
                SendExplodingMessage();
                ToggleExplode();
            }
        }
    }

    private void SendExplodingMessage()
    {
        if (customMessages != null)
        {
            customMessages.SendToggleExplodeMessage((byte)lastState);
        }
    }

    private void ToggleExplode()
    {
        soundFX.Play();
        // We toggle the last state and then send the message to all other HoloLenses
        switch (lastState)
        {
            case ExplodingState.ExplodingOut:
                lastState = ExplodingState.ExplodingIn;
                currentState = ExplodingState.ExplodingIn;
                break;
            case ExplodingState.ExplodingIn:
                lastState = ExplodingState.ExplodingOut;
                currentState = ExplodingState.ExplodingOut;
                cortex.SetActive(false); // The cortex should be deactivated before the brain explodes
                break;
        }
    }

    private void Explode()
    {
        bool doneExploding = false;

        // Each of the structures moves a bit away from or towards the center of the brain collection
        // MoveBrainStructure() returns true if the structure has reached its furthest point (when exploding out) or initial point (when exploding in)
        foreach (BrainStructure sub in explodingStructures)
        {
            doneExploding = sub.MoveBrainStructure(currentState);
        }

        if (doneExploding)
        {
            if (currentState == ExplodingState.ExplodingIn)
            {
                foreach (BrainStructure sub in explodingStructures)
                {
                    sub.SnapToInitialPosition();
                }
                cortex.SetActive(true);
            }
            else if (currentState == ExplodingState.ExplodingOut)
            {
                foreach (BrainStructure sub in explodingStructures)
                {
                    sub.SnapToFurthestPosition();
                }
            }
            currentState = ExplodingState.Resting;
        }
    }

    public void ResetExplode()
    {
        foreach(BrainStructure sub in explodingStructures)
        {
            sub.modelTransform.localPosition = sub.initialPosition;
        }

        cortex.SetActive(true);
        currentState = ExplodingState.Resting;
        lastState = ExplodingState.ExplodingIn;
    }
}
