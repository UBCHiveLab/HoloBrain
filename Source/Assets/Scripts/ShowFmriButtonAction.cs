using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFmriButtonAction : MonoBehaviour {
    GameObject fmri;
	// Use this for initialization
	void Start () {
        fmri = GameObject.FindWithTag("fmri");
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnSelect()
    {
        fmri.GetComponent<MeshRenderer>().enabled = !fmri.GetComponent<MeshRenderer>().enabled;
    }
}
