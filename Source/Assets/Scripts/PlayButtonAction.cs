using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayButtonAction : CommandToExecute {
    
    public GameObject crossfade;
    public List<GameObject> playButtons = new List<GameObject>();
    public List<GameObject> pauseButtons = new List<GameObject>();
    public bool IsPauseButton = false;

    private ObjectNiftiSlider crossfadeSlider;

    // Use this for initialization
    override public void Start ()
    {
        crossfadeSlider = crossfade.GetComponent<ObjectNiftiSlider>();
        base.Start();
    }
    
    override protected Action Command()
    {
        return delegate
        {
            crossfadeSlider.TogglePlay();

            if (crossfadeSlider.isPlaying)
            {
                foreach (GameObject obj in playButtons)
                {
                    obj.SetActive(true);
                }

                foreach (GameObject obj in pauseButtons)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject obj in playButtons)
                {
                    obj.SetActive(false);
                }

                foreach (GameObject obj in pauseButtons)
                {
                    obj.SetActive(true);
                }
            }
        };
    }
}
