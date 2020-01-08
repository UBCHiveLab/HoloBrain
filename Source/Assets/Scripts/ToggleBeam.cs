using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToggleBeam : MonoBehaviour {

    private GameObject line;
	// Use this for initialization
	void Start () {
        line = GameObject.Find("beamHolder");
        line.SetActive(false);
        gameObject.GetComponent<ButtonCommands>().AddCommand(toggleBeam());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Action toggleBeam()
    {
        return delegate
        {
            line.SetActive(!line.activeSelf);
        };
    }
}
