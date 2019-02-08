using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

// this script should be start disabled in Unity
public class TriggerNextOnGaze : MonoBehaviour {

    public GameObject nextButton;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnStartGaze(){
        if (this.GetComponent<TriggerNextOnGaze>().enabled)
            nextButton.GetComponent<NextButtonAction>().OnSelect();
    }

    void OnEndGaze()
    {
    }

}
