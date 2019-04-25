using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CortexCollider : MonoBehaviour {

    public BoxCollider myCollider;
	// Use this for initialization
	void Start () {
        foreach (Renderer current in this.GetComponentsInChildren<Renderer>())
        {
            myCollider.bounds.Encapsulate(current.bounds);
        }
        myCollider.center = Vector3.zero;
	}
}