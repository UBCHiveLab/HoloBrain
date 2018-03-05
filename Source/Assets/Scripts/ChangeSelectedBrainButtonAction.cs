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
    private StateAccessor stateAccessor;
    private List<GameObject> isolateButtons;

    private GameObject selectBrainControlGameObject;

    // Use this for initialization
    void Start ()
    {
        selectBrainControlGameObject = GameObject.Find("BrainSelectControl");
        stateAccessor = StateAccessor.Instance;
        isolateButtons = new List<GameObject>();
        SwapImage(selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

        foreach (GameObject isolateButton in GameObject.FindGameObjectsWithTag("isolate"))
        {
            isolateButtons.Add(isolateButton);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect()
    {
        //do the action
        Debug.Log("ChangeBrain button selected");
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().OnSelect();
       // SwapImage(selectBrainControlGameObject.GetComponent<BrainSelectControl>().getSelectedBrain);

        if (stateAccessor.GetCurrentMode() == StateAccessor.Mode.Isolated)
        {
            foreach (GameObject isolateButton in isolateButtons)
            {
                isolateButton.GetComponent<ButtonCommands>().setIsPressd(isolateButton.GetComponent<IsolateButtonAction>().getButtonStatus());
                isolateButton.GetComponent<ButtonEnabledFeedback>().ToggleOpacity(isolateButton.GetComponent<IsolateButtonAction>().getButtonStatus());
            }
        }
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
