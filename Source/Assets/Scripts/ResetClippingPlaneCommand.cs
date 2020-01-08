using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetClippingPlaneCommand: MonoBehaviour {

    public MoveClippingPlane MoveClippingPlane;
    private ButtonCommands commands;
	// Use this for initialization
	void Start () {
        commands = gameObject.GetComponent<ButtonCommands>();
        if(commands != null)
        {
            commands.AddCommand(resetClipPlaneAction());
        }
	}

    private Action resetClipPlaneAction()
    {
        return delegate
        {
            MoveClippingPlane.resetPlanePosition();
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
