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
            {"Exit Isolate", "exit-mode-icon" },
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
        voiceRecognitionKeywords.Add("Stop", HandleStopRotate);
        voiceRecognitionKeywords.Add("Scale Up", HandleScaleUp);
        voiceRecognitionKeywords.Add("Scale Down", HandleScaleDown);
        voiceRecognitionKeywords.Add("Expand", HandleExplode);
        voiceRecognitionKeywords.Add("Collapse", HandleCollapse);
        voiceRecognitionKeywords.Add("Show Isolate", HandleIsolate);
        voiceRecognitionKeywords.Add("Hide Isolate", HandleConcludeIsolate);
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
        RotateStructures rs = brain.GetComponent<RotateStructures>();
        GameObject target = GameObject.Find("rotate-stop");
        if (rs != null && target != null && rs.isRotating == false)
        {
            target.GetComponentsInChildren<RotateButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleStopRotate()
    {
        RotateStructures rs = brain.GetComponent<RotateStructures>();
        GameObject target = GameObject.Find("rotate-stop");
        if (rs != null && target != null && rs.isRotating == true)
        {
            target.GetComponentsInChildren<RotateButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleScaleUp()
    {
        brain.GetComponent<ScaleToggler>().ScaleUp();
    }

    private void HandleScaleDown()
    {
        brain.GetComponent<ScaleToggler>().ScaleDown();
    }

    private void HandleExplode()
    {
        GameObject target = GameObject.Find("Expand/Collapse");
        if(target != null)
        {
            ExplodeButtonAction component = target.GetComponentsInChildren<ExplodeButtonAction>()[0];
            if (component != null && component.gameObject.name == "Expand")
            {
                component.gameObject.SendMessage("OnSelect");
            }
        }
    }

    private void HandleCollapse()
    {
        GameObject target = GameObject.Find("Expand/Collapse");
        if (target != null)
        {
            ExplodeButtonAction component = target.GetComponentsInChildren<ExplodeButtonAction>()[0];
            if (component != null && component.gameObject.name == "Collapse")
            {
                component.gameObject.SendMessage("OnSelect");
            }
        }
    }

    private void HandleIsolate()
    {
        //brain.GetComponent<IsolateStructures>().InitiateIsolationMode(); original isolation doesnt work with restructure, need to fix it
        //these two OnSelects will make the menu state ready to do isolate
        GameObject.Find("Educational").GetComponent<ButtonCommands>().OnSelect();
        GameObject.Find("Isolate").GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleConcludeIsolate()
    {
        //brain.GetComponent<IsolateStructures>().ConcludeIsolationMode();
    }

    private void HandleResetState()
    {
        brain.GetComponent<ResetState>().ResetEverything();
    }

    private void HandleResetAnchor()
    {
        brain.GetComponent<HologramPlacement>().ResetStage();
    }

    private void HandleAddAll()
    {
        brain.GetComponent<IsolateStructures>().AddAllParts();
    }

    private void HandleRemoveAll()
    {
        brain.GetComponent<IsolateStructures>().RemoveAllParts();
    }

    private void HandleEducationalRoom()
    {
        GameObject.Find("Educational").GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleMRIRoom()
    {
        GameObject.Find("MRI").GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleFMRIRoom()
    {
        GameObject.Find("fMRI").GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandleBrainCellRoom()
    {
        GameObject.Find("Cell").GetComponent<ButtonCommands>().OnSelect();
    }

    private void HandlePinMenu()
    {
        if (!GameObject.Find("ControlsUI").GetComponent<ControlsUIManager>().GetMenuPinState())
        {
            GameObject.Find("Pin/UnPin").GetComponentsInChildren<PinButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleUnpinMenu()
    {
        if (GameObject.Find("ControlsUI").GetComponent<ControlsUIManager>().GetMenuPinState())
        {
            GameObject.Find("Pin/UnPin").GetComponentsInChildren<PinButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleStartPlay()
    {
        GameObject target = GameObject.Find("Play/Pause");
        if (target != null)
        {
            target.GetComponentsInChildren<PlayButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleSpeedUpPlayback()
    {
        GameObject target = GameObject.Find("speed");
        if(target != null)
        {
            target.GetComponentsInChildren<SpeedUpButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleSlowDownPlayback()
    {
        GameObject target = GameObject.Find("speed");
        if (target != null)
        {
            target.GetComponentsInChildren<SlowDownButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleSkipOne()
    {
        GameObject target = GameObject.Find("skip");
        if (target != null)
        {
            target.GetComponentsInChildren<SkipOneButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleBackOne()
    {
        GameObject target = GameObject.Find("skip");
        if (target != null)
        {
            target.GetComponentsInChildren<BackOneButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleSkipTen()
    {
        GameObject target = GameObject.Find("skip");
        if (target != null)
        {
            target.GetComponentsInChildren<SkipTenButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
    }

    private void HandleBackTen()
    {
        GameObject target = GameObject.Find("skip");
        if (target != null)
        {
            target.GetComponentsInChildren<BackTenButtonAction>()[0].gameObject.SendMessage("OnSelect");
        }
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

    private void HandleShowMicroglia()
    {
        GameObject target = GameObject.Find("microglia");
        if (target != null)
        {
            target.SendMessage("OnSelect");
        }
    }

    private void HandleShowChannelOne()
    {
        GameObject target = GameObject.Find("channel1");
        if(target != null)
        {
            target.SendMessage("OnSelect");
        }
    }

    private void HandleShowChannelTwo()
    {
        GameObject target = GameObject.Find("channel2");
        if (target != null)
        {
            target.SendMessage("OnSelect");
        }
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
