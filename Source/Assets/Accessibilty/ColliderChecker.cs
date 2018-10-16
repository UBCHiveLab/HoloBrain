using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderChecker : MonoBehaviour {

    public float ColliderMinDistance;
    // Use this for initialization
    void Start() {
        Collider[] allColliders = GetComponentsInChildren<Collider>();
        List<infringement> items = new List<infringement>();
        foreach (Collider coll1 in allColliders)
        {
            infringement current = new infringement(coll1.gameObject, new List<GameObject>());
            foreach (Collider coll2 in allColliders)
            {
                if (CompareDistance(coll1, coll2))
                {
                    current.list.Add(coll2.gameObject);
                }
            }
            items.Add(current);
        }

        PrintItems(items);
    }

    // Update is called once per frame
    void Update() {

    }

    struct infringement
    {
        public GameObject focus;
        public List<GameObject> list;

        public infringement(GameObject f, List<GameObject> l)
        {
            focus = f;
            list = l;
        }
    }

    bool CompareDistance(Collider coll1, Collider coll2)
    {
        Vector3 point1 = coll1.bounds.ClosestPoint(coll2.bounds.center);
        Vector3 point2 = coll2.bounds.ClosestPoint(coll1.bounds.center);
        if (Vector3.Distance(point1, point2) < ColliderMinDistance)
        {
            return true;
        }
        return false;
    }

    void PrintItems(List<infringement> items)
    {
        string log = "[ACCESSIBILITY][MOBILITY][ColliderChecker]\n";
        foreach(infringement item in items)
        {
            log += item.focus.name + " is too close to...\n";
            foreach(GameObject innerItem in item.list)
            {
                log += innerItem.name + "\n";
            }
            log += "done\n";
        }

        Debug.Log(log);
    }
}