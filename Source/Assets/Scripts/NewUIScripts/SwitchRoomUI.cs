using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchRoomUI : MonoBehaviour
{
    public List<GameObject> Buttons;
    public List<GameObject> Elements;

    // This doesn't work because button onclick doesn't work

    // Note that button order must match element order
    // EX Education room button and UI canvas must both be in same index for both list
    // (ex: both at index 0)

    void Start()
    {
        if (Buttons.Count != Elements.Count)
        {
            throw new System.Exception("Amount of buttons and elements are not equal");
        }

        RegisterClickToElement();
    }

    private void RegisterClickToElement()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].GetComponent<Button>().onClick.AddListener(delegate { SwitchToUI(Elements[i]); });
        }
    }

    private void SwitchToUI(GameObject activeElement)
    {
        foreach (GameObject element in Elements)
        {
            element.SetActive(false);
        }

        activeElement.SetActive(true);
    }
}
