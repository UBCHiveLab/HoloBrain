using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMRIToggleAction : MonoBehaviour {

    private GameObject brainParts;
    private GameObject fmriUI;
    private GameObject basalGangliaUI;
    private GameObject fmriObject;

    public int sceneToSwitchTo;

    private const string BRAIN_PARTS_NAME = "BrainParts";
    private const string FMRI_UI_OBJECT_NAME = "FMRIMode";
    private const string BASAL_GANGLIA_UI = "BasalGangliaModes";
    private const string FMRI_OBJECT_NAME = "fMRIBrains";


    // Use this for initialization
    void Start () {
        /*
        brainParts = GameObject.Find(BRAIN_PARTS_NAME);
        fmriObject = GameObject.Find(FMRI_OBJECT_NAME);
        fmriUI = GameObject.Find(FMRI_UI_OBJECT_NAME);
        basalGangliaUI = GameObject.Find(BASAL_GANGLIA_UI);
        brainParts.SetActive(false);
        basalGangliaUI.SetActive(false);
        */
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect()
    {
        SceneManager.LoadScene(sceneToSwitchTo);
    }
}
