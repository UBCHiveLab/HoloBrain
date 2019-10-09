using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIEvents : MonoBehaviour {

    public delegate void MRIHighlightDelegate(string mriName);
    public event MRIHighlightDelegate HighlightMRIEvent;

	// Use this for initialization
	public void publishMRIHighlightEvent(string mriName)
    {
        if(HighlightMRIEvent != null)
        {
            HighlightMRIEvent(mriName);
        }
    }
}
