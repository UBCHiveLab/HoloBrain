using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStructure : MonoBehaviour {

    // Use this for initialization
    public GameObject[] structures;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect()
    {
        foreach (GameObject structure in structures)
        {
            structure.SetActive(!structure.activeSelf);
        }
    }
}
