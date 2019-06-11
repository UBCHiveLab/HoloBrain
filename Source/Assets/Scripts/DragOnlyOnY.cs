using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOnlyOnY : MonoBehaviour {

    public float dragSpeed = 0.001f;
    public float minY = -0.41f;
    public float maxY = -0.25f;
    Vector3 lastMousePos;

    void OnMouseDown()
    {
        lastMousePos = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - lastMousePos;
        Vector3 pos = transform.localPosition;
        pos.y += delta.y * dragSpeed;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
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