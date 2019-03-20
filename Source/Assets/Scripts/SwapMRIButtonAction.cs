using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMRIButtonAction : MonoBehaviour {

    public MRIInteractions slice;
    private GameObject container;

	// Use this for initialization
	void Start () {
        container = GameObject.Find("MRIRoomInteractions");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect()
    {
        //MRIManager.Instance.ReturnFromDisplaySingleMRI();
        //MRIIconManager.Instance.DeselectAll();
        //gameObject.GetComponent<ButtonSwapFeedback>().ToggleButtonImage();
        //slice.SelectMRI();
        foreach(SwapMRIButtonAction current in container.GetComponentsInChildren<SwapMRIButtonAction>())
        {
            current.gameObject.GetComponent<ButtonAppearance>().ResetButton();
        }
        GetComponent<ButtonAppearance>().SetButtonActive();
    }
}
