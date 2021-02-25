using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleStructures : MonoBehaviour
{
    [SerializeField]
    private GameObject structure;

    public Vector3 scaleChange;


    public void ScaleUp()
    {
        if (structure.transform.localScale.y < 1.0f)
        {
            structure.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            structure.transform.localScale += scaleChange;
        }
    }

    public void ScaleDown()
    {
        structure.transform.localScale -= scaleChange;
        if (structure.transform.localScale.y < 0.1f)
        {
            structure.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}