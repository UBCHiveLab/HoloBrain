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
    private const string BRAINPARTS_1_NAME = "BrainParts";
    private const string BRAINPARTS_2_NAME = "BrainParts2";
    private const string BRAIN_SELECT_CONTROL_TAG_NAME = "selectBrainController";

    GameObject brain_1, brain_2, brainparts_1, brainparts_2, brainSelectControl;
    GameObject ButtonsMenu;

    // Use this for initialization
    void Awake()
    {
        stateAccessor = StateAccessor.Instance;
        
    }

    void Start ()
    {
        brainSelectControl = GameObject.FindWithTag(BRAIN_SELECT_CONTROL_TAG_NAME);
        brain_1 = GameObject.FindWithTag(BRAIN_1_NAME);
        brain_2 = GameObject.FindWithTag(BRAIN_2_NAME);
        brainparts_1 = GameObject.Find(BRAINPARTS_1_NAME);
        brainparts_2 = GameObject.Find(BRAINPARTS_2_NAME);
        changeBrainButton.SetActive(true);
        changeBrainButton.GetComponent<SpriteRenderer>().enabled = true;
        changeBrainButton.GetComponent<BoxCollider>().enabled = true;
        selectedBrainIcon.SetActive(true);
        selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = true;
        selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
    }


    public void OnSelect ()
    {
        brainparts_1.GetComponent<RotateStructures>().ResetRotation();
        brainparts_1.GetComponent<ScaleToggler>().ResetScale();
        brainparts_1.GetComponent<ExplodingCommands>().ResetExplode();
        brainparts_2.GetComponent<RotateStructures>().ResetRotation();
        brainparts_2.GetComponent<ScaleToggler>().ResetScale();
        brainparts_2.GetComponent<ExplodingCommands>().ResetExplode();
        string buttons;
        if (stateAccessor.GetCurrentMode() == StateAccessor.Mode.Default)
            buttons = "Buttons";
        else if (stateAccessor.GetCurrentMode() == StateAccessor.Mode.Isolated)
        {
            GameObject.Find("remove-icon").GetComponent<IsolateButtonAction>().ResetAllParts();
            buttons = "IsolateMode";
        }
        else
            buttons = "MRIMode";
        ButtonsMenu = GameObject.Find(buttons);
        for (int i = 0; i < ButtonsMenu.transform.childCount; i++)
        {
            Debug.Log("in reset ui buttons" + ButtonsMenu.transform.GetChild(i).gameObject.name);
            if (ButtonsMenu.transform.GetChild(i).gameObject.GetComponent<ButtonSwapFeedback>() != null)
            {
                Debug.Log("in reset ui buttons thw swap feed back is not null " + ButtonsMenu.transform.GetChild(i).gameObject.name);
                ButtonsMenu.transform.GetChild(i).gameObject.GetComponent<ButtonSwapFeedback>().ResetButtonState();
            }
        }

        if (!stateAccessor.IsInCompareMode())
        {
            stateAccessor.SetCompare(true);
            changeBrainButton.SetActive(false);
            changeBrainButton.GetComponent<SpriteRenderer>().enabled = false;
            changeBrainButton.GetComponent<BoxCollider>().enabled = false;
            selectedBrainIcon.SetActive(false);
            selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = false;
            selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = false;

            float x = brain_1.transform.position.x + 0.75f;
            float y = brain_1.transform.position.y;
            float z = brain_1.transform.position.z;

            brain_2.transform.position = new Vector3(x, y, z);
        } else
        {
            stateAccessor.SetCompare(false);
            changeBrainButton.SetActive(true);
            changeBrainButton.GetComponent<SpriteRenderer>().enabled = true;
            changeBrainButton.GetComponent<BoxCollider>().enabled = true;
            selectedBrainIcon.SetActive(true);
            selectedBrainIcon.GetComponent<SpriteRenderer>().enabled = true;
            selectedBrainIcon.transform.Find("white-border").GetComponent<SpriteRenderer>().enabled = true;
        }
        Debug.Log(stateAccessor.IsInCompareMode());
        brainSelectControl.GetComponent<BrainSelectControl>().ToggleConeVisibility(!stateAccessor.IsInCompareMode());
    }
}
