using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackTenButtonAction : CommandToExecute
{
    public GameObject crossfadeSlider;

    // Use this for initialization

    override protected Action Command()
    {
        return delegate
        {
            crossfadeSlider.GetComponent<ObjectNiftiSlider>().Back(10);
        };
    }
}
