using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortRenderers : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.sortingLayerName = "BrainLayer";
            renderer.sortingOrder = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
