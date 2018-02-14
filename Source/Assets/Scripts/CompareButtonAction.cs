using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareButtonAction : MonoBehaviour {

    public GameObject changeBrainButton;
    public GameObject selectedBrainIcon;
    private StateAccessor stateAccessor;
    private const string BRAIN_1_NAME = "brain_1";
    private const string BRAIN_2_NAME = "brain_2";

    GameObject brain_1, brain_2;

    // Use this for initialization
    void Start () {
        brain_1 = GameObject.FindWithTag(BRAIN_1_NAME);
        brain_2 = GameObject.FindWithTag(BRAIN_2_NAME);
        stateAccessor = StateAccessor.Instance;
        changeBrainButton.GetComponent<SpriteRenderer>().enabled = true;
        changeBrainButton.GetComponent<BoxCollider>().enabled = true;
        selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = true;
        selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
    }
	
	public void OnSelect ()
    {
        GameObject.Find("reset-icon").GetComponent<ResetButtonAction>().OnSelect();
        if (!stateAccessor.IsInCompareMode())
        {
            stateAccessor.SetCompare(true);
            changeBrainButton.GetComponent<SpriteRenderer>().enabled = false;
            changeBrainButton.GetComponent<BoxCollider>().enabled = false;
            selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = false;
            selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;
            float x = brain_1.transform.position.x + 0.75f;
            float y = brain_1.transform.position.y;
            float z = brain_1.transform.position.z;

            brain_2.transform.position = new Vector3(x, y, z);
        } else
        {
            stateAccessor.SetCompare(false);
            changeBrainButton.GetComponent<SpriteRenderer>().enabled = true;
            changeBrainButton.GetComponent<BoxCollider>().enabled = true;
            selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = true;
            selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
