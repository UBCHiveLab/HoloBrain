using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/*
    Follow the steps for PlayMode tests from the following documentation link:
    https://docs.unity3d.com/Manual/testing-editortestsrunner.html
 */

public class TEST_ModeSelection {

    #region variable declarations
    private const string BRAIN_1_GAME_OBJECT_NAME = "Brain";
    private const string BRAIN_2_GAME_OBJECT_NAME = "Brain2";
    private string selectedBrainName;
    private GameObject changeBrainIcon;

    public GameObject OneBrainButton { get; private set; }
    public GameObject ModeSelectionScript { get; private set; }

    GameObject selectBrainControlGameObject;
    #endregion

    void TestingInitializations() {
        OneBrainButton = GameObject.Find("OneBrainButton");
        ModeSelectionScript = GameObject.Find("ModeSelectionScript");
        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
        changeBrainIcon = GameObject.Find("change-selected-brain-icon");
        OneBrainButton.GetComponent<ButtonAction>().Start();
        //ModeSelectionScript.GetComponent<ModeSelection>().Start();
    }

    /// <summary>
    /// Test Case 4:  When “OneBrain” button is tapped, LoadOneBrainScene will be called, (“brainMode”, “one”) will be stored in PlayerPrefs.
    /// </summary>
    [Test]
	public void TEST_CASE_4_OneBrainMode() {
        // setup
        TestingInitializations();

        // test
        OneBrainButton.GetComponent<ButtonAction>().OnSelect();
        Assert.AreEqual("one", PlayerPrefs.GetString("brainMode"));
    }
    
}
