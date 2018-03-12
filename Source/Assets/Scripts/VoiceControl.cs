// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;

public class VoiceControl : MonoBehaviour {
    private const string BRAIN_PARTS_NAME = "BrainParts";
    private const string BRAIN_GAME_OBJECT_NAME = "Brain";

   // private const string MRI_SCANS = "MRIScans";
    private const string Control_UI = "ControlsUI";

    private const string HOLOGRAM_COLLECTION_NAME = "HologramCollection";
    private const string MRI_SCANS_NAME = "MRIScans";
    private const string MRI_COLLECTION_NAME = "MRICollection";


    private GameObject brain;
    private GameObject brainStructures;
    private GameObject mriScans;

    private GameObject ControlsUI;

    private GameObject mriCollection;
    private HTGazeManager gazeManager;


    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> voiceRecognitionKeywords;
    public static Dictionary <string, string> brainStructureNameToGameObjectName;
    private Dictionary<string, string> buttonActionsToGameObjectName;

    // Use this for initialization
    void Start () {
        // Referencing game objects to access their scripts
        brain = GameObject.Find(BRAIN_GAME_OBJECT_NAME);
        brainStructures = GameObject.Find(BRAIN_PARTS_NAME);

       // mriScans = GameObject.Find(MRI_SCANS);
        ControlsUI= GameObject.Find(Control_UI);

        gazeManager = GameObject.Find(HOLOGRAM_COLLECTION_NAME).GetComponent<HTGazeManager>();
        mriScans = GameObject.Find(MRI_SCANS_NAME);
        mriCollection = GameObject.Find(MRI_COLLECTION_NAME);

        voiceRecognitionKeywords = new Dictionary<string, System.Action>();

        brainStructureNameToGameObjectName = new Dictionary<string, string>
        {
            { "Putamen", "right_putamen" },
            { "Caudate", "right_caudate" },
            { "Globus Pallidus", "right_globus_pallidus" },
            { "Substantia Nigra", "right_substantia_nigra" },
            { "Subthalamic Nucleus", "right_subthalamic" },
            { "Thalamus", "thalamus" },
        };

        foreach (var item in brainStructureNameToGameObjectName)
        {
            voiceRecognitionKeywords.Add("Add " + item.Key,()=> HandleAddBrainPart(item.Key));
            voiceRecognitionKeywords.Add("Remove " + item.Key, ()=> HandleRemoveBrainPart(item.Key));
        }

        //map voice commands to the corresponding button name
        buttonActionsToGameObjectName = new Dictionary<string, string>
        {
            { "Rotate", "rotate-icon" },
            { "Stop", "rotate-icon" },
            { "Expand", "expand-icon" },
            { "Collapse", "expand-icon" },
            { "Reset", "reset-icon" },
            {"Scale Up", "scale-up" },
            {"Scale Down", "scale-down" },
            {"Isolate", "isolate-mode-icon" },
            {"Go Back", "exit-mode-icon" },
            {"Reposition", "reposition-icon" },
            {"Add All", "add-icon" },
            {"Remove All", "remove-icon" },
            { "Putamen", "putamen-icon" },
            { "Caudate", "caudate-icon" },
            { "Globus Pallidus", "globus-icon" },
            { "Substantia Nigra", "nigra-icon" },
            { "Subthalamic Nucleus", "subthalamic-icon" },
            { "Thalamus", "thalamus-icon" },
            { "MRI", "mri-icon" },
            { "MRI Outline", "show-colour-icon" },
            { "Pin", "pin-icon" },
            { "Structures", "structures-icon" },
        };

        voiceRecognitionKeywords.Add("Rotate", HandleStartRotate); 
        voiceRecognitionKeywords.Add("Stop", HandleStopRotate);
        voiceRecognitionKeywords.Add("Scale Up", HandleScaleUp);
        voiceRecognitionKeywords.Add("Scale Down", HandleScaleDown);
        voiceRecognitionKeywords.Add("Expand", HandleExplode);
        voiceRecognitionKeywords.Add("Collapse", HandleCollapse);
        voiceRecognitionKeywords.Add("Show Isolate", HandleIsolate);
        voiceRecognitionKeywords.Add("Go Back", HandleConcludeIsolate);
        voiceRecognitionKeywords.Add("Reset", HandleResetState);
        voiceRecognitionKeywords.Add("Reposition", HandleResetAnchor);
        voiceRecognitionKeywords.Add("Add All", HandleAddAll);
        voiceRecognitionKeywords.Add("Remove All", HandleRemoveAll);
        //UNCOMMENT THIS FOR GAZE MARKER
//        voiceRecognitionKeywords.Add("Place Marker", HandlePlaceMarker);
//        voiceRecognitionKeywords.Add("Clear Marker", HandleClearMarker);
        voiceRecognitionKeywords.Add("Show MRI Scans", HandleMRI);
        voiceRecognitionKeywords.Add("Show Outline", HandleMRIOutlineOn);
        voiceRecognitionKeywords.Add("Hide Outline", HandleMRIOutlineOff);
        //voiceRecognitionKeywords.Add("Toggle Outline", HandleMRIOutline);
        voiceRecognitionKeywords.Add("Pin Menu", HandlePinMenu);
        voiceRecognitionKeywords.Add("UnPin Menu", HandleUnpinMenu);
        voiceRecognitionKeywords.Add("Show Structures", HandleStructuresMode);

        keywordRecognizer = new KeywordRecognizer(voiceRecognitionKeywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (voiceRecognitionKeywords.TryGetValue(args.text, out keywordAction))
        {
            Debug.Log(args.text);
            if (!brainStructures.GetComponent<StateAccessor>().CurrentlyInStudentMode())
            {
                keywordAction.Invoke();
            }
        }               
    }

    private void HandleStartRotate()
    {
        if (!brainStructures.GetComponent<RotateStructures>().isRotating)
        {
            GameObject.Find(buttonActionsToGameObjectName["Rotate"]).GetComponent<ButtonCommands>().OnSelect();
        }

        brainStructures.GetComponent<RotateStructures>().StartRotate();
    }

    private void HandleStopRotate()
    {
        if (brainStructures.GetComponent<RotateStructures>().isRotating)
        {
            GameObject.Find(buttonActionsToGameObjectName["Stop"]).GetComponent<ButtonCommands>().OnSelect();
        }

        brainStructures.GetComponent<RotateStructures>().StopRotate();
    }

    private void HandleScaleUp()
    {
        brainStructures.GetComponent<ScaleToggler>().ScaleUp();
        GameObject.Find(buttonActionsToGameObjectName["Scale Up"]).GetComponent<ButtonCommands>().OnSelect();

    }

    private void HandleScaleDown()
    {
        brainStructures.GetComponent<ScaleToggler>().ScaleDown();
        GameObject.Find(buttonActionsToGameObjectName["Scale Down"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleExplode()
    {
        if (brainStructures.GetComponent<ExplodingCommands>().lastState == ExplodingCommands.ExplodingState.ExplodingIn)
        {
            GameObject.Find(buttonActionsToGameObjectName["Expand"]).GetComponent<ButtonCommands>().OnSelect();
        }
        
        brainStructures.GetComponent<ExplodingCommands>().TryToExplode();
    }

    private void HandleCollapse()
    {
        if (brainStructures.GetComponent<ExplodingCommands>().lastState == ExplodingCommands.ExplodingState.ExplodingOut)
        {
            GameObject.Find(buttonActionsToGameObjectName["Collapse"]).GetComponent<ButtonCommands>().OnSelect();
        }

        brainStructures.GetComponent<ExplodingCommands>().TryToCollapse();
    }

    private void HandleIsolate()
    {
        brainStructures.GetComponent<IsolateStructures>().InitiateIsolationMode();
        GameObject.Find(buttonActionsToGameObjectName["Isolate"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleConcludeIsolate()
    {
        GameObject.Find(buttonActionsToGameObjectName["Go Back"]).GetComponent<ButtonCommands>().OnSelect();
        brainStructures.GetComponent<IsolateStructures>().ConcludeIsolationMode();
    }

    private void HandleResetState()
    {
        brainStructures.GetComponent<ResetState>().ResetEverything();
        ControlsUI.GetComponent<SubMenusManager>().EnableDefaultMenus();
        if (GameObject.Find(buttonActionsToGameObjectName["Reset"]) != null)
        {
            GameObject.Find(buttonActionsToGameObjectName["Reset"]).GetComponent<ResetButtonAction>().ResetUI();
        }
       
    }

    private void HandleResetAnchor()
    {
        // TODO: 2 brain fix
        brain.GetComponent<HologramPlacement>().ResetStage();
        GameObject.Find(buttonActionsToGameObjectName["Reposition"]).GetComponent<ButtonCommands>().OnSelect();
    }


    private void HandleAddAll()
    {
       // brainStructures.GetComponent<IsolateStructures>().AddAllParts();
        GameObject.Find(buttonActionsToGameObjectName["Add All"]).GetComponent<ButtonCommands>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Add All"]).GetComponent<IsolateButtonAction>().OnSelect();
    }

    private void HandleRemoveAll()
    {
       //brainStructures.GetComponent<IsolateStructures>().RemoveAllParts();
        GameObject.Find(buttonActionsToGameObjectName["Remove All"]).GetComponent<ButtonCommands>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Remove All"]).GetComponent<IsolateButtonAction>().OnSelect();

    }

    private void HandleAddBrainPart(string partName)
    {
        brainStructures.GetComponent<IsolateStructures>().TryToIsolate(brainStructureNameToGameObjectName[partName]);
        GameObject.Find(buttonActionsToGameObjectName[partName]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleRemoveBrainPart(string partName)
    {
        brainStructures.GetComponent<IsolateStructures>().TryToReturnFromIsolate(brainStructureNameToGameObjectName[partName]);
        GameObject.Find(buttonActionsToGameObjectName[partName]).GetComponent<ButtonCommands>().OnSelect();
    }

    //UNCOMMENT THIS FOR GAZE MARKER
    //private void HandlePlaceMarker()
    //{
    //    if (gazeManager.Hit)
    //    {
    //        gazeManager.HitInfo.transform.GetComponent<GazeMarkerCommands>().OnSelect();
    //    }
    //}

    //UNCOMMENT THIS FOR GAZE MARKER
    //private void HandleClearMarker()
    //{
    //    if (gazeManager.Hit)
    //    {
    //        gazeManager.HitInfo.transform.GetComponent<GazeMarkerCommands>().RemoveMarkerFromStructure();
    //    }
    //    //        brainStructures.GetComponent<GazeMarkerManager>().TryToRemoveGazeMarker();
    //}

    private void HandleMRI()
    {
        mriCollection.GetComponent<MRIManager>().ProcessMRIButtonAction();
        if (GameObject.Find(buttonActionsToGameObjectName["MRI"]))
        {
            GameObject.Find(buttonActionsToGameObjectName["MRI"]).GetComponent<ButtonCommands>().OnSelect();
        }
        else
        {
            GameObject.Find("ControlsUI").GetComponent<SubMenusManager>().ToggleMenuUI("mri-icon");
        }
  
    }

    private void HandleMRIOutlineOn()
    {
        if (!mriCollection.GetComponent<MRIManager>().IsOutlinedMRIImages())
        {
            GameObject.Find(buttonActionsToGameObjectName["MRI Outline"]).GetComponent<ButtonCommands>().OnSelect();
        }
        mriCollection.GetComponent<MRIManager>().TurnOnMRIImageOutlines();
    }

    private void HandleMRIOutlineOff()
    {
        if (mriCollection.GetComponent<MRIManager>().IsOutlinedMRIImages())
        {
            GameObject.Find(buttonActionsToGameObjectName["MRI Outline"]).GetComponent<ButtonCommands>().OnSelect();
        }
        mriCollection.GetComponent<MRIManager>().TurnOffMRIImageOutlines();
    }

    private void HandlePinMenu()
    {
        if (!GameObject.Find("ControlsUI").GetComponent<ControlsUIManager>().GetMenuPinState())
        {
            GameObject.Find(buttonActionsToGameObjectName["Pin"]).GetComponent<PinButtonAction>().OnSelect();
            GameObject.Find(buttonActionsToGameObjectName["Pin"]).GetComponent<ButtonCommands>().OnSelect();
        }
    }

    private void HandleUnpinMenu()
    {
        if (GameObject.Find("ControlsUI").GetComponent<ControlsUIManager>().GetMenuPinState())
        {
            GameObject.Find(buttonActionsToGameObjectName["Pin"]).GetComponent<PinButtonAction>().OnSelect();
            GameObject.Find(buttonActionsToGameObjectName["Pin"]).GetComponent<ButtonCommands>().OnSelect();
        }
    }

    private void HandleStructuresMode()
    {
        GameObject.Find(buttonActionsToGameObjectName["Structures"]).GetComponent<StructuresButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Structures"]).GetComponent<ButtonCommands>().OnSelect();
    }


}
