using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNextChapter : MonoBehaviour {

    public GameObject flowScriptObject;
	// Use this for initialization
	void Start () {
		
	}

    void OnSelect() {
        flowScriptObject.GetComponent<Flow>().ShowNextChapterPages();
    }
}
