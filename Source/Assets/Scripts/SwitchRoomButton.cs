using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoomButton : MonoBehaviour {

    public bool Educational;

    GameObject EduRoomInteractions;
    GameObject MRIroomInteractions;

    bool yes = true;

	// Use this for initialization
	void Start () {
        EduRoomInteractions = GameObject.Find("EduRoomInteractions");
        MRIroomInteractions = GameObject.Find("MRIroomInteractions");
	}

    public void OnSelect ()
    {
        EduRoomInteractions.SetActive(!yes);
        MRIroomInteractions.SetActive(yes);
        yes = !yes;
    }
}
