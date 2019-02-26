using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMRIButtonAction : MonoBehaviour {

    public MRIInteractions slice;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect()
    {
        MRIManager.Instance.ReturnFromDisplaySingleMRI();
        //MRIIconManager.Instance.DeselectAll();
        //gameObject.GetComponent<ButtonSwapFeedback>().ToggleButtonImage();
        slice.SelectMRI();
    }
}
