using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HoloToolkit.Unity.InputModule;

public class ApplyMenuConfiguration : MonoBehaviour {

    public List<GameObject> references;
    public EventSystem eventSystem;

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
                if(cur.name == "Educational")
                {
                        cur.GetComponent<ButtonCommands>().OnInputClicked(new InputClickedEventData(eventSystem));
                }
            }
        } catch(Exception e)
        {
            Debug.Log("error applying menu config " + e.Message);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
