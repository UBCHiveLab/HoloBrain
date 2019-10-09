using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApplyAppConfiguration : MonoBehaviour {

    // Use this for initialization
    public List<GameObject> References;
    public EventSystem eventSystem;

    private Dictionary<string, GameObject> CheckedReferences;
    private List<string> activeElements;

	void Start () {
        try {
            CheckedReferences = new Dictionary<string, GameObject>();
            activeElements = new List<string>();

            foreach(GameObject g in References)
            {
                CheckedReferences.Add(g.name, g);
                g.SetActive(false);
            }

            GameObject preLoad = GameObject.Find("PreLoad");
            if(preLoad != null)
            {
                activeElements = preLoad.GetComponent<AppConfiguration>().GetActiveHolograms();
            }

            foreach(string e in activeElements)
            {
                GameObject cur = CheckedReferences[e];
                if(cur != null)
                {
                    Debug.Log("activating " + cur.name);
                    cur.SetActive(true);
                } else
                {
                    Debug.Log("could not find " + e);
                }
            }
        } catch(Exception e)
        {
            Debug.Log("error while applying app config " + e.Message);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
