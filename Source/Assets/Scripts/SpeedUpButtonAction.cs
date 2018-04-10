using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedUpButtonAction : MonoBehaviour
{

    private const string FMRI_SLIDER = "CrossfadeSlider";
    private GameObject crossfadeSlider;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        crossfadeSlider = GameObject.Find(FMRI_SLIDER);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelect()
    {
        audio.Play();
        crossfadeSlider.GetComponent<ObjectNiftiSlider>().SpeedUpPlayback();
    }
}
