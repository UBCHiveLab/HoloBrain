using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTIRoomCommand : CommandToExecute {

    public GameObject MRI;
    public GameObject fMRI;
    public GameObject CELL;
    public GameObject DTI;
    
    override protected Action Command()
    {
        return delegate
        {
            if (StateAccessor.Instance.ChangeMode(StateAccessor.Mode.Default))
            {
                if (DTI != null)
                {
                    foreach (Renderer renderer in DTI.transform.GetComponentsInChildren<Renderer>(true))
                    {
                        renderer.enabled = true;
                    }
                    foreach (Collider collider in DTI.transform.GetComponentsInChildren<Collider>(true))
                    {
                        collider.enabled = true;
                    }
                    foreach (Canvas canvas in MRI.transform.GetComponentsInChildren<Canvas>(true))
                    {
                        canvas.enabled = false;
                    }
                }
                if (MRI != null)
                {
                    foreach (Renderer renderer in MRI.transform.GetComponentsInChildren<Renderer>(true))
                    {
                        renderer.enabled = false;
                    }
                    foreach (Collider collider in MRI.transform.GetComponentsInChildren<Collider>(true))
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
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Structure"))
                {
                    foreach (Collider collider in cur.GetComponentsInChildren<Collider>(true))
                    {
                        collider.enabled = false;
                    }
                    foreach (Renderer renderer in cur.GetComponentsInChildren<Renderer>(true))
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
            }
        };
    }
}
