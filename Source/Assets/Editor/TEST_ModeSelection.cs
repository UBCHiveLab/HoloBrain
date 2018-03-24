using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
//using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

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
    public GameObject TwoBrainButton { get; private set; }
    public GameObject ModeSelectionScript { get; private set; }

    GameObject selectBrainControlGameObject;
    #endregion

    void TestingInitializations() {
        EditorSceneManager.OpenScene("Assets/Scenes/BrainNumSelectScene.unity");
        //Debug.Log(SceneManager.GetActiveScene().name);
        OneBrainButton = GameObject.Find("OneBrainButton");
        TwoBrainButton = GameObject.Find("TwoBrainsButton");
        ModeSelectionScript = GameObject.Find("ModeSelectionScript");
        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
        changeBrainIcon = GameObject.Find("change-selected-brain-icon");
        ModeSelectionScript.GetComponent<ModeSelection>().Start();
        OneBrainButton.GetComponent<ButtonAction>().Start();
        TwoBrainButton.GetComponent<ButtonAction>().Start();
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

    /// <summary>
    /// Test Case 5:  When “TwoBrains” button is tapped, LoadOneBrainScene will be called, (“brainMode”, “two”) will be stored in PlayerPrefs.
    /// </summary>
    [Test]
    public void TEST_CASE_5_TwoBrainMode()
    {
        // setup
        TestingInitializations();

        // test
        TwoBrainButton.GetComponent<ButtonAction>().OnSelect();
        Assert.AreEqual("two", PlayerPrefs.GetString("brainMode"));
    }

}
