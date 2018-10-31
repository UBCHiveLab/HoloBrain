using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Accessibility.GazeOver
{
    public class GazeOverUISingleton : MonoBehaviour {
        public GameObject uiPrefab;

        private static  GazeOverUISingleton instance = null;
        public static GazeOverUISingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GazeOverUISingleton();
                }
                return instance;
            }
        }

        public GazeOverUISingleton()
        {
            GameObject uiPrefabClone = Instantiate(uiPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            uiPrefabClone.SetActive(false);
        }
    }
}