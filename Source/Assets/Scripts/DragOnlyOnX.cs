using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOnlyOnX : MonoBehaviour {

    public float dragSpeed = 0.001f;
    public float minX = -0.41f;
    public float maxX = -0.25f;
    Vector3 lastMousePos;

    void OnMouseDown()
    {
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.localPosition;
        pos.x += delta.x * dragSpeed;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
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