using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainSelectControl : MonoBehaviour {

    const string BRAIN_1 = "Brain";
    const string BRAIN_2 = "Brain2";
    const string CONE_1_TAG = "cone_1";
    const string CONE_2_TAG = "cone_2";

    private string __selectedBrain = BRAIN_1;

    private AudioSource soundFX;
    GameObject brain_1;
    GameObject brain_2;
    GameObject selectionCone_1;
    GameObject selectionCone_2;

    public string SelectedBrain {
        get {
            return __selectedBrain;
        }

        set {
            __selectedBrain = value;
        }
    }

	// Use this for initialization
	void Start () {
        soundFX = gameObject.GetComponent<AudioSource>();
        selectionCone_1 = GameObject.FindWithTag(CONE_1_TAG);
        selectionCone_2 = GameObject.FindWithTag(CONE_2_TAG);
        UpdateSelectedCone();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect() {
        ChangeSelectedBrain(); // similar format to ToggleRotate()
    }

    private void SendChangeSelectedBrainMessage() {
        // TODO: might not even have to send this since students don't care about what brain is selected
        throw new NotImplementedException();
    }

    public void UpdateSelectedCone() {
        selectionCone_1.SetActive(SelectedBrain == BRAIN_1);
        selectionCone_2.SetActive(SelectedBrain == BRAIN_2);
    }

    public void ToggleConeVisibility(bool isVisible)
    {
        if (isVisible)
        {
            UpdateSelectedCone();
        } else
        {
            selectionCone_1.SetActive(false);
            selectionCone_2.SetActive(false);
        }
    }

    private void ChangeSelectedBrain() {
        soundFX.Play();
        SelectedBrain = (SelectedBrain == BRAIN_1) ? (BRAIN_2) : (BRAIN_1);
        UpdateSelectedCone();
    }
}
