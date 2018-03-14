// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIManager : Singleton<MRIManager> {
    private const string MRIObjectTag_1 = "MRI_1";
	private const string MRIObjectTag_2 = "MRI_2";
	private const string CLIP_PLANE = "ClipPlane";
    private const string BRAIN_PARTS_1 = "BrainParts";
    private const string BRAIN_PARTS_2 = "BrainParts2";
    private const string BRAIN_1 = "Brain";

	private GameObject MRICollection;
	private List<GameObject> MRIObjects, allMRIObjects;
    private GameObject activeMRI;
    private GameObject clipPlane;
    private GameObject brainParts_1, brainParts_2;
    private ColoursAccessor coloursAccessor;
    private CustomMessages customMessages;
    private StateAccessor stateAccessor;
    private bool isInMRIMode = false;
    private MoveClippingPlane moveClippingPlane;
    private bool isOutlinedMRIImages = true;
    private bool isOneMRIActive = false;
    private BoxCollider boxCollider;

	
    private const string BRAIN_1_NAME = "BrainParts";
    private const string BRAIN_2_NAME = "BrainParts2";
    private const string BRAIN_SELECTION_CONTROLLER = "selectBrainController";

    GameObject currentBrain;
    private string selectedBrainName;
    private const string BRAIN_1_GAME_OBJECT_NAME = "Brain";



    private string __selectedBrain;
    private GameObject selectBrainControlGameObject;

    /*
	private List<GameObject> SelectedMRIObjects {
		get {
			__selectedBrain = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
			return (__selectedBrain == BRAIN_1) ? (mriObjects_1) : (mriObjects_2);
		}
	}*/
	
	// Use this for initialization
	void Start () {
		MRICollection = this.gameObject;
		MRIObjects = new List<GameObject>();
        allMRIObjects = new List<GameObject>();
        //mriObjects_2 = new List<GameObject>();
        customMessages = CustomMessages.Instance;
        coloursAccessor = ColoursAccessor.Instance;
        clipPlane = MRICollection.transform.Find(CLIP_PLANE).gameObject;
        moveClippingPlane = clipPlane.GetComponent<MoveClippingPlane>();
        
        //brainParts = GameObject.Find((__selectedBrain == BRAIN_1) ? (BRAIN_PARTS_1) : (BRAIN_PARTS_2));
        brainParts_1 = GameObject.Find(BRAIN_PARTS_1);
        brainParts_2 = GameObject.Find(BRAIN_PARTS_2);

        stateAccessor = StateAccessor.Instance;
        selectBrainControlGameObject = GameObject.FindWithTag(BRAIN_SELECTION_CONTROLLER);
        boxCollider = GetComponent<BoxCollider>();

        boxCollider.enabled = false;

        if (customMessages != null)
        {
            customMessages.MessageHandlers[CustomMessages.TestMessageID.ToggleMRIButton] = this.ToggleMRIButtonMessageReceived;
            customMessages.MessageHandlers[CustomMessages.TestMessageID.ToggleMRIImages] = this.ChangeMRIImageMessageReceived;
        }

        MRIObjects.Add(MRICollection.transform.Find("MRI1").gameObject);
        MRIObjects.Add(MRICollection.transform.Find("MRI3").gameObject);
        foreach (GameObject MRIObject in GameObject.FindGameObjectsWithTag(MRIObjectTag_1))
        {
            allMRIObjects.Add(MRIObject);
        }

        foreach (GameObject MRIObject in GameObject.FindGameObjectsWithTag(MRIObjectTag_2)) {
        	allMRIObjects.Add(MRIObject);
        }

        SetMRICollectionChildrenActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect()
    {
        ReturnFromDisplaySingleMRI();
        if (customMessages != null)
        {
            customMessages.SendToggleMRIMessage(activeMRI.name, isOneMRIActive);
        }
    }


    /*Changes the MRI image that is displayed in every MRI object*/
    private void ChangeMRIImage()
    {
        //foreach(GameObject MRIObject in SelectedMRIObjects)
        foreach (GameObject MRIObject in allMRIObjects)
        {
            MRIObject.GetComponent<MRIInteractions>().ChangeMRIImage(isOutlinedMRIImages);
        }
    }

    public void ProcessChangeMRIImageButtonAction()
    {
        isOutlinedMRIImages = !isOutlinedMRIImages;
        ChangeMRIImage();

        if (customMessages != null)
        {
            customMessages.SendChangeMRIImageButtonMessage(isOutlinedMRIImages);
        }
    }

    public void TurnOnMRIImageOutlines()
    {
        if (isInMRIMode)
        {
            isOutlinedMRIImages = true;
            ChangeMRIImage();

            if (customMessages != null)
            {
                customMessages.SendChangeMRIImageButtonMessage(isOutlinedMRIImages);
            }
        }
    }

    public void TurnOffMRIImageOutlines()
    {
        if (isInMRIMode)
        {
            isOutlinedMRIImages = false;
            ChangeMRIImage();

            if (customMessages != null)
            {
                customMessages.SendChangeMRIImageButtonMessage(isOutlinedMRIImages);
            }
        }
    }

    private void ChangeMRIImageMessageReceived(NetworkInMessage msg)
    {
        msg.ReadInt64();
        this.isOutlinedMRIImages = msg.ReadByte() == 1;
        ChangeMRIImage();
    }

    public void ProcessMRIButtonAction()
    {
        // TurnOnMRIMode();
        /* if (ToggleIsCurrentlyInMRIMode())
         {
             SetMRICollectionActive();
             customMessages.SendMRIButtonMessage(isInMRIMode);
         }
         else
         {
             Debug.Log("MRIManager: Cannot currently switch modes.");
         }*/

        if (stateAccessor.ChangeMode(StateAccessor.Mode.MRI))
        {
            LoadOrUnloadMRIObjects(true);
            if (customMessages != null)
            {
                customMessages.SendMRIButtonMessage(isInMRIMode);
            }
        }
    }

 /*   public bool TurnOnMRIMode()
    {
        if(stateAccessor.ChangeMode(StateAccessor.Mode.MRI))
        {
            isInMRIMode = true;
            SetMRICollectionActive();
            customMessages.SendMRIButtonMessage(isInMRIMode);
            return true;
        }

        return false;
    }*/

    public void ToggleMRIButtonMessageReceived(NetworkInMessage msg)
    {
        msg.ReadInt64();
       /* if (SetIsCurrentlyInMRIMode(msg.ReadByte() == 1))
        {*/
            LoadOrUnloadMRIObjects(true);
        /*}
        else
        {
            Debug.Log("MRIManager: Cannot current switch modes.");
        }*/
    }

   /* public bool ToggleIsCurrentlyInMRIMode()
    {
        if(TryToSwitchModes(!isInMRIMode))
        {
            isInMRIMode = !isInMRIMode;
            return true;
        }

        return false;
    }*/

   /* private bool TryToSwitchModes(bool isGoingIntoMRIMode)
    {
        bool successfullySwitchedModes;
        if(isGoingIntoMRIMode)
        {
            successfullySwitchedModes = stateAccessor.ChangeMode(StateAccessor.Mode.MRI);
        }
        else
        {
            successfullySwitchedModes = stateAccessor.ChangeMode(StateAccessor.Mode.Default);
        }

        return successfullySwitchedModes;
    }*/

   /* public bool SetIsCurrentlyInMRIMode(bool isInMRIMode)
    {
        if (TryToSwitchModes(isInMRIMode))
        {
            this.isInMRIMode = isInMRIMode;
            return true;
        }

        return false;
    }*/

    public bool isCurrentlyInMRIMode()
    {
        return isInMRIMode;
    }

    public bool IsOutlinedMRIImages()
    {
        return isOutlinedMRIImages;
    }

    public void ResetMRI()
    {
        LoadOrUnloadMRIObjects(false);
    }

    private void LoadOrUnloadMRIObjects(bool isGoingIntoMRIMode)
    {
        if (isGoingIntoMRIMode)
        {
            LoadMRIObjects();
        }
        else
        {
            UnloadMRIObjects();
        }
    }

    private bool LoadMRIObjects()
    {
        isInMRIMode = true;
        SetMRICollectionChildrenActive(true);
        if (stateAccessor.IsInCompareMode() == true)
        {
            brainParts_1.GetComponent<ResetState>().ResetInteractions();
            if (brainParts_2 != null)
                brainParts_2.GetComponent<ResetState>().ResetInteractions();
        }
        else
        {
            selectedBrainName = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            currentBrain = (selectedBrainName == BRAIN_1_GAME_OBJECT_NAME) ? (brainParts_1) : (brainParts_2);
            currentBrain.GetComponent<ResetState>().ResetInteractions();
        }
        coloursAccessor.ToggledLockedHighlightOnBrain();
        return true;
    }

    private void UnloadMRIObjects()
    {
        isInMRIMode = false;
        ReturnFromDisplaySingleMRI();
        moveClippingPlane.TurnOffClipping();
        SetMRICollectionChildrenActive(false);
        if (stateAccessor.IsInCompareMode() == true)
        {
            brainParts_1.GetComponent<ResetState>().ResetInteractions();
            if (brainParts_2 != null)
                brainParts_2.GetComponent<ResetState>().ResetInteractions();
        }
        else
        {
            selectedBrainName = selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain;
            currentBrain = (selectedBrainName == BRAIN_1_GAME_OBJECT_NAME) ? (brainParts_1) : (brainParts_2);
            currentBrain.GetComponent<ResetState>().ResetInteractions();
        }
    }

    private void SetMRICollectionChildrenActive(bool active)
    {
        //foreach (GameObject MRIObject in SelectedMRIObjects)
        foreach (GameObject MRIObject in allMRIObjects)
        {
            MRIObject.SetActive(active);
        }

        clipPlane.SetActive(active);
    }

    public void ProcessMRISelectionReceived(string toggledMRI, bool isOneMRIActive)
    {
        GameObject toggledMRIObject = GetMRIObjectByName(toggledMRI);

        if (isOneMRIActive)
        {
            toggledMRIObject.GetComponent<MRIInteractions>().DisplayMRIImage();
            DisplaySingleMRI(toggledMRIObject);
        }
        else
        {
           SetActiveMRI(toggledMRIObject);
           ReturnFromDisplaySingleMRI();
        }
    }

    private GameObject GetMRIObjectByName(string MRIName)
    {
        foreach (GameObject MRIObject in MRIObjects)
            //foreach (GameObject MRIObject in SelectedMRIObjects)
        {
            if (MRIObject.name == MRIName)
            {
                return MRIObject;
            }
        }

        return null;
    }

    public void SetActiveMRI(GameObject activeMRI)
    {
        this.activeMRI = activeMRI;
    }

    public void ReturnFromDisplaySingleMRI()
    {
        if (isOneMRIActive)
        {
            foreach (GameObject MRIObject in MRIObjects)
            //foreach (GameObject MRIObject in SelectedMRIObjects)
            {
                if (MRIObject.name != activeMRI.name)
                {
                    MRIObject.SetActive(true);
                }
            }

            activeMRI.GetComponent<MRIInteractions>().HideMRIImage();
            boxCollider.enabled = false;
            isOneMRIActive = false;
        }

    }

    public void DisplaySingleMRI(GameObject activeMRI)
    {
        SetActiveMRI(activeMRI);
        foreach (GameObject MRIObject in MRIObjects)
        //foreach (GameObject MRIObject in SelectedMRIObjects)
        {
            if (MRIObject.name != activeMRI.name)
            {
                MRIObject.SetActive(false);
            }
        }
        boxCollider.enabled = true;
        isOneMRIActive = true;
    }

    public void UpdateClippingForRepositioning(bool GotTransform)
    {
        clipPlane.SetActive(!GotTransform);
        if (!GotTransform)
        {
            moveClippingPlane.TurnOnClipping();
        }
        else
        {
            moveClippingPlane.TurnOffClipping();
            ReturnFromDisplaySingleMRI();
        }
    }
}
