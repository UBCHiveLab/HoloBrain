using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMRI : MonoBehaviour {
    private Renderer mriRenderer;
	// Use this for initialization
	void Start () {
        mriRenderer = transform.Find("MRIImage").GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnSelect()
    {
        if(mriRenderer.enabled)
        {
            mriRenderer.enabled = false;
        } else
        {
            mriRenderer.enabled = true;
        }
    }
}
