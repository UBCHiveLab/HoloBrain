using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainSelectControl : MonoBehaviour {

    const string BRAIN_1 = "Brain";
    const string BRAIN_2 = "Brain2";
    const string BRAIN_1_TAG = "brain_1";
    const string BRAIN_2_TAG = "brain_2";

    private string __selectedBrain = BRAIN_1;

    private AudioSource soundFX;
    GameObject brain_1;
    GameObject brain_2;
    Behaviour halo_1;
    Behaviour halo_2;

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
        halo_1 = (Behaviour)GameObject.FindWithTag(BRAIN_1_TAG).GetComponent("Halo");
        halo_2 = (Behaviour)GameObject.FindWithTag(BRAIN_2_TAG).GetComponent("Halo");
        UpdateHaloEffect();
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

    public void UpdateHaloEffect() {
        Debug.Log(brain_2);
        halo_1.enabled = SelectedBrain == BRAIN_1;
        halo_2.enabled = SelectedBrain == BRAIN_2;
    }

    private void ChangeSelectedBrain()
    {
        //Debug.Log("Inside ChangeSelectedBrain function()");
        soundFX.Play();
        SelectedBrain = (SelectedBrain == BRAIN_1) ? (BRAIN_2) : (BRAIN_1);
        UpdateHaloEffect();
        //Debug.Log("======================================Selected Brain changed to Brain" + SelectedBrain);
    }
}
