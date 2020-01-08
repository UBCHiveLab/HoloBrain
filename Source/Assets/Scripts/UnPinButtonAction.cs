using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HolobrainConstants;
using UnityEngine.EventSystems;
using HoloToolkit.Unity.InputModule;

public class UnPinButtonAction : CommandToExecute {

    public GameObject pinButton;
	// Use this for initialization
    protected override Action Command()
    {
        return delegate
        {
            EventSystem eventSystem = GameObject.Find(Names.EVENT_SYSTEM_NAME).GetComponent<EventSystem>();
            if(!transform.GetComponentInParent<ControlsUIManager>().GetMenuPinState())
            {
                pinButton.GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
            }
        };
    }
}
