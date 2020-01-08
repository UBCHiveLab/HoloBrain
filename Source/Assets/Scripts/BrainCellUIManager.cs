using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


public class BrainCellUIManager: Singleton<BrainCellUIManager> {

    public GameObject dynamicUI;
    public GameObject dynamicObject;
    public GameObject staticUI;
    public GameObject staticObject;

    // Use this for initialization
    void Start () {
        dynamicUI.SetActive(false);
        dynamicObject.SetActive(false);
	}

    public void SetStaticMode(bool isStatic)
    {
        if (isStatic)
        {
            staticUI.SetActive(true);
            staticObject.SetActive(true);
            dynamicUI.SetActive(false);
            dynamicObject.SetActive(false);
        } else
        {
            staticUI.SetActive(false);
            staticObject.SetActive(false);
            dynamicUI.SetActive(true);
            dynamicObject.SetActive(true);
        }
    }
}
