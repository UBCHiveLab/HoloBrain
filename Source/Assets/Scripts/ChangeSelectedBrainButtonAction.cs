using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSelectedBrainButtonAction : MonoBehaviour
{

    private const string BRAIN_1 = "Brain";
    public GameObject selectedBrainIcon;
    public Sprite StartIcon;
    public Sprite EndIcon;

    private GameObject selectBrainControlGameObject;

    // Use this for initialization
    void Start ()
    {
        selectBrainControlGameObject = GameObject.Find("BrainSelectControl");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect()
    {
        //do the action
        Debug.Log("ChangeBrain button selected");
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().OnSelect();
        SwapImage(selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);
    }

    private void SwapImage(string selectedBrain)
    {
        if (selectedBrain == BRAIN_1)
        {
            selectedBrainIcon.GetComponent<SpriteRenderer>().sprite = StartIcon;
        } else
        {
            selectedBrainIcon.GetComponent<SpriteRenderer>().sprite = EndIcon;
        }
        //gameObject.GetComponentInChildren<Image>().GetComponent<ImageSwap>().ToggleImage(); TODO
    }
}
