using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkipOneButtonAction : CommandToExecute
{
    public GameObject crossfadeSlider;

    override protected Action Command()
    {
        return delegate
        {
            crossfadeSlider.GetComponent<ObjectNiftiSlider>().Skip(1);
        };
    }
}
