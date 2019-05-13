using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGazeMarker : MonoBehaviour {

    public GameObject marker;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartGaze()
    {
        foreach(Renderer renderer in marker.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
    }

    public void OnEndGaze()
    {
        foreach (Renderer renderer in marker.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }
    }
}
