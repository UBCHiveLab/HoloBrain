using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Transform.Rotate example
//
// This script creates two different cubes: one red which is rotated using Space.Self; one green which is rotated using Space.World.
// Add it onto any GameObject in a scene and hit play to see it run. The rotation is controlled using xAngle, yAngle and zAngle, modifiable on the inspector.
public class RotateStructure : MonoBehaviour
{
    public float xAngle, yAngle, zAngle;

    [SerializeField]
    private GameObject structure;

    void Awake()
    {
    }

    void Update()
    {
        structure.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
    }
}
