using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwapCellButtonAction : CommandToExecute {

    public GameObject cell;
    public GameObject cellContainer;

    override protected Action Command()
    {
        return delegate
        {
            if (cell == null || cellContainer == null)
            {
                Debug.Log("need a cell and container here");
            }
            else
            {
                foreach (Transform child in cellContainer.GetComponent<Transform>())
                {
                    child.gameObject.SetActive(false);
                }
                cell.SetActive(true);
            }

            foreach (Transform cur in transform.parent)
            {
                ButtonAppearance appearance = cur.gameObject.GetComponent<ButtonAppearance>();
                if (appearance != null)
                {
                    appearance.ResetButton();
                }
                else
                {
                    cur.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }

            ButtonAppearance myAppearance = GetComponent<ButtonAppearance>();
            if (myAppearance != null)
            {
                myAppearance.SetButtonActive();
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
            }
        };
    }
}
