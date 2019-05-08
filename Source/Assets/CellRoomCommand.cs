using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellRoomCommand : MonoBehaviour
{


    public GameObject MRI;
    public GameObject fMRI;
    public GameObject CELL;
    public GameObject DTI;
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<ButtonCommands>().AddCommand(HideOthers());
    }

    Action HideOthers()
    {
        return delegate
        {
            if (MRI != null)
            {
                foreach (Renderer renderer in MRI.transform.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = false;
                }
                foreach(Collider collider in MRI.transform.GetComponentsInChildren<Collider>(true))
                {
                    collider.enabled = false;
                }
            }
            if (fMRI != null)
            {
                foreach (Renderer renderer in fMRI.transform.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = false;
                }
            }
            if (DTI != null)
            {
                foreach (Renderer renderer in DTI.transform.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = false;
                }
            }
            if (CELL != null)
            {
                foreach (Renderer renderer in CELL.transform.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = true;
                }
            }
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Structure"))
            {
                foreach(Collider collider in cur.transform.GetComponentsInChildren<Collider>(true))
                {
                    collider.enabled = false;
                }
                foreach (Renderer renderer in cur.transform.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = false;
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
