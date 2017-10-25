using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainSelectControl : MonoBehaviour {

    const string BRAIN_1 = "Brain";
    const string BRAIN_2 = "Brain2";

    private string __selectedBrain = BRAIN_1;

    private AudioSource soundFX;

    public string SelectedBrain
    {
        get
        {
            return __selectedBrain;
        }

        set
        {
            __selectedBrain = value;
        }
    }

	// Use this for initialization
	void Start () {
        soundFX = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect()
    {
        //Debug.Log("Inside BrainSelectControl");
        //SendChangeSelectedBrainMessage();
        ChangeSelectedBrain(); // similar format to ToggleRotate()
    }

    private void SendChangeSelectedBrainMessage()
    {
        // TODO: might not even have to send this since students don't care about what brain is selected
        throw new NotImplementedException();
    }

    private void ChangeSelectedBrain()
    {
        //Debug.Log("Inside ChangeSelectedBrain function()");
        soundFX.Play();
        SelectedBrain = (SelectedBrain == BRAIN_1) ? (BRAIN_2) : (BRAIN_1);
        Debug.Log("======================================Selected Brain changed to Brain" + SelectedBrain);
    }
}
