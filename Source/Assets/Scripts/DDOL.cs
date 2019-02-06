using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Main");
        PlayerPrefs.SetString("mode", "solo");
        this.gameObject.AddComponent<AppStateManager>();
       // this.gameObject.AddComponent<StudentModeCommands>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
