// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class VoiceControl : MonoBehaviour {
    private const string BRAIN_PARTS_NAME = "BrainParts";
    private const string BRAIN_GAME_OBJECT_NAME = "Brain";

   // private const string MRI_SCANS = "MRIScans";
    private const string Control_UI = "ControlsUI";

    private const string HOLOGRAM_COLLECTION_NAME = "HologramCollection";
    private const string MRI_SCANS_NAME = "MRIScans";
    private const string MRI_COLLECTION_NAME = "MRICollection";

    private const string FMRI_BRAIN_NAME = "fMRIBrains"; // change later

    private const string TUTORIAL_SCENE_NAME= "Tutorial";


    private GameObject brain;
    private GameObject brainStructures;
    private GameObject mriScans;
    private GameObject crossfadeSlider;

    private GameObject fmriBrain;


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
        crossfadeSlider = GameObject.Find("CrossfadeSlider");
        

        fmriBrain = GameObject.Find(FMRI_BRAIN_NAME);
        //fmriBrain.SetActive(false);


        // mriScans = GameObject.Find(MRI_SCANS);
        ControlsUI = GameObject.Find(Control_UI);

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
            { "Rotate Walls", "rotate-walls-icon" },
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
            { "Microglia", "microglia-icon" },
            { "Channel 1", "red-icon" },
            { "Channel 2", "green-icon" },
            { "MRI", "mri-icon" },
            { "MRI Outline", "show-colour-icon" },
            { "Pin", "pin-icon" },
            { "Structures", "structures-icon" },
            // New Voice Commands
            { "Play", "play-icon" },
            { "Faster", "faster-icon" },
            { "Slower", "slower-icon" },
            { "Skip One", "skip-one-icon" },
            { "Skip Ten", "skip-ten-icon" },
            { "Back One", "back-one-icon" },
            { "Back Ten", "back-ten-icon" },
            { "Educational Room", "structures-icon"},
            { "MRI Room", "mri-icon"},
            { "fMRI Room", "fmri-icon"},
            { "Brain Cell Room", "brain-cell-icon"},
            { "End Tutorial", "SkipButton"},
            { "Next", "NextButton"}
        };

        voiceRecognitionKeywords.Add("Rotate", HandleStartRotate);
        voiceRecognitionKeywords.Add("Rotate Walls", HandleStartRotateWalls);
        voiceRecognitionKeywords.Add("Stop", HandleStopRotate);
        voiceRecognitionKeywords.Add("Stop Walls", HandleStartRotateWalls);
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
        // New Voice Commands

        voiceRecognitionKeywords.Add("Play", HandleStartPlay);
        voiceRecognitionKeywords.Add("Pause", HandleStartPlay);
        voiceRecognitionKeywords.Add("Faster", HandleSpeedUpPlayback);
        voiceRecognitionKeywords.Add("Slower", HandleSlowDownPlayback);
        voiceRecognitionKeywords.Add("Skip One", HandleSkipOne);
        voiceRecognitionKeywords.Add("Back One", HandleBackOne);
        voiceRecognitionKeywords.Add("Skip Ten", HandleSkipTen);
        voiceRecognitionKeywords.Add("Back Ten", HandleBackTen);

        voiceRecognitionKeywords.Add("Educational Room", HandleEducationalRoom);
        voiceRecognitionKeywords.Add("FMRI Room", HandleFMRIRoom);
        voiceRecognitionKeywords.Add("Functional MRI Room", HandleFMRIRoom);
        voiceRecognitionKeywords.Add("MRI Room", HandleMRIRoom);
        voiceRecognitionKeywords.Add("MRI Scan Room", HandleMRIRoom);
        voiceRecognitionKeywords.Add("Brain Cell Room", HandleBrainCellRoom);
        voiceRecognitionKeywords.Add("Cell Room", HandleBrainCellRoom);

        voiceRecognitionKeywords.Add("Show Microglia", HandleShowMicroglia);
        voiceRecognitionKeywords.Add("Show Channel One", HandleShowChannelOne);
        voiceRecognitionKeywords.Add("Show Channel Two", HandleShowChannelTwo);

        //UNCOMMENT THIS FOR GAZE MARKER
        voiceRecognitionKeywords.Add("Place Marker", HandlePlaceMarker);
        voiceRecognitionKeywords.Add("Clear Marker", HandleClearMarker);
        voiceRecognitionKeywords.Add("Show MRI Scans", HandleMRI);
        voiceRecognitionKeywords.Add("Show Outline", HandleMRIOutlineOn);
        voiceRecognitionKeywords.Add("Hide Outline", HandleMRIOutlineOff);
        //voiceRecognitionKeywords.Add("Toggle Outline", HandleMRIOutline);
        voiceRecognitionKeywords.Add("Pin Menu", HandlePinMenu);
        voiceRecognitionKeywords.Add("UnPin Menu", HandleUnpinMenu);
        voiceRecognitionKeywords.Add("Show Structures", HandleStructuresMode);

        voiceRecognitionKeywords.Add("End Tutorial", HandleEndTutorial);
        voiceRecognitionKeywords.Add("Next", HandleNextChapter);

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

            if (brain != null)
            {
                if (!brain.GetComponent<StateAccessor>().CurrentlyInStudentMode()) keywordAction.Invoke();
            }

            if (SceneManager.GetActiveScene().name == TUTORIAL_SCENE_NAME)
            {
                keywordAction.Invoke();
            }

        }               
    }

    private void HandleStartRotate()
    {
        if (!brain.GetComponent<RotateStructures>().isRotating)
        {
            //This line was just for testing the voice command "Play"
            //GameObject.Find(buttonActionsToGameObjectName["Play"]).GetComponent<ButtonCommands>().OnSelect();

            GameObject.Find(buttonActionsToGameObjectName["Rotate"]).GetComponent<ButtonCommands>().OnSelect();
        }

        brain.GetComponent<RotateStructures>().StartRotate();
    }

    private void HandleStartRotateWalls()
    {
        GameObject.Find(buttonActionsToGameObjectName["Rotate Walls"]).GetComponent<RotateWallsButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Rotate Walls"]).GetComponent<ButtonCommands>().OnSelect();
        
    }

    private void HandleStartPlay()
    {
        GameObject.Find(buttonActionsToGameObjectName["Play"]).GetComponent<PlayButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Play"]).GetComponent<ButtonCommands>().OnSelect();

    }

    private void HandleSpeedUpPlayback()
    {
        GameObject.Find(buttonActionsToGameObjectName["Faster"]).GetComponent<SpeedUpButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Faster"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleSlowDownPlayback()
    {
        GameObject.Find(buttonActionsToGameObjectName["Slower"]).GetComponent<SlowDownButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Slower"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleSkipOne()
    {
        GameObject.Find(buttonActionsToGameObjectName["Skip One"]).GetComponent<SkipOneButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Skip One"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleBackOne()
    {
        GameObject.Find(buttonActionsToGameObjectName["Back One"]).GetComponent<BackOneButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Back One"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleSkipTen()
    {
        GameObject.Find(buttonActionsToGameObjectName["Skip Ten"]).GetComponent<SkipTenButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Skip Ten"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleBackTen()
    {
        GameObject.Find(buttonActionsToGameObjectName["Back Ten"]).GetComponent<BackTenButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Back Ten"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleStopRotate()
    {
        if (brain.GetComponent<RotateStructures>().isRotating)
        {
            GameObject.Find(buttonActionsToGameObjectName["Stop"]).GetComponent<ButtonCommands>().OnSelect();
        }

        brain.GetComponent<RotateStructures>().StopRotate();
    }

    private void HandleScaleUp()
    {
        GameObject.Find(buttonActionsToGameObjectName["Scale Up"]).GetComponent<ButtonCommands>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Scale Up"]).GetComponent<ScaleUpButtonAction>().OnSelect();

    }

    private void HandleScaleDown()
    {
        GameObject.Find(buttonActionsToGameObjectName["Scale Down"]).GetComponent<ButtonCommands>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Scale Down"]).GetComponent<ScaleDownButtonAction>().OnSelect();
    }

    private void HandleExplode()
    {
        if (brain.GetComponent<ExplodingCommands>().lastState == ExplodingCommands.ExplodingState.ExplodingIn)
        {
            GameObject.Find(buttonActionsToGameObjectName["Expand"]).GetComponent<ButtonCommands>().OnSelect();
        }
        
        brain.GetComponent<ExplodingCommands>().TryToExplode();
    }

    private void HandleCollapse()
    {
        if (brain.GetComponent<ExplodingCommands>().lastState == ExplodingCommands.ExplodingState.ExplodingOut)
        {
            GameObject.Find(buttonActionsToGameObjectName["Collapse"]).GetComponent<ButtonCommands>().OnSelect();
        }

        brain.GetComponent<ExplodingCommands>().TryToCollapse();
    }

    private void HandleIsolate()
    {
        brain.GetComponent<IsolateStructures>().InitiateIsolationMode();
        GameObject.Find(buttonActionsToGameObjectName["Isolate"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleConcludeIsolate()
    {
        GameObject.Find(buttonActionsToGameObjectName["Go Back"]).GetComponent<ButtonCommands>().OnSelect();
        brain.GetComponent<IsolateStructures>().ConcludeIsolationMode();
    }

    private void HandleResetState()
    {
        brain.GetComponent<ResetState>().ResetEverything();
        ControlsUI.GetComponent<SubMenusManager>().EnableDefaultMenus();
        if (GameObject.Find(buttonActionsToGameObjectName["Reset"]) != null)
        {
            GameObject.Find(buttonActionsToGameObjectName["Reset"]).GetComponent<ResetButtonAction>().ResetUI();
        }
       
    }

    private void HandleResetAnchor()
    {
        GameObject.Find(buttonActionsToGameObjectName["Reposition"]).GetComponent<ButtonCommands>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Reposition"]).GetComponent<RepositionButtonAction>().OnSelect();
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
        brain.GetComponent<IsolateStructures>().TryToIsolate(brainStructureNameToGameObjectName[partName]);
        GameObject.Find(buttonActionsToGameObjectName[partName]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleRemoveBrainPart(string partName)
    {
        brain.GetComponent<IsolateStructures>().TryToReturnFromIsolate(brainStructureNameToGameObjectName[partName]);
        GameObject.Find(buttonActionsToGameObjectName[partName]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleEducationalRoom()
    {
        GameObject.Find(buttonActionsToGameObjectName["Educational Room"]).GetComponent<SwitchRoomAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Educational Room"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleMRIRoom()
    {
        GameObject.Find(buttonActionsToGameObjectName["MRI Room"]).GetComponent<SwitchRoomAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["MRI Room"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleFMRIRoom()
    {
        GameObject.Find(buttonActionsToGameObjectName["fMRI Room"]).GetComponent<SwitchRoomAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["fMRI Room"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleBrainCellRoom()
    {
        GameObject.Find(buttonActionsToGameObjectName["Brain Cell Room"]).GetComponent<SwitchRoomAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Brain Cell Room"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleShowMicroglia()
    {
        GameObject.Find(buttonActionsToGameObjectName["Microglia"]).GetComponent<SwitchBrainCellModeButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Microglia"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleShowChannelOne()
    {
        GameObject.Find(buttonActionsToGameObjectName["Channel 1"]).GetComponent<EnableObjectButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Channel 1"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleShowChannelTwo()
    {
        GameObject.Find(buttonActionsToGameObjectName["Channel 2"]).GetComponent<EnableObjectButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Channel 2"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void Update()
    {
    }

    //UNCOMMENT THIS FOR GAZE MARKER
    private void HandlePlaceMarker()
    {
        print("handleplacemarker called");
        if (gazeManager.Hit)
        {
            print("gazemanager hit");
            gazeManager.HitInfo.transform.GetComponent<GazeMarkerCommands>().OnSelect();
        }
    }

    //UNCOMMENT THIS FOR GAZE MARKER
    private void HandleClearMarker()
    {
        if (gazeManager.Hit)
        {
            gazeManager.HitInfo.transform.GetComponent<GazeMarkerCommands>().RemoveMarkerFromStructure();
        }
        GazeMarkerManager gmm = (GazeMarkerManager)FindObjectOfType(typeof(GazeMarkerManager));
        gmm.TryToRemoveGazeMarker();
    }

    private void HandleMRI()
    {
        MRIManager.Instance.ProcessMRIButtonAction();
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
        if (!MRIManager.Instance.IsOutlinedMRIImages())
        {
            GameObject.Find(buttonActionsToGameObjectName["MRI Outline"]).GetComponent<ButtonCommands>().OnSelect();
        }
        MRIManager.Instance.TurnOnMRIImageOutlines();
    }

    private void HandleMRIOutlineOff()
    {
        if (MRIManager.Instance.IsOutlinedMRIImages())
        {
            GameObject.Find(buttonActionsToGameObjectName["MRI Outline"]).GetComponent<ButtonCommands>().OnSelect();
        }
        MRIManager.Instance.TurnOffMRIImageOutlines();
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

    private void HandleNextChapter()
    {
        GameObject.Find(buttonActionsToGameObjectName["Next"]).GetComponent<NextButtonAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["Next"]).GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleEndTutorial()
    {
        GameObject.Find(buttonActionsToGameObjectName["End Tutorial"]).GetComponent<SwitchRoomAction>().OnSelect();
        GameObject.Find(buttonActionsToGameObjectName["End Tutorial"]).GetComponent<ButtonCommands>().OnSelect();
    }
}
