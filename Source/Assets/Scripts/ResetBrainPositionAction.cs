using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBrainPositionAction : MonoBehaviour {

    private const string BRAIN_1_NAME = "brain_1";
    private const string BRAIN_2_NAME = "brain_2";

    GameObject brain_1, brain_2;

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        brain_1 = GameObject.FindWithTag(BRAIN_1_NAME);
        brain_2 = GameObject.FindWithTag(BRAIN_2_NAME);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect()
    {
        //do the action
        Debug.Log("Rest Brain Position button selected");
        //Debug.Log();
        float x = brain_1.transform.position.x + 0.75f;
        float y = brain_1.transform.position.y;
        float z = brain_1.transform.position.z;

        brain_2.transform.position = new Vector3(x, y, z);

        Debug.Log("brain_2!!");
        //Debug.Log("x: " + x);
    }
}
