using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

    public class RemoveMissingScripts : Editor
    {
        [MenuItem("GameObject/Remove Missing Scripts")]
        public static void Remove()
        {
            var objs = Resources.FindObjectsOfTypeAll<GameObject>();
            int count = objs.Sum(UnityEditor.GameObjectUtility.RemoveMonoBehavioursWithMissingScript);
            Debug.Log($"Removed {count} missing scripts");
        }
    }
