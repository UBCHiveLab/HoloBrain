using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureCollision : MonoBehaviour {

    private List<Collider> colliders;

    private void Awake()
    {
        foreach(StructureCollision other in GameObject.Find("menu").GetComponentsInChildren<StructureCollision>())
        {
            foreach(Collider coll1 in other.GetColliders())
            {
                foreach(Collider coll2 in GetColliders())
                {
                    Physics.IgnoreCollision(coll1, coll2, true);
                }
            }
        }
    }

    public List<Collider> GetColliders()
    {
        if(colliders == null)
        {
            colliders = new List<Collider>();
            foreach (Collider coll in transform.GetComponentsInChildren<Collider>())
            {
                colliders.Add(coll);
            }
        }
        return colliders;
    }
}
