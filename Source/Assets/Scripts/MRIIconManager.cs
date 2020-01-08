using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIIconManager : Singleton<MRIIconManager>
{
    private List<ButtonSwapHighlightFeedback> MRIWallIcons = new List<ButtonSwapHighlightFeedback>();
    private ButtonSwapHighlightFeedback activeIcon;

	// Use this for initialization
   void Start () {

		foreach (ButtonSwapHighlightFeedback icon in GetComponentsInChildren<ButtonSwapHighlightFeedback>())
        {
            MRIWallIcons.Add(icon);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DeselectAll ()
    {
        Debug.Log("In deselect all, " + MRIWallIcons);
        foreach (ButtonSwapHighlightFeedback icon in MRIWallIcons)
        {
            Debug.Log(icon);
            icon.ResetButtonState();
        }
    }
}
