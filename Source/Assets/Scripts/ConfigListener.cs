using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIConfig
{
    public class ConfigListener : MonoBehaviour
    {

        enum UIType : int { Default /*WHAT TYPES OF UI WILL WE HAVE???*/}

        public void OnConfigReady(object source, StdConfig conf)
        {
            Debug.Log("heard configready event!");
        }
    }
}
