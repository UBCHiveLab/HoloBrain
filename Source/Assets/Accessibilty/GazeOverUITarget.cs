using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accessibility.GazeOver;

public class GazeOverUITarget : MonoBehaviour {
    private GameObject uiPrefabClone;

    void OnGazeEnteredUI()
    {
        uiPrefabClone = GazeOverUISingleton.Instance.uiPrefab;
        //TODO: figure out offset
        uiPrefabClone.transform.position = transform.position;
        uiPrefabClone.transform.rotation = transform.rotation;
        uiPrefabClone.SetActive(true);
        uiPrefabClone.transform.parent = this.transform;
    }

    void OnGazeExitUI()
    {
        uiPrefabClone.SetActive(false);
    }
}