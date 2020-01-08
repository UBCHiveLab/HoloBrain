using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour {

    DataRecorder dataRecorder;
    bool sceneSwitched = false;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        PlayerPrefs.SetString("mode", "solo");
        Debug.Log("finished ddol start");
        dataRecorder = FindObjectOfType<DataRecorder>();
        // this.gameObject.AddComponent<StudentModeCommands>();
    }
	
	// Update is called once per frame
	void Update () {
        if (dataRecorder.IsNameSet && dataRecorder.IsConditionSet && !sceneSwitched)
        {   
            SceneManager.LoadScene("App");
            sceneSwitched = true;
            gameObject.AddComponent<AppStateManager>();
        }
	}
}
