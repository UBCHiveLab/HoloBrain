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
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            foreach (Renderer renderer in structure.GetComponentsInChildren<Renderer>())
            {
                if (bounds.size.Equals(Vector3.zero))
                {
                    bounds = new Bounds(renderer.bounds.center, renderer.bounds.size);
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds.center);
                }
            }
            explodingDirection = modelTransform.InverseTransformPoint(bounds.center) - modelTransform.InverseTransformPoint(centerOfBrainModel);
            furthestPosition = initialPosition + (explodingDirection * MAX_EXPLODE_DISTANCE_MULTIPLE);
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

    private const string BRAIN_PARTS_GAMEOBJECT_NAME = "Brain";
    private const string CORTEX_GAMEOBJECT_NAME = "Cortex";
    private const float EXPLODING_TRANSITION_TIME_IN_SECONDS = 2.375f;
    private const float MAX_EXPLODE_DISTANCE_MULTIPLE = 0.8f;
    private readonly List<string> STRUCTURES_THAT_DO_NOT_EXPLODE = new List<string> { "Cortex", "Ventricles", "Arteries", "Sinuses"};

    public enum ExplodingState
    {
        Resting,
        ExplodingOut,
        ExplodingIn
    }

    private CustomMessages customMessages;
    private List<BrainStructure> explodingStructures;
    private GameObject cortex;
    private GameObject brain;
    private Vector3 centerOfBrainModel;
    public GameObject ExplodeButton;
    public GameObject CollapseButton;
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

        brain = GameObject.Find(BRAIN_PARTS_GAMEOBJECT_NAME);
        cortex = brain.transform.Find(CORTEX_GAMEOBJECT_NAME).gameObject;
        Renderer[] cortexRenderers = cortex.GetComponentsInChildren<Renderer>();
        Bounds brainBounds = new Bounds(cortex.GetComponentInChildren<Renderer>().bounds.center, Vector3.zero);
        foreach (Renderer renderer in cortexRenderers)
        {
            brainBounds.Encapsulate(renderer.bounds.center);
        }

        centerOfBrainModel = brainBounds.center;

        // Create and populate the list of brain structures
        // We add up all the initial positions of the structures to later determine the middle
        explodingStructures = new List<BrainStructure>();
        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("Structure"))
        {
            if (structure.transform.GetChild(0).name.Contains("Container"))
            {
                foreach (HighlightAndLabelCommands cur in structure.GetComponentsInChildren<HighlightAndLabelCommands>())
                {
                    if (!STRUCTURES_THAT_DO_NOT_EXPLODE.Contains(structure.name))
                    {
                        explodingStructures.Add(new BrainStructure(cur.gameObject.transform, centerOfBrainModel));
                    }
                }
            }
            else
            {
                if (!STRUCTURES_THAT_DO_NOT_EXPLODE.Contains(structure.name))
                {
                    explodingStructures.Add(new BrainStructure(structure.transform, centerOfBrainModel));
                }
            }
        }

        ResetExplode();
        soundFX = this.gameObject.GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        explodingStructures.Clear();
    }

    void OnEnable()
    {
        if (cortex == null)
        {
            return;
        }

        Renderer[] cortexRenderers = cortex.GetComponentsInChildren<Renderer>();
        Bounds brainBounds = new Bounds(cortex.GetComponentInChildren<Renderer>().bounds.center, Vector3.zero);
        foreach (Renderer renderer in cortexRenderers)
        {
            brainBounds.Encapsulate(renderer.bounds.center);
        }

        centerOfBrainModel = brainBounds.center;
        // Create and populate the list of brain structures
        // We add up all the initial positions of the structures to later determine the middle
        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("Structure"))
        {
            if (structure.transform.GetChild(0).name.Contains("Container"))
            {
                foreach (HighlightAndLabelCommands cur in structure.GetComponentsInChildren<HighlightAndLabelCommands>())
                {
                    if (!STRUCTURES_THAT_DO_NOT_EXPLODE.Contains(structure.name))
                    {
                        explodingStructures.Add(new BrainStructure(cur.gameObject.transform, centerOfBrainModel));
                    }
                }
            }
            else
            {

                if (!STRUCTURES_THAT_DO_NOT_EXPLODE.Contains(structure.name))
                {
                    explodingStructures.Add(new BrainStructure(structure.transform, centerOfBrainModel));
                }
            }
        }

        ResetExplode();
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

    public void ToggleExplode()
    {
        if (this.GetComponent<StateAccessor>().AbleToTakeAnInteraction())
        {
            soundFX.Play();
            // We toggle the last state and then send the message to all other HoloLenses
            switch (lastState)
            {
                case ExplodingState.ExplodingOut:
                    lastState = ExplodingState.ExplodingIn;
                    currentState = ExplodingState.ExplodingIn;
                    cortex.SetActive(true);
                    break;
                case ExplodingState.ExplodingIn:
                    lastState = ExplodingState.ExplodingOut;
                    currentState = ExplodingState.ExplodingOut;
                    cortex.SetActive(false); // The cortex should be deactivated before the brain explodes
                    break;
            }
        }
    }

    public bool Exploded()
    {
        // this is the state when we are not exploded, any other state is either exploded, or in the process of exploding
        return !(currentState == ExplodingState.Resting && lastState == ExplodingState.ExplodingIn);
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
        if(ExplodeButton != null)
        {
            ExplodeButton.SetActive(true);
        }
        if(CollapseButton != null)
        {
            CollapseButton.SetActive(false);
        }
    }
}
