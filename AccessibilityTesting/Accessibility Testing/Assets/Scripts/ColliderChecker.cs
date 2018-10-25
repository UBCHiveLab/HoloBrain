using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Accessibility
{
    public class ColliderChecker : AccComparer
    {
        public float ColliderMinDistance = 1.0f;

        public bool Compare(GameObject item1, GameObject item2)
        {
            Bounds bounds1 = item1.GetComponent<Collider>().bounds;
            Bounds bounds2 = item2.GetComponent<Collider>().bounds;
            Vector3 point1 = bounds1.ClosestPoint(bounds2.center);
            Vector3 point2 = bounds2.ClosestPoint(bounds1.center);
            if(Vector3.Distance(point1, point2) < ColliderMinDistance)
            {
                return false;
            }
            return true;
        }

        public void OnCompareReady(object source, CompareEvent evnt)
        {
            string log = "[ACCESSIBILITY][MOBILITY][ColliderChecker]\n";
            bool logged = false;
            if (!Compare(evnt.item1, evnt.item2))
            {
                log += evnt.item1.name + " is too close to " + evnt.item2.name + "\n";
                logged = true;
            }
            if(logged)
            {
                Debug.Log(log);
            }
        }

        //constructor
        public ColliderChecker()
        {

        }
    }
}