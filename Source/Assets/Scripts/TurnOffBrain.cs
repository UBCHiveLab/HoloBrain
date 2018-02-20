using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffBrain : MonoBehaviour {

    GameObject brain;

	// Use this for initialization
	void Start () {

        brain = GameObject.Find("BrainParts");
        brain.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
