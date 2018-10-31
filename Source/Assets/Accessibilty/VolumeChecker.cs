using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Accessibility
{
    public class VolumeChecker : AccChecker
    {        
        public bool Check(GameObject item)
        {
            return (item.GetComponent<AudioSource>() == null);
        }

        public void OnCheckReady(object source, CheckEvent evnt)
        {
            string log = "[ACCESSIBILITY][MOBILITY][VolumeChecker]";
            bool logged = false;
            if (Check(evnt.item))
            {
                logged = true;
                log += evnt.item.name + " Has no AudioSource\n";
            }
            if (logged)
            {
                Debug.Log(log);
            }
        }

        public VolumeChecker() {

        }
    }

}

