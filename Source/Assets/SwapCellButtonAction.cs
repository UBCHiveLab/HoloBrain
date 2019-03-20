using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCellButtonAction : MonoBehaviour {

    public GameObject cell;
    public GameObject cellContainer;
    private AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}

    void OnSelect()
    {
        if(audio != null)
        {
            audio.Play();
        }
        if (cell == null || cellContainer == null)
        {
            Debug.Log("need a cell and container here");
        }
        else
        {
            foreach(Transform child in cellContainer.GetComponent<Transform>())
            {
                child.gameObject.SetActive(false);
            }
            cell.SetActive(true);
        }

        foreach(Transform cur in transform.parent)
        {
            ButtonAppearance appearance = cur.gameObject.GetComponent<ButtonAppearance>();
            if (appearance != null)
            {
                appearance.ResetButton();
            } else
            {
                cur.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
        }

        ButtonAppearance myAppearance = GetComponent<ButtonAppearance>();
        if(myAppearance != null)
        {
            myAppearance.SetButtonActive();
        } else
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
