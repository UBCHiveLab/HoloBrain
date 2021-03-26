using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuctureActiveManager : MonoBehaviour
{
    public void DisableStructures()
    {
        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("Structure"))
        {
            structure.gameObject.SetActive(false);
        }
    }

    public void EnableStructures()
    {
        foreach (GameObject structure in GameObject.FindGameObjectsWithTag("Structure"))
        {
            structure.gameObject.SetActive(true);
        }
    }

}
