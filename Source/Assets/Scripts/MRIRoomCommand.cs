using HoloToolkit.Unity;
using System;
using UnityEngine;
using HolobrainConstants;

public class MRIRoomCommand : CommandToExecute {

    public GameObject MRI;
    public GameObject fMRI;
    public GameObject CELL;
    public GameObject DTI;
    private AudioSource sound;
	// Use this for initialization

    override protected Action Command()
    {
        return delegate
        {
            GameObject Brain = GameObject.Find(Names.BRAIN_GAMEOBJECT_NAME);
            if (!Brain.GetComponent<IsolateStructures>().CurrentlyInIsolationModeOrIsolating())
            {
                Brain.GetComponent<ExplodingCommands>().ResetExplode();
                if (MRI != null)
                {
                    ((MoveClippingPlane)(MRI.GetComponentInChildren(typeof(MoveClippingPlane), true))).resetPlanePosition();
                    ((MoveClippingPlane)(MRI.GetComponentInChildren(typeof(MoveClippingPlane), true))).TurnOnClipping();
                    foreach (Renderer renderer in MRI.transform.GetComponentsInChildren<Renderer>(true))
                    {
                        renderer.enabled = true;
                    }
                    foreach (Collider collider in MRI.transform.GetComponentsInChildren<Collider>(true))
                    {
                        collider.enabled = true;
                    }
                    foreach (Canvas canvas in MRI.transform.GetComponentsInChildren<Canvas>(true))
                    {
                        canvas.enabled = true;
                    }
                }
                if (fMRI != null)
                {
                    foreach (Renderer renderer in fMRI.transform.GetComponentsInChildren<Renderer>(true))
                    {
                        renderer.enabled = false;
                    }
                    foreach (Collider collider in fMRI.transform.GetComponentsInChildren<Collider>(true))
                    {
                        collider.enabled = false;
                    }
                }
                if (CELL != null)
                {
                    foreach (Renderer renderer in CELL.transform.GetComponentsInChildren<Renderer>(true))
                    {
                        renderer.enabled = false;
                    }
                    foreach (Collider collider in CELL.transform.GetComponentsInChildren<Collider>(true))
                    {
                        collider.enabled = false;
                    }
                }
                if (DTI != null)
                {
                    foreach (Renderer renderer in DTI.transform.GetComponentsInChildren<Renderer>(true))
                    {
                        renderer.enabled = false;
                    }
                    foreach (Collider collider in DTI.transform.GetComponentsInChildren<Collider>(true))
                    {
                        collider.enabled = false;
                    }
                }
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Structure"))
                {
                    foreach (Collider collider in cur.transform.GetComponentsInChildren<Collider>(true))
                    {
                        if (cur.name != "Cortex")
                        {
                            collider.enabled = true;
                        }
                        else
                        {
                            collider.enabled = false;
                        }
                    }
                    foreach (Renderer renderer in cur.transform.GetComponentsInChildren<Renderer>(true))
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
            }
        };
    }
}
