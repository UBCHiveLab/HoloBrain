using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMenuFeedback : MonoBehaviour {
    
    public void OnSelect()
    {
        foreach(ButtonAppearance button in transform.parent.GetComponentsInChildren<ButtonAppearance>())
        {
            if(button.name != name)
            {
                button.ResetButton();
            } else
            {
                button.SetButtonActive();
            }
        }
    }
}
