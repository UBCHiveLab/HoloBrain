using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIEnable : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MRIManager.Instance.ProcessMRIButtonAction();
        //StateAccessor.Instance.ChangeMode(StateAccessor.Mode.MRI);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
