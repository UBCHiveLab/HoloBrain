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
    void Awake()
    {
        selectBrainControlGameObject = GameObject.Find("BrainSelectControl");
        stateAccessor = StateAccessor.Instance;
    }

    void Start ()
    {
        isolateButtons = new List<GameObject>();
        SwapImage(selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

        foreach (GameObject isolateButton in GameObject.FindGameObjectsWithTag("isolate"))
        {
            isolateButtons.Add(isolateButton);
        }

    }

    public void OnSelect()
    {
        //do the action
        Debug.Log("ChangeBrain button selected");
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().OnSelect();
        SwapImage(selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

        if (stateAccessor.GetCurrentMode() == StateAccessor.Mode.Isolated)
        {
            foreach (GameObject isolateButton in isolateButtons)
            {
                isolateButton.GetComponent<ButtonCommands>().setIsPressed(isolateButton.GetComponent<IsolateButtonAction>().getButtonStatus());
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
