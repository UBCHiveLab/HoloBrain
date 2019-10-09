using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class VoiceCommandPrompt : MonoBehaviour, IFocusable {

    public string prompt;
    public Text promptArea;
	// Use this for initialization
	void Start () {
		if (prompt == null)
        {
            Debug.Log("voice command prompt needs a prompt gameobject");
        }
	}

    public void ChangePrompt(string newPrompt)
    {
        prompt = newPrompt;
    }
	
	public void OnFocusEnter()
    {
        promptArea.text = "\"" + prompt + "\"";
    }

    public void OnFocusExit()
    {
        promptArea.text = "...";
    }
}
