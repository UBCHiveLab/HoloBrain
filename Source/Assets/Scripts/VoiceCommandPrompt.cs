using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceCommandPrompt : MonoBehaviour {

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
	
	public void OnStartGaze()
    {
        promptArea.text = "\"" + prompt + "\"";
    }

    public void OnEndGaze()
    {
        promptArea.text = "...";
    }
}
