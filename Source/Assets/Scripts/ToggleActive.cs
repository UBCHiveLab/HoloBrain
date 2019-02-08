using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour {

    public bool onOrOff = true;
    public GameObject obj;

	// Use this for initialization
	void Start () {
	}      void OnEnable()     {         if (onOrOff)
        {
            obj.SetActive(true);         }         else
        {
            obj.SetActive(false);         }     }
}
