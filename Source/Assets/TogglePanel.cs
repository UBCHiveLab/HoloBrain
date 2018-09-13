using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanel : MonoBehaviour {

    //which panel to be toggled is linked from outside
    public GameObject target;
    List<GameObject> panels = new List<GameObject>();
    bool toggle;
	// Use this for initialization
	void Start () {
        //find where the panels are and store a reference
        foreach (Transform child in transform.parent)
        {
            Debug.Log(child.name);
            if(child.name == "panels")
            {
                foreach(Transform panel in child)
                {
                    panels.Add(panel.gameObject);
                }
            }
        }
        toggle = false;
	}
	
	// Update is called once per frame
    //unused
	void Update () {
		
	}

    public void OnSelect()
    {
        foreach(GameObject p in panels)
        {
            if(p.name == target.name)
            {
                Animation anim = p.GetComponent<Animation>();
                int index = 0;
                foreach(AnimationState state in anim)
                {
                    if (!toggle && index == 0)
                    {
                        Debug.Log(state.name);
                        anim.Play(state.name);
                    } else if (toggle && index == 1)
                    {
                        Debug.Log(state.name);
                        anim.Play(state.name);
                    }
                    index++;
                }
            }
        }
        toggle = !toggle;
    }
}
