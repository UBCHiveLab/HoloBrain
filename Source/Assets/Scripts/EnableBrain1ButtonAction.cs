using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBrain1ButtonAction : MonoBehaviour
{

    private const string BRAIN_PARTS_1_NAME = "BrainParts";

    private GameObject selectBrainControlGameObject;

    // Use this for initialization
    void Start ()
    {
        selectBrainControlGameObject = GameObject.Find("BrainSelectControl");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect()
    {
        //do the action
        Debug.Log("ChangeBrain button selected");
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().OnSelect();
    }
}
