using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NextButtonAction : CommandToExecute {

    public GameObject flowScriptObject;
	// Use this for initialization
	override public void Start () {
        base.Start();
	}

    override protected Action Command() {
        return delegate
        {
            flowScriptObject.GetComponent<Flow>().ShowNextChapterPages();
        }; 
    }
}
