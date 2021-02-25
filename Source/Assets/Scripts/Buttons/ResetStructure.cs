using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStructure : MonoBehaviour
{
    [SerializeField]
    private GameObject structure;

    public Vector3 DefaultPosition;
    public Quaternion DefaultRotation;
    public Vector3 DefaultScale;

    private void Awake()
    {
        DefaultPosition = structure.transform.localPosition;
        DefaultRotation = structure.transform.localRotation;
        DefaultScale = structure.transform.localScale;

    }

    public void Reset()
    {
        structure.transform.localPosition = DefaultPosition;
        structure.transform.localRotation = DefaultRotation;
        structure.transform.localScale = DefaultScale;
    }
}
