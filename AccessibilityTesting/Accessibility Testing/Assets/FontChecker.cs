using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Accessibility
{
    public class FontChecker : AccChecker
    {
        public bool Check(GameObject item)
        {
            return false;
        }

        public void OnCheckReady(object source, CheckEvent evnt)
        {
            // Debug.Log("If you put your texts inside images, make sure the fonts are accessible and consistent!!!");
            string log = "[ACCESSIBILITY][MOBILITY][FontChecker]";
            bool logged = false;
            if (Check(evnt.item))
            {
                logged = true;

            }
            if (logged)
            {
                Debug.Log(log);
            }
        }

        public FontChecker()
        {

        }
    }

}

