using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToggleStructure : MonoBehaviour {

    // Use this for initialization
    public GameObject[] structures;
    private AudioSource sound;
    public bool activated = false;
	void Start () {
        sound = GetComponent<AudioSource>();
        GetComponent<ButtonCommands>().AddCommand(ToggleStructureAction());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Action ToggleStructureAction()
    {
        return delegate
        {
            Material mat;
            Color col;
            sound.Play();
            if (!activated) { 
                foreach(GameObject candidate in GameObject.FindGameObjectsWithTag("Structure")) {
                    foreach (Renderer renderer in candidate.GetComponentsInChildren<Renderer>())
                    {
                        mat = renderer.material;
                        col = mat.color;
                        //ShaderBlendMode.Instance.SetupMaterialWithBlendMode(mat, ShaderBlendMode.BlendMode.Transparent);
                        col.a = 0.1f;
                        mat.color = col;
                        renderer.material = mat;
                    }
                    foreach (Collider collider in candidate.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = false;
                    }
                    foreach (GameObject structure in structures)
                    {
                        if(candidate.name == structure.name)
                        {
                            foreach(Renderer renderer in structure.GetComponentsInChildren<Renderer>())
                            {
                                mat = renderer.material;
                                //ShaderBlendMode.Instance.SetupMaterialWithBlendMode(mat, ShaderBlendMode.BlendMode.Opaque);
                                col = mat.color;
                                col.a = 1.0f;
                                mat.color = col;
                                renderer.material = mat;
                            }
                            foreach(Collider collider in structure.GetComponentsInChildren<Collider>())
                            {
                                collider.enabled = true;
                            }
                        }
                    }
                }
                foreach(ToggleStructure cur in GameObject.Find("Isolate").GetComponentsInChildren<ToggleStructure>())
                {
                    cur.gameObject.GetComponent<ButtonAppearance>().ResetButton();
                    cur.activated = false;
                }
                gameObject.GetComponent<ButtonAppearance>().SetButtonActive();
                activated = true;
            } else
            {
                foreach (GameObject candidate in GameObject.FindGameObjectsWithTag("Structure"))
                {
                    foreach (Renderer renderer in candidate.GetComponentsInChildren<Renderer>())
                    {
                        mat = renderer.material;
                        col = candidate.GetComponent<HighlightAndLabelCommands>().defaultColour;
                        //ShaderBlendMode.Instance.SetupMaterialWithBlendMode(mat, ShaderBlendMode.BlendMode.Transparent);
                        mat.color = col;
                        renderer.material = mat;
                    }
                    foreach (Collider collider in candidate.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = true;
                    }
                }
                activated = false;
                foreach (ToggleStructure cur in GameObject.Find("Isolate").GetComponentsInChildren<ToggleStructure>())
                {
                    cur.gameObject.GetComponent<ButtonAppearance>().ResetButton();
                    cur.activated = false;
                }
            }
        };
    }
}
