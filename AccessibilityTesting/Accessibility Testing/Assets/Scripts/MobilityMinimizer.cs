using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Accessibility
{
    public class MobilityMinimizer : AccChecker, AccComparer
    {

        private float MAX_DISTANCE;

        private float EXPECTED_USER_X;
        private float EXPECTED_USER_Y;
        private float EXPECTED_USER_Z;
        private float EXPECTED_USER_MAX_DISTANCE;

        public bool Compare(GameObject child1, GameObject child2)
        {
            return Vector3.Distance(child1.transform.position, child2.transform.position) > MAX_DISTANCE;
        }

        public void OnCompareReady(object source, CompareEvent evnt)
        {
            string log = "[ACCESSIBILITY][MOBILITY][MobilityMinimizer]";
            bool logged = false;
            if (Compare(evnt.item1, evnt.item2))
            {
                logged = true;
                log += evnt.item1.name + " is too far away from " + evnt.item2.name + "\n";
            }
            if(logged)
            {
                Debug.Log(log);
            }
        }

        public bool Check(GameObject child)
        {
            return Vector3.Distance(child.transform.position, new Vector3(EXPECTED_USER_X, EXPECTED_USER_Y, EXPECTED_USER_Z)) > EXPECTED_USER_MAX_DISTANCE;
        }

        public void OnCheckReady(object source, CheckEvent evnt)
        {
            string log = "[ACCESSIBILITY][MOBILITY][MobilityMinimizer]";
            bool logged = false;
            if(Check(evnt.item))
            {
                logged = true;
                log += evnt.item.name + " is too far from the expected user position\n";
            }

            if(logged)
            {
                Debug.Log(log);
            }
        }

        public MobilityMinimizer()
        {
            EXPECTED_USER_X = AccConfig.EXPECTED_USER_X;
            EXPECTED_USER_Y = AccConfig.EXPECTED_USER_Y;
            EXPECTED_USER_Z = AccConfig.EXPECTED_USER_Z;
            EXPECTED_USER_MAX_DISTANCE= AccConfig.EXPECTED_USER_MAX_DISTANCE;
            MAX_DISTANCE = AccConfig.MAX_DISTANCE;
        }
    }
}