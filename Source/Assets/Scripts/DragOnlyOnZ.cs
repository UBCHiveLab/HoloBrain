using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOnlyOnZ : MonoBehaviour {

    public float dragSpeed = 0.001f;
    public float minZ = -0.41f;
    public float maxZ = -0.25f;
    Vector3 lastMousePos;

    void OnMouseDown()
    {
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.localPosition;
        pos.z += delta.x * dragSpeed;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.localPosition = pos;
        lastMousePos = Input.mousePosition;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}