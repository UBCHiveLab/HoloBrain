using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Accessibility {
    //[HelpURL("https://google.com")]
    //[DisallowMultipleComponent]
    //[RequireComponent(typeof(AccessibilityManager))]
    public class AccConfig : MonoBehaviour
    {
        
        [Header("Vision")]

        [Range(0.2f, 5.0f)]
        [Tooltip("Defines User Interface's scaling factor")]
        public float UI_SCALE_FACTOR = 1f;


        //--------------------------------------------------------
        [Header("Hearing")]
        public int blahblah2;

        //--------------------------------------------------------

        [Header("Mobility")]
        public float SET_MAX_DISTANCE = 0.2f;
        public float SET_EXPECTED_USER_X = 0.0f;
        public float SET_EXPECTED_USER_Y = 0.0f;
        public float SET_EXPECTED_USER_Z = 0.0f;

        public static float MAX_DISTANCE = 0.2f;
        public static float EXPECTED_USER_X = 0.0f;
        public static float EXPECTED_USER_Y = 0.0f;
        public static float EXPECTED_USER_Z = 0.0f;

        [Range(0, 20)]
        [Tooltip("Define the max distance for object")]
        public float SET_EXPECTED_USER_MAX_DISTANCE = 1.0f;

        public static float EXPECTED_USER_MAX_DISTANCE = 1.0f;

        [TextArea]
        public string SOMETHING_DUMB;
        //--------------------------------------------------------

        [Header("Cognitive")]
        public int blahblah3;
        

        void Start()
        {
            Debug.Log("config awake");
            
            MAX_DISTANCE = SET_MAX_DISTANCE;
            EXPECTED_USER_X = SET_EXPECTED_USER_X;
            EXPECTED_USER_Y = SET_EXPECTED_USER_Y;
            EXPECTED_USER_Z = SET_EXPECTED_USER_Z;
            EXPECTED_USER_MAX_DISTANCE = SET_EXPECTED_USER_MAX_DISTANCE;
        }
    }
}