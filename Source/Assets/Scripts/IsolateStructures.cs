// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IsolateStructures : MonoBehaviour {
    private class MovingStructure
    {
        public Transform ModelTransform;
        private Vector3 finalPosition;
        private Vector3 finalScale;
        private Quaternion initialRotation;
        private Quaternion finalRotation;
        private float rotationTimeElapsedPercentage;
        private Vector3 positionTransitionVector;
        private Vector3 scaleTransitionVector;

        public MovingStructure(Transform structure, Vector3 finalPosition, Vector3 finalScale, Quaternion finalRotation)
        {
            ModelTransform = structure;
            this.finalPosition = finalPosition;
            this.finalScale = finalScale;
            this.initialRotation = structure.rotation;
            this.finalRotation = finalRotation;
            rotationTimeElapsedPercentage = 0f;
            positionTransitionVector = finalPosition - structure.localPosition;
            scaleTransitionVector = finalScale - structure.localScale;
        }

        public void MoveStructure()
        {
            if (!FinalPositionReached())
            {
                ModelTransform.localPosition += Time.deltaTime / ISOLATION_TRANSITION_TIME_IN_SECONDS * positionTransitionVector;
            }
            if (!FinalScaleReached())
            {
                ModelTransform.localScale += Time.deltaTime / ISOLATION_TRANSITION_TIME_IN_SECONDS * scaleTransitionVector;
            }
            if (!FinalRotationReached())
            {
                rotationTimeElapsedPercentage += Time.deltaTime / ISOLATION_TRANSITION_TIME_IN_SECONDS;
                if (rotationTimeElapsedPercentage >= 1f)
                {
                    ModelTransform.rotation = finalRotation;
                }
                else
                {
                    ModelTransform.rotation = Quaternion.Lerp(initialRotation, finalRotation, rotationTimeElapsedPercentage);
                }
            }
        }

        public void SnapToFinalPositionAndScale()
        {
            ModelTransform.localPosition = finalPosition;
            ModelTransform.localScale = finalScale;
            if (finalRotation != Quaternion.identity)
            {
                ModelTransform.rotation = finalRotation;
            }
        }

        public bool IsDoneMoving()
        {
            return FinalPositionReached() && FinalScaleReached() && FinalRotationReached();
        }

        private bool FinalPositionReached()
        {
            return (ModelTransform.localPosition - finalPosition).magnitude < Time.deltaTime / ISOLATION_TRANSITION_TIME_IN_SECONDS * positionTransitionVector.magnitude || ModelTransform.localPosition.Equals(finalPosition);
        }

        private bool FinalScaleReached()
        {
            return (ModelTransform.localScale - finalScale).magnitude < Time.deltaTime / ISOLATION_TRANSITION_TIME_IN_SECONDS * scaleTransitionVector.magnitude || ModelTransform.localScale.Equals(finalScale);
        }

        private bool FinalRotationReached()
        {
            return ModelTransform.rotation == finalRotation || finalRotation == Quaternion.identity;
        }
    }

    public enum MovingToState
    {
        Default,
        Isolation,
        Minimap
    }

    private struct MovingStructureWithDirection
    {
        public MovingStructure structure;
        public MovingToState direction;

        public MovingStructureWithDirection(MovingStructure structure, MovingToState direction)
        {
            this.structure = structure;
            this.direction = direction;
        }
    }

    private const float ISOLATED_STRUCTURE_SCALE_SIZE = 2.0f;
    private const float MINIMAP_STRUCTURES_SCALE_SIZE = 0.2f;
    private const float ISOLATION_TRANSITION_TIME_IN_SECONDS = 1.5f;
    private const string BRAIN_PARTS_GAMEOBJECT_NAME = "BrainParts";
    private const string BRAIN_PARTS2_GAMEOBJECT_NAME = "BrainParts2"; //ADDED
    private const string CORTEX_OBJECT_NAME = "cortex_low";
    private const string VENTRICLE_OBJECT_NAME = "ventricle";
    private const string BRAIN_MINIMAP_POSITION_OBJECT = "MinimapPositionObject";

    private CustomMessages customMessages;
    private List<Transform> structuresList;
    private List<Transform> isolatedStructures;
    private List<MovingStructureWithDirection> movingStructures;
    private Vector3 defaultStructurePosition;
    private Vector3 defaultStructurePosition2; //ADDED
    private Vector3 defaultStructureScale;
    private Quaternion defaultStructureRotation;
    private Vector3 isolatedStructureScale;
    private Vector3 minimapStructurePosition;
    private Vector3 minimapStructureScale;
    private Quaternion minimapStructureRotation;
    private bool currentlyInIsolatedMode;
    private StateAccessor stateAccessor;
    private BoxCollider cortexBoxCollider;
    
    public bool AtLeastOneStructureIsMovingOrResizing
    {
        get { return movingStructures.Any(); }
    }

    void Start () {
        customMessages = CustomMessages.Instance;
        stateAccessor = StateAccessor.Instance;
        if (customMessages != null)
        {
            customMessages.MessageHandlers[CustomMessages.TestMessageID.InitiateIsolateMessage] = InitiateIsolateMessageReceived;
            customMessages.MessageHandlers[CustomMessages.TestMessageID.TryToIsolateStructure] = TryToIsolateStructureMessageReceived;
            customMessages.MessageHandlers[CustomMessages.TestMessageID.TryToReturnIsolatedStructure] = TryToReturnIsolatedStructureMessageReceived;
            customMessages.MessageHandlers[CustomMessages.TestMessageID.ConcludeIsolate] = ConcludeIsolateMessageReceived;
        }

        structuresList = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            structuresList.Add(transform.GetChild(i));
        }

        isolatedStructures = new List<Transform>();
        movingStructures = new List<MovingStructureWithDirection>();

        CalculateDefaultAndFinalPositionsScalesAndRotations();
        currentlyInIsolatedMode = false;
        cortexBoxCollider = GameObject.Find(CORTEX_OBJECT_NAME).GetComponent<BoxCollider>();
    }

    private GameObject CopyStructure(GameObject structureToCopy)
    {
        GameObject copiedStructure = Instantiate<GameObject>(structureToCopy);
        copiedStructure.transform.SetParent(structureToCopy.transform.parent);
        copiedStructure.transform.position = structureToCopy.transform.position;
        copiedStructure.transform.localScale = structureToCopy.transform.localScale;
        return copiedStructure;
    }

    void Update () {
        if (AtLeastOneStructureIsMovingOrResizing)
        {
            MoveStructures();
        }
	}

    public void InitiateIsolateMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();
        InitiateIsolate();
    }

    public void TryToIsolateStructureMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();
        TryToIsolateStructure(msg.ReadString());
    }

    public void TryToReturnIsolatedStructureMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();
        TryToReturnIsolatedStructure(msg.ReadString());
    }

    public void ConcludeIsolateMessageReceived(NetworkInMessage msg)
    {
        // This reads the user ID which we do not need
        msg.ReadInt64();
        ConcludeIsolation();
    }

    private void CalculateDefaultAndFinalPositionsScalesAndRotations()
    {
        defaultStructurePosition = GameObject.Find(BRAIN_PARTS_GAMEOBJECT_NAME).transform.localPosition;
        defaultStructurePosition2 = GameObject.Find(BRAIN_PARTS2_GAMEOBJECT_NAME).transform.localPosition; //ADDED
        defaultStructureScale = transform.GetChild(0).localScale;
        defaultStructureRotation = transform.GetChild(0).rotation;
        minimapStructurePosition = 0.3f * GameObject.Find(BRAIN_MINIMAP_POSITION_OBJECT).transform.localPosition;
        minimapStructureScale = new Vector3(MINIMAP_STRUCTURES_SCALE_SIZE, MINIMAP_STRUCTURES_SCALE_SIZE, MINIMAP_STRUCTURES_SCALE_SIZE);
        isolatedStructureScale = new Vector3(ISOLATED_STRUCTURE_SCALE_SIZE, ISOLATED_STRUCTURE_SCALE_SIZE, ISOLATED_STRUCTURE_SCALE_SIZE);
    }

    public void InitiateIsolationMode()
    {
        InitiateIsolate();
        if (customMessages != null)
        {
            customMessages.SendInitiateIsolateMessage();
        }
    }

    private void InitiateIsolate()
    {
        if (AtLeastOneStructureIsMovingOrResizing)
        {
            return;
        }

        if (currentlyInIsolatedMode)
        {
            Debug.Log("InitiateIsolate() was called, but the brain is currently in isolation mode");
            return;
        }

        if (stateAccessor.ChangeMode(StateAccessor.Mode.Isolated))
        {
            GetComponent<ResetState>().ResetInteractions();
            CalculateDefaultAndFinalPositionsScalesAndRotations();
            ResetStructurePositionsAndScales();
            StartSettingUpMinimap();

            GameObject ventricles = GameObject.Find(VENTRICLE_OBJECT_NAME);
            ventricles.AddComponent<MeshCollider>();
            StartIsolatingStructure(ventricles);
            Destroy(ventricles.GetComponent<MeshCollider>());

            cortexBoxCollider.enabled = true;
            currentlyInIsolatedMode = true;
        }
        else
        {
            Debug.Log("Isolate Structures: Cannot currently enter isolate mode.");
        }
    }

    public void ConcludeIsolationMode()
    {
        if (!currentlyInIsolatedMode)
        {
            Debug.Log("ConcludeIsolationMode() was called, but no structure is currently isolated");
            return;
        }

        ConcludeIsolation();
        if (customMessages != null)
        {
            customMessages.SendConcludeIsolateMessage();
        }
    }

    private void ConcludeIsolation()
    {
        //UNCOMMENT THIS FOR GAZE MARKER
//        GameObject.Find(BRAIN_PARTS_GAMEOBJECT_NAME).GetComponent<GazeMarkerManager>().ClearMarkerLocally();

        if (AtLeastOneStructureIsMovingOrResizing)
        {
            Debug.Log("ConcludeIsolation() was called, but there are currently structures moving");
            return;
        }

        if (stateAccessor.ChangeMode(StateAccessor.Mode.Default))
        {
            StartReturningStructuresToDefaultState();
            currentlyInIsolatedMode = false;
        }
        else
        {
            Debug.Log("Isolate Structures: Cannot currently exit Isolate mode.");
        }
     }

    public void TryToIsolate(string structureName)
    {
        if (GameObject.Find(structureName) == null)
        {
            Debug.Log("Tried to isolate the structure '" + structureName + "', which could not be found");
            return;
        }

        TryToIsolateStructure(structureName);
        if (customMessages != null)
        {
            customMessages.SendTryToIsolateStructureMessage(structureName);
        }
    }

    private void TryToIsolateStructure(string structureName)
    {
        GameObject structureToIsolate = GameObject.Find(structureName);
        if (currentlyInIsolatedMode && GameObject.Find(structureToIsolate.name + "(Clone)") == null)
        {
            StartIsolatingStructure(structureToIsolate);
        }
    }

    public void TryToReturnFromIsolate(string structureName)
    {
        if (GameObject.Find(structureName + "(Clone)") == null)
        {
            Debug.Log("Tried to return from isolation the structure '" + structureName + "', which could not be found");
            return;
        }

        foreach (MovingStructureWithDirection movingStructure in movingStructures)
        {
            if (movingStructure.structure.ModelTransform.name == structureName + "(Clone)")
            {
                return;
            }
        }

        TryToReturnIsolatedStructure(structureName);
        if (customMessages != null)
        {
            customMessages.SendTryToReturnIsolatedStructureMessage(structureName);
        }
        Debug.Log("Try to return from isolate is done for " + structureName);
    }

    private void TryToReturnIsolatedStructure(string structureName)
    {
        GameObject structureToReturnFromIsolate = GameObject.Find(structureName+"(Clone)");
        if (currentlyInIsolatedMode)
        //if (currentlyInIsolatedMode && !AtLeastOneStructureIsMovingOrResizing)
        {
            StartRemovingIsolatedStructure(structureToReturnFromIsolate);
        }
        Debug.Log("Try to return from isolateed structure is done for " + structureName);
    }

    private void StartSettingUpMinimap()
    {
        foreach (Transform structure in structuresList)
        {
            movingStructures.Add(new MovingStructureWithDirection(new MovingStructure(structure, minimapStructurePosition, minimapStructureScale, structure.rotation), MovingToState.Minimap));
        }
        minimapStructureRotation = movingStructures[0].structure.ModelTransform.rotation;
    }

    private void StartIsolatingStructure(GameObject structureToIsolate)
    {
        Transform copiedStructureToIsolate = CopyStructure(structureToIsolate).transform;
        if (isolatedStructures.Any())
        {
            movingStructures.Add(new MovingStructureWithDirection(new MovingStructure(copiedStructureToIsolate, defaultStructurePosition, isolatedStructureScale, Quaternion.identity), MovingToState.Isolation));
            copiedStructureToIsolate.rotation = isolatedStructures[0].rotation;
        }
        else
        {
            movingStructures.Add(new MovingStructureWithDirection(new MovingStructure(copiedStructureToIsolate, defaultStructurePosition, isolatedStructureScale, minimapStructureRotation), MovingToState.Isolation));
        }
        isolatedStructures.Add(copiedStructureToIsolate);
        UpdateListOfRotatingStructures();
    }

    private void StartRemovingIsolatedStructure(GameObject structureToRemove)
    {
        movingStructures.Add(new MovingStructureWithDirection(new MovingStructure(structureToRemove.transform, minimapStructurePosition, minimapStructureScale, minimapStructureRotation), MovingToState.Minimap));
        //UNCOMMENT THIS FOR GAZE MARKER
//        structureToRemove.GetComponent<GazeMarkerCommands>().RemoveMarkerFromStructure();
        isolatedStructures.Remove(structureToRemove.transform);
        UpdateListOfRotatingStructures();
        Debug.Log("removing the isolated part "+ structureToRemove.name);
    }

    private void StartReturningStructuresToDefaultState()
    {
        foreach (Transform structure in structuresList.Union(isolatedStructures).ToList())
        {
            movingStructures.Add(new MovingStructureWithDirection(new MovingStructure(structure, defaultStructurePosition, defaultStructureScale, defaultStructureRotation), MovingToState.Default));
        }

        isolatedStructures.Clear();
        UpdateListOfRotatingStructures();
     }

    private void MoveStructures()
    {
        MovingStructureWithDirection currentStructureWithDirection;
        for (int i = movingStructures.Count - 1; i >= 0; i--)
        {
            currentStructureWithDirection = movingStructures[i];
            currentStructureWithDirection.structure.MoveStructure();
            if (currentStructureWithDirection.structure.IsDoneMoving())
            {
                FinishMovingAndResizingStructure(currentStructureWithDirection);
                movingStructures.RemoveAt(i);
            }
        }

        if (!AtLeastOneStructureIsMovingOrResizing && !currentlyInIsolatedMode)
        {
            ResetIsolate();
        }

    }

    private void FinishMovingAndResizingStructure(MovingStructureWithDirection structureWithDirection)
    {
        structureWithDirection.structure.SnapToFinalPositionAndScale();
        switch(structureWithDirection.direction)
        {
            case MovingToState.Isolation:
                if (structureWithDirection.structure.ModelTransform.name != VENTRICLE_OBJECT_NAME+"(Clone)")
                {
                    structureWithDirection.structure.ModelTransform.GetComponent<HighlightAndLabelCommands>().TurnOnLockedHighlight();
                    GameObject.Find(structureWithDirection.structure.ModelTransform.name.Replace("(Clone)", "")).GetComponent<HighlightAndLabelCommands>().TurnOnLockedHighlight();
                }
                break;
            case MovingToState.Minimap:
                if (structureWithDirection.structure.ModelTransform.name.Contains("(Clone)"))
                {
                    GameObject.Find(structureWithDirection.structure.ModelTransform.name.Replace("(Clone)", "")).GetComponent<HighlightAndLabelCommands>().TurnOffLockedHighlight();
                    GameObject.DestroyImmediate(structureWithDirection.structure.ModelTransform.gameObject);
                }
                break;
            case MovingToState.Default:
                if (structureWithDirection.structure.ModelTransform.name.Contains("(Clone)"))
                {
                    if (structureWithDirection.structure.ModelTransform.name != VENTRICLE_OBJECT_NAME + "(Clone)")
                    {
                        structureWithDirection.structure.ModelTransform.GetComponent<HighlightAndLabelCommands>().TurnOffLockedHighlight();
                        GameObject.Find(structureWithDirection.structure.ModelTransform.name.Replace("(Clone)", "")).GetComponent<HighlightAndLabelCommands>().TurnOffLockedHighlight();
                    }
                    GameObject.DestroyImmediate(structureWithDirection.structure.ModelTransform.gameObject);
                }
                break;
        }
    }

    private void UpdateListOfRotatingStructures()
    {
        this.GetComponent<RotateStructures>().SetIsolatedStructures(isolatedStructures);
        Debug.Log("updated the list of structures");
    }

    public bool CurrentlyInIsolationModeOrIsolating()
    {
        return AtLeastOneStructureIsMovingOrResizing || currentlyInIsolatedMode;
    }

    public void ResetIsolate()
    {
        foreach (Transform structure in isolatedStructures)
        {
            GameObject.DestroyImmediate(structure.gameObject);
        }

        foreach (MovingStructureWithDirection movingStructureWithDirection in movingStructures)
        {
            if (movingStructureWithDirection.structure.ModelTransform.name.Contains("(Clone)"))
            {
                GameObject.DestroyImmediate(movingStructureWithDirection.structure.ModelTransform.gameObject);
            }
        }
        //UNCOMMENT THIS FOR GAZE MARKER
//        GameObject.Find(BRAIN_PARTS_GAMEOBJECT_NAME).GetComponent<GazeMarkerManager>().ClearMarkerLocally();
        cortexBoxCollider.enabled = false;
        isolatedStructures.Clear();
        movingStructures.Clear();
        ResetStructurePositionsAndScales();
        currentlyInIsolatedMode = false;
    }

    private void ResetStructurePositionsAndScales()
    {
        foreach (Transform structure in structuresList)
        {
            structure.localPosition = defaultStructurePosition;
            structure.localScale = defaultStructureScale;
        }
    }
    
    public void AddAllParts()
    {
        foreach( var item in VoiceControl.brainStructureNameToGameObjectName)
        {
            TryToIsolate(item.Value);
        }
    }

    public void RemoveAllParts()
    {
        foreach (var item in VoiceControl.brainStructureNameToGameObjectName)
        {
            TryToReturnFromIsolate(item.Value);
        }
    }

    public bool AStructureIsMovingOrResizing()
    {
        return AtLeastOneStructureIsMovingOrResizing;
    }

}

