using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMenuConfiguration : MonoBehaviour {

    public List<GameObject> references;

    private Dictionary<string, GameObject> CheckedReferences;
    private List<string> activeElements;
	// Use this for initialization
	void Start () {
        try {
            CheckedReferences = new Dictionary<string, GameObject>();
            activeElements = new List<string>();

            foreach(GameObject g in references)
            {
                CheckedReferences.Add(g.name, g);
                g.SetActive(false);
            }

            GameObject preload = GameObject.Find("PreLoad");
            if(preload != null)
            {
                activeElements = preload.GetComponent<AppConfiguration>().GetActiveMenuItems();
            }

            foreach(string s in activeElements)
            {
                GameObject cur = CheckedReferences[s];
                if(cur != null)
                {
                    cur.SetActive(true);
                }
            }
        } catch(Exception e)
        {
            Debug.Log("error applying menu config " + e.Message);
        }

    }
}
