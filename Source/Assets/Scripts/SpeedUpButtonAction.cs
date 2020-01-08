using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpeedUpButtonAction : CommandToExecute
{
    public GameObject crossfadeSlider;

    protected override Action Command()
    {
        return delegate
        {
            crossfadeSlider.GetComponent<ObjectNiftiSlider>().SpeedUpPlayback();
        };
    }
}
