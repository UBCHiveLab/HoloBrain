using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIRoomCommand : MonoBehaviour {

    public GameObject MRI;
    public GameObject fMRI;
    public GameObject CELL;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<ButtonCommands>().AddCommand(HideOthers());
	}

    Action HideOthers()
    {
        return delegate
        {
            if (MRI != null)
            {
                MRI.SetActive(true);
            }
            if (fMRI != null)
            {
                fMRI.SetActive(false);
            }
            if (CELL != null)
            {
                CELL.SetActive(false);
            }
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Structure"))
            {
                foreach (Renderer renderer in cur.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }
        };
    }
}
