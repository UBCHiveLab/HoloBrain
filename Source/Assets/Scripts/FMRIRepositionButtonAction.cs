using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMRIRepositionButtonAction : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //brain = GameObject.Find("Brain");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnSelect()
    {
        GameObject.Find("Brain").GetComponent<HologramPlacement>().ResetStage();
    }
}
