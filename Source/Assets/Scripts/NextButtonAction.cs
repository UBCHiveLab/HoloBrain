using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButtonAction : MonoBehaviour {

    public GameObject flowScriptObject;
	// Use this for initialization
	void Start () {
		
	}

    public void OnSelect() {
        flowScriptObject.GetComponent<Flow>().ShowNextChapterPages();
    }
}
