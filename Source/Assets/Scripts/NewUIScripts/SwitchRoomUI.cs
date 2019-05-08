using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchRoomUI : MonoBehaviour
{
    public List<GameObject> Buttons;
    public List<GameObject> Elements;

    protected SwitchRoomUICondition condition;

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
        condition = GetComponent<SwitchRoomUICondition>();
    }

    private void RegisterClickToElement()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Debug.Log(Buttons[i].name);
            Buttons[i].GetComponent<ButtonCommands>().AddCommand(SwitchToUI(i));
        }
    }

    private Action SwitchToUI(int i)
    {
        return delegate
        {
            if (condition != null && condition.SwitchCondition()) //condition is met
            {
                foreach (GameObject element in Elements)
                {
                    element.SetActive(false);
                }

                Elements[i].SetActive(true);
            }
            else if(condition == null)  //there is no condition
            {
                foreach (GameObject element in Elements)
                {
                    element.SetActive(false);
                }

                Elements[i].SetActive(true);
            }
        };
    }
}
