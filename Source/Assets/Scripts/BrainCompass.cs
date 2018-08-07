using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainCompass : MonoBehaviour {

    public GameObject arrow;
    public GameObject brain;
    private Renderer rend;

	// Use this for initialization
	void Start () {
        rend = brain.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!rend.isVisible)
        {
            arrow.GetComponent<SpriteRenderer>().enabled = true;
            //arrow.transform.LookAt(brain.transform, Vector3.forward);
        }
        else
        {
            arrow.GetComponent<SpriteRenderer>().enabled = false;
        }
	}
}
