using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;

/*
    Follow the steps for PlayMode tests from the following documentation link:
    https://docs.unity3d.com/Manual/testing-editortestsrunner.html
 */
/// <summary>
/// 3 unit tests for testing BrainSelectControl functionalities.
/// </summary>
public class TEST_BrainSelectController {

    #region variable declarations
    private const string BRAIN_1_GAME_OBJECT_NAME = "Brain";
    private const string BRAIN_2_GAME_OBJECT_NAME = "Brain2";
    private string selectedBrainName;
    private GameObject changeBrainIcon;
    GameObject selectBrainControlGameObject;
    #endregion

    void TestingInitializations() {
        EditorSceneManager.OpenScene("Assets/Scenes/HoloBrain.unity");
        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
        changeBrainIcon = GameObject.Find("change-selected-brain-icon");
    }

    /// <summary>
    /// Test Case 1: By default, the selected brain should be “Brain” (not null).
    /// </summary>
    [Test]
	public void TEST_CASE_1_DefaultSelectedBrain() {
        // Use the Assert class to test conditions.
        TestingInitializations();
        Assert.AreEqual(BRAIN_1_GAME_OBJECT_NAME, selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);
    }

    /// <summary>
    /// Test Case 2: When currently selected “Brain”, by calling changeBrain(), the selected brain should be “Brain2
    /// </summary>
    [Test]
    public void TEST_CASE_2_ChangeBrainTo1() {
        // Setting up initial conditions
        TestingInitializations();
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().Awake();
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().Start();

        // To get selected brain to be Brain2 and not Brain
        while (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain != BRAIN_2_GAME_OBJECT_NAME) {
            changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        }

        // Test
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        Assert.AreEqual(BRAIN_1_GAME_OBJECT_NAME, selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);
    }

    /// <summary>
    /// Test Case 3: When currently selected “Brain2”, by calling changeBrain(), the selected brain should be “Brain”
    /// </summary>
    [Test]
    public void TEST_CASE_3_ChangeBrainTo2() {
        // Setting up initial conditions
        TestingInitializations();
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().Awake();
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().Start();
        
        // To get selected brain to be Brain and not Brain2
        while (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain != BRAIN_1_GAME_OBJECT_NAME) {
            changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        }

        // Test
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        Assert.AreEqual(BRAIN_2_GAME_OBJECT_NAME, selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);
    }
    
}
