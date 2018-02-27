using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIIconManager : Singleton<MRIIconManager>
{
    private List<ButtonSwapFeedback> MRIWallIcons = new List<ButtonSwapFeedback>();
    private ButtonSwapFeedback activeIcon;

	// Use this for initialization
   void Start () {

		foreach (ButtonSwapFeedback icon in GetComponentsInChildren<ButtonSwapFeedback>())
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
        foreach (ButtonSwapFeedback icon in MRIWallIcons)
        {
            Debug.Log(icon);
            icon.ResetButtonState();
        }
    }
}
