// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.InputModule;
using HolobrainConstants;
using UnityEngine.EventSystems;

public class VoiceControl : MonoBehaviour {
    private const string BRAIN_PARTS_NAME = "Brain";
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


    private GameObject controlsUI;

    private GameObject mriCollection;
    private HTGazeManager gazeManager;


    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> voiceRecognitionKeywords;
    public static Dictionary <string, string> brainStructureNameToGameObjectName;
    private Dictionary<string, string> buttonActionsToGameObjectName;
    private MuteButtonAction muteButton;
    private EventSystem eventSystem;
    
    private DataRecorder dataRecorder;

    // Use this for initialization
    void Start() {
        // Referencing game objects to access their scripts
        brain = GameObject.Find(BRAIN_GAME_OBJECT_NAME);
        brainStructures = GameObject.Find(BRAIN_PARTS_NAME);
        crossfadeSlider = GameObject.Find("CrossfadeSlider");
        muteButton = GameObject.Find("mute").GetComponent<MuteButtonAction>();
        eventSystem = GameObject.Find("PreLoad/InputManager/EventSystem").GetComponent<EventSystem>();
        dataRecorder = GameObject.Find("PreLoad").GetComponent<DataRecorder>();


        fmriBrain = GameObject.Find(FMRI_BRAIN_NAME);
        //fmriBrain.SetActive(false);


        // mriScans = GameObject.Find(MRI_SCANS);
        controlsUI = GameObject.Find(Control_UI);

        gazeManager = GameObject.Find(HOLOGRAM_COLLECTION_NAME).GetComponent<HTGazeManager>();
        mriScans = GameObject.Find(MRI_SCANS_NAME);
        mriCollection = GameObject.Find(MRI_COLLECTION_NAME);

        voiceRecognitionKeywords = new Dictionary<string, Action>();

        foreach (var item in Names.GetStructureNames())
        {
            voiceRecognitionKeywords.Add("Add " + item, () => { HandleAddBrainPart(item); });
            voiceRecognitionKeywords.Add("Remove " + item, () => { HandleRemoveBrainPart(item); });
        }

        //map voice commands to the corresponding button name
        buttonActionsToGameObjectName = new Dictionary<string, string>
        {
            { "Rotate", "rotate-stop" },
            //{ "Rotate Walls", "rotate-walls-icon" },
            { "Stop", "rotate-stop" },
            { "Expand", "expand-collapse" },
            { "Collapse", "expand-collapse" },
           // { "Reset", "reset-icon" },
            {"Scale Up", "scale-up" },
            {"Scale Down", "scale-down" },
            {"Isolate", "menu" },
            {"Hide Isolate", "menu" },
            {"Reposition", "reposition-icon" },
            {"Add All", "menu" },
            {"Remove All", "menu" },
            { "Microglia", "menu" },
           { "Channel 1", "menu" },
            { "Channel 2", "menu" },
            { "MRI", "mri-icon" },
            { "MRI Outline", "show-colour-icon" },
            { "Pin", "pin-unpin" },
            { "Structures", "structures-icon" },
             //New Voice Commands
            { "Play", "menu" },
            { "Pause", "menu" },
            { "Faster", "menu" },
            { "Slower", "menu" },
            { "Skip One", "menu" },
            { "Skip Ten", "menu" },
            { "Back One", "menu" },
           { "Back Ten", "menu" },
            { "Educational Room", "rooms"},
            { "MRI Room", "rooms"},
          { "fMRI Room", "rooms"},
            { "Brain Cell Room", "rooms"},
            { "DTI Room", "rooms" },
          { "End Tutorial", "SkipButton"},
            { "Next", "NextButton"},
            {"Locate", "Locate" }
        };

        voiceRecognitionKeywords.Add("Rotate", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Rotate"]), () =>
        {
            var rs = brain.GetComponent<RotateStructures>();
            return rs != null && rs.isRotating == false;
        }, typeof(RotateButtonAction)));
        voiceRecognitionKeywords.Add("Stop", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Stop"]), () =>
        {
            var rs = brain.GetComponent<RotateStructures>();
            return rs != null && rs.isRotating == true;
        }, typeof(RotateButtonAction)));

        voiceRecognitionKeywords.Add("Scale Up", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Scale Up"])));
        voiceRecognitionKeywords.Add("Scale Down", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Scale Down"])));

        voiceRecognitionKeywords.Add("Expand", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Expand"]), () =>
        {
            var sa = brain.GetComponent<StateAccessor>();
            return sa != null && sa.AbleToTakeAnInteraction();
        }, typeof(ExplodeButtonAction), "Expand"));
        voiceRecognitionKeywords.Add("Collapse", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Collapse"]), () =>
        {
            var sa = brain.GetComponent<StateAccessor>();
            return sa != null && sa.AbleToTakeAnInteraction();
        }, typeof(ExplodeButtonAction), "Collapse"));

        voiceRecognitionKeywords.Add("Isolate", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Isolate"]), () => {
            var rs = brain.GetComponent<RotateStructures>();
            return rs != null && !rs.isRotating;
        }, typeof(IsolateModeButtonAction)));
        voiceRecognitionKeywords.Add("Hide Isolate", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Hide Isolate"]), () =>
        {
            var rs = brain.GetComponent<RotateStructures>();
            return rs != null && !rs.isRotating;
        }, typeof(IsolateExitButtonAction)));
        voiceRecognitionKeywords.Add("Reset", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Reset"])));
        voiceRecognitionKeywords.Add("Reposition", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Reposition"])));
        voiceRecognitionKeywords.Add("Add All", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Add All"]), typeof(IsolateButtonAction), buttonActionsToGameObjectName["Add All"]));
        voiceRecognitionKeywords.Add("Remove All", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Remove All"]), typeof(IsolateButtonAction), buttonActionsToGameObjectName["Remove All"]));
        // New Voice Commands

        voiceRecognitionKeywords.Add("Play", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Play"]), typeof(PlayButtonAction)));
        voiceRecognitionKeywords.Add("Pause", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Pause"])));
        voiceRecognitionKeywords.Add("Faster", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Faster"]), typeof(SpeedUpButtonAction)));
        voiceRecognitionKeywords.Add("Slower", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Slower"]), typeof(SlowDownButtonAction)));
        voiceRecognitionKeywords.Add("Skip One", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Skip One"]), typeof(SkipOneButtonAction)));
        voiceRecognitionKeywords.Add("Back One", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Skip One"]), typeof(BackOneButtonAction)));
        voiceRecognitionKeywords.Add("Skip Ten", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Skip Ten"]), typeof(SkipTenButtonAction)));
        voiceRecognitionKeywords.Add("Back Ten", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Back Ten"]), typeof(BackTenButtonAction)));

        voiceRecognitionKeywords.Add("Educational Room", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Educational Room"]), typeof(EduRoomCommand)));
        voiceRecognitionKeywords.Add("FMRI Room", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["fMRI Room"]), typeof(fMRIRoomCommand)));
        voiceRecognitionKeywords.Add("Functional MRI Room", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["fMRI Room"]), typeof(fMRIRoomCommand)));
        voiceRecognitionKeywords.Add("MRI Room", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["MRI Room"]), typeof(MRIRoomCommand)));
        voiceRecognitionKeywords.Add("MRI Scan Room", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["MRI Room"]), typeof(MRIRoomCommand)));
        voiceRecognitionKeywords.Add("Brain Cell Room", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Brain Cell Room"]), typeof(CellRoomCommand)));

        voiceRecognitionKeywords.Add("Show Microglia", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Microglia"]), typeof(SwapCellButtonAction), "microglia"));
        voiceRecognitionKeywords.Add("Show Channel One", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Channel 1"]), typeof(SwapCellButtonAction), "channel1"));
        voiceRecognitionKeywords.Add("Show Channel Two", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Channel 2"]), typeof(SwapCellButtonAction), "channel2"));

        //UNCOMMENT THIS FOR GAZE MARKER
        //voiceRecognitionKeywords.Add("Place Marker", HandlePlaceMarker);
        //voiceRecognitionKeywords.Add("Clear Marker", HandleClearMarker);
        //voiceRecognitionKeywords.Add("Show MRI Scans", HandleMRI); //commented because mris now show in mri room only
        //voiceRecognitionKeywords.Add("Show Outline", HandleMRIOutlineOn); // mri ouline toggle functionaly disabled for now (MRIManager needs refactoring or splitting up)
        //voiceRecognitionKeywords.Add("Hide Outline", HandleMRIOutlineOff);
        //voiceRecognitionKeywords.Add("Toggle Outline", HandleMRIOutline);
        voiceRecognitionKeywords.Add("Pin Menu", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Pin"]), () =>
        {
            var cu = controlsUI.GetComponent<ControlsUIManager>();
            return cu != null && !cu.GetMenuPinState();
        }, typeof(PinButtonAction)));
        voiceRecognitionKeywords.Add("UnPin Menu", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Pin"]), () => {
            var cu = controlsUI.GetComponent<ControlsUIManager>();
            return cu != null && cu.GetMenuPinState();
        }, typeof(PinButtonAction)));
        //voiceRecognitionKeywords.Add("Show Structures", HandleStructuresMode);

        //voiceRecognitionKeywords.Add("End Tutorial", HandleEndTutorial);
        //voiceRecognitionKeywords.Add("Next", HandleNextChapter);

        voiceRecognitionKeywords.Add("Locate", HandleCommand(GameObject.Find(buttonActionsToGameObjectName["Locate"])));

        keywordRecognizer = new KeywordRecognizer(voiceRecognitionKeywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if (voiceRecognitionKeywords.TryGetValue(args.text, out keywordAction))
        {
            Debug.Log("voice keyword: " + args.text);

            if (brain != null)
            {
                if (!brain.GetComponent<StateAccessor>().CurrentlyInStudentMode()) keywordAction.Invoke();
            }
            if(dataRecorder != null)
            {
                //dataRecorder.QueueMessage("voice command " + args.text);
            }
            /*
            if (SceneManager.GetActiveScene().name == TUTORIAL_SCENE_NAME)
            {
                keywordAction.Invoke();
            }*/
        }               
    }

    private void executeClick(GameObject g)
    {
        g.GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
    }

    //use when you just want to click surface, either found or not
    private Action HandleCommand(GameObject target)
    {
        return delegate
        {
            if (target != null)
            {
                executeClick(target);
            }
        };
    }

    //use when the click should only happen on precondition
    private Action HandleCommand(GameObject target, Func<bool> pred)
    {
        return delegate
        {
            if(pred() && target != null)
            {
                executeClick(target);
            }
        };
    }

    //use when click should happen on precondition, and to a child that has type component
    private Action HandleCommand(GameObject target, Func<bool> pred, System.Type t)
    {
        return delegate
        {
            if (pred() && target != null)
            {
                var btn = target.GetComponentsInChildren(t)[0];
                if (btn != null)
                {
                    executeClick(btn.gameObject);
                }
            }
        };
    }

    //overloaded to check gameobject name before clicking (cant be part of pred because it requires component t in target to be found already)
    private Action HandleCommand(GameObject target, Func<bool> pred, Type t, string name)
    {
        return delegate
        {
            if (pred() && target != null)
            {
                var btns = target.GetComponentsInChildren(t);
                foreach (Component c in target.GetComponentsInChildren(t))
                {
                    if (c.name == name)
                    {
                        executeClick(c.gameObject);
                    }
                }
            }
        };
    }

    //when button of interest isnt on the surface level, but we dont care about precondition
    private Action HandleCommand(GameObject target, Type t)
    {
        return HandleCommand(target, delegate { return true; }, t);
    }

    //when button of interest isnt on the surface level and we want to check name, but we dont care about precondition
    private Action HandleCommand(GameObject target, Type t, string name)
    {
        return HandleCommand(target, delegate { return true; }, t, name);
    }


    private void HandleAddBrainPart(string partName)
    {
        if(brain.GetComponent<IsolateStructures>().CurrentlyInIsolationModeOrIsolating())
        {
            foreach (ButtonAppearance button in controlsUI.transform.GetComponentsInChildren<ButtonAppearance>(true))
            {
                if(button.name == partName)
                {
                    brain.GetComponent<IsolateStructures>().TryToIsolate(partName);
                    button.GetComponent<IsolateButtonAction>().SetButtonSelected(true);
                    button.SetButtonActive();
                }
            }
        }
    }

    private void HandleRemoveBrainPart(string partName)
    {
        if (brain.GetComponent<IsolateStructures>().CurrentlyInIsolationModeOrIsolating())
        {
            foreach (ButtonAppearance button in controlsUI.transform.GetComponentsInChildren<ButtonAppearance>(true))
            {
                if(button.name == partName)
                {
                    brain.GetComponent<IsolateStructures>().TryToReturnFromIsolate(partName);
                    button.GetComponent<IsolateButtonAction>().SetButtonSelected(false);
                    button.ResetButton();
                }
            }
        }
    }

    private void Update()
    {
    }

    //BELLOW HERE ARE UNUSED/UNLINKED COMMANDS

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

    private Action HandleMRI(GameObject target)
    {
        //GameObject.Find(buttonActionsToGameObjectName["MRI"])
        return delegate
        {
            MRIManager.Instance.ProcessMRIButtonAction();
            if (target != null)
            {
                target.GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
            }
            else
            {
                controlsUI.GetComponent<SubMenusManager>().ToggleMenuUI("mri-icon");
            }
        };
    }

    private Action HandleMRIOutlineOn(GameObject target)
    {
        //GameObject.Find(buttonActionsToGameObjectName["MRI Outline"])
        return delegate
        {
            if (!MRIManager.Instance.IsOutlinedMRIImages())
            {
                target.GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
            }
            MRIManager.Instance.TurnOnMRIImageOutlines();
        };
    }

    private Action HandleMRIOutlineOff(GameObject target)
    {
        //GameObject.Find(buttonActionsToGameObjectName["MRI Outline"])
        return delegate
        {
            if (MRIManager.Instance.IsOutlinedMRIImages())
            {
                target.GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
            }
            MRIManager.Instance.TurnOffMRIImageOutlines();
        };
    }

    private Action HandleStructuresMode(GameObject target)
    {
        //GameObject.Find(buttonActionsToGameObjectName["Structures"])
        return delegate
        {
            target.GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
        };
    }

    private void HandleNextChapter()
    {
        GameObject.Find(buttonActionsToGameObjectName["Next"]).GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
    }

    private void HandleEndTutorial()
    {
        GameObject.Find(buttonActionsToGameObjectName["End Tutorial"]).GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
    }
}
