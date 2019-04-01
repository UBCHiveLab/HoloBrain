using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fMRIRoomCommand : MonoBehaviour
{

    public GameObject MRI;
    public GameObject fMRI;
    public GameObject CELL;
    public GameObject MRIVolume;
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
                foreach (Renderer renderer in MRI.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
            if (fMRI != null)
            {
                foreach (Renderer renderer in fMRI.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }
            if(CELL != null)
            {
                foreach (Renderer renderer in CELL.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
            if (MRIVolume != null)
            {
                foreach (Renderer renderer in MRIVolume.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Structure"))
            {
                foreach(Collider collider in cur.GetComponentsInChildren<Collider>())
                {
                    collider.enabled = false;
                }
                foreach (Renderer renderer in cur.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
        };
    }
}
