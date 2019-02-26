using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkipTenButtonAction : MonoBehaviour
{
    public GameObject crossfadeSlider;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelect()
    {
        audio.Play();
        crossfadeSlider.GetComponent<ObjectNiftiSlider>().Skip(10);
    }
}
