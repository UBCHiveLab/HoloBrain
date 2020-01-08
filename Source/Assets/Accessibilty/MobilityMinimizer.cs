using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilityMinimizer : MonoBehaviour {

    public float MAX_DISTANCE = 2.0f;
	// Use this for initialization
	void Start () {
        List<farAway> items = CheckDistances();
        string log = "";
        if(items.Count > 0)
        {
            log += "[ACCESSIBILITY][MOBILITY][MobilityMinimizer] Some objects in your scene are far away from each other, this can be a barrier to users with mobility impairments:\n";
            log += PrintItems(items);
            Debug.Log(log);
        }
        else
        {
            Debug.Log("[ACCESSIBILITY][MOBILITY][MobilityMinimizer] All of your objects are acceptable distances from each other.");
        }
	}

    struct farAway
    {
        public GameObject A;
        public GameObject B;

        public farAway(GameObject x, GameObject y)
        {
            A = x;
            B = y;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    List<farAway> CheckDistances()
    {
        List<farAway> result = new List<farAway>();
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach(Transform child1 in allChildren)
        {
            foreach (Transform child2 in allChildren)
            {
                if(Vector3.Distance(child1.position, child2.position) > MAX_DISTANCE) {
                    //add child1 and child2 to result list
                    result.Add(new farAway(child1.gameObject, child2.gameObject));
                }
            }
        }

        return result;
    }

    string PrintItems(List<farAway> items)
    {
        string log = "";
        foreach(farAway item in items)
        {
            log += item.A.name + " is far away from " + item.B.name + "\n";
        }
        return log;
    }
}
