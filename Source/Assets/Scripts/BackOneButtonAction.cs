using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackOneButtonAction : CommandToExecute
{
    public GameObject crossfadeSlider;

    override protected Action Command()
    {
        return delegate
        {
            crossfadeSlider.GetComponent<ObjectNiftiSlider>().Back(1);
        };
    }
}
