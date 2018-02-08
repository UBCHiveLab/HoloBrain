using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareButtonAction : MonoBehaviour {

    public GameObject changeBrainButton;
    public GameObject selectedBrainIcon;

    private bool isInCompareMode;

	// Use this for initialization
	void Start () {
        isInCompareMode = false;
        changeBrainButton.GetComponent<SpriteRenderer>().enabled = true;
        changeBrainButton.GetComponent<BoxCollider>().enabled = true;
        selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = true;
        selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
    }
	
	public void OnSelect ()
    {
        if (!isInCompareMode)
        {
            changeBrainButton.GetComponent<SpriteRenderer>().enabled = false;
            changeBrainButton.GetComponent<BoxCollider>().enabled = false;
            selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = false;
            selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
        } else
        {
            changeBrainButton.GetComponent<SpriteRenderer>().enabled = true;
            changeBrainButton.GetComponent<BoxCollider>().enabled = true;
            selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = true;
            selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
        }
        isInCompareMode = !isInCompareMode;
    }
}
