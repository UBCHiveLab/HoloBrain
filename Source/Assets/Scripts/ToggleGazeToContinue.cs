using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGazeToContinue : MonoBehaviour {

    public GameObject nextButton;
    public bool onOrOff;

    // Use this for initialization
    void Start () {
        if (onOrOff)
        {
            nextButton.GetComponent<TriggerNextOnGaze>().enabled = true;
        }         else
        {
            nextButton.GetComponent<TriggerNextOnGaze>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()     {              }
}
