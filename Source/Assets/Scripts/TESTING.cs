
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTING : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Button>().onClick.AddListener(CLicked);
	}
	
	public void CLicked ()
    {
        Debug.Log("clicekd");
    }
}
