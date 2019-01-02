using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOn : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
	}      void OnEnable()     {         obj.SetActive(true);     }
}
