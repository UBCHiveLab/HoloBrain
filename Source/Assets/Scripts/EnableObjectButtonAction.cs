using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectButtonAction : MonoBehaviour {

    public GameObject gameObject;
    private AudioSource audio;
    private bool isActive;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        isActive = gameObject.activeSelf;
	}

    public void OnSelect()
    {
        audio.Play();
        isActive = !isActive;
        gameObject.SetActive(isActive);
        
    }
}
