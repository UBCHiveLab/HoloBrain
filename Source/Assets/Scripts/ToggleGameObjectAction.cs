using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToggleGameObjectAction : CommandToExecute {

    public GameObject objectToToggle;

    protected override Action Command()
    {
        return delegate
        {
            if(objectToToggle != null)
            {
                objectToToggle.SetActive(!objectToToggle.activeSelf);
                GetComponent<ButtonAppearance>().setActiveDefault(objectToToggle.activeSelf);
            }
        };
    }
}
