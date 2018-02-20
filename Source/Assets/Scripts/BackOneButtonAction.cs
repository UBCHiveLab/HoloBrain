using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackOneButtonAction : MonoBehaviour
{

    private const string FMRI_SLIDER = "CrossfadeSlider";
    private GameObject crossfadeSlider;

    // Use this for initialization
    void Start()
    {

        crossfadeSlider = GameObject.Find(FMRI_SLIDER);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSelect()
    {
        crossfadeSlider.GetComponent<ObjectNiftiSlider>().Back(1);
    }
}
