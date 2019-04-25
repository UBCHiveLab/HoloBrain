using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIRoomCommand : MonoBehaviour {

    public GameObject MRI;
    public GameObject fMRI;
    public GameObject CELL;
    private AudioSource sound;
	// Use this for initialization
	void Start () {
        sound = GetComponent<AudioSource>();
        gameObject.GetComponent<ButtonCommands>().AddCommand(HideOthers());
	}

    Action HideOthers()
    {
        return delegate
        {
            sound.Play();
            if (MRI != null)
            {
                foreach (Renderer renderer in MRI.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }
            if (fMRI != null)
            {
                foreach (Renderer renderer in fMRI.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
            if (CELL != null)
            {
                foreach (Renderer renderer in CELL.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }
            }
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Structure"))
            {
                foreach(Collider collider in cur.GetComponentsInChildren<Collider>())
                {
                    if(cur.name != "Cortex")
                    {
                        collider.enabled = true;
                    } else
                    {
                        collider.enabled = false;
                    }

                }
                foreach (Renderer renderer in cur.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }
            }
            foreach (ButtonAppearance button in transform.parent.GetComponentsInChildren<ButtonAppearance>())
            {
                if (button.name != gameObject.name)
                {
                    button.ResetButton();
                }
            }
            GetComponent<ButtonAppearance>().SetButtonActive();
        };
    }
}
