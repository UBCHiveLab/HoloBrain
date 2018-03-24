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
/// Unit test for RotateStrcutures
/// </summary>
public class TEST_RotateStructures {

    #region variable declarations
    private const string BRAIN_1_GAME_OBJECT_NAME = "Brain";
    private const string BRAIN_2_GAME_OBJECT_NAME = "Brain2";
    private string selectedBrainName;
    private GameObject changeBrainIcon;
    GameObject selectBrainControlGameObject, BrainParts_1GameObject, BrainParts_2GameObject;
    #endregion

    void TestingInitializations() {
        EditorSceneManager.OpenScene("Assets/Scenes/HoloBrain.unity");
        selectBrainControlGameObject = GameObject.FindWithTag("selectBrainController");
        changeBrainIcon = GameObject.Find("change-selected-brain-icon");
        BrainParts_1GameObject = GameObject.Find("BrainParts");
        BrainParts_1GameObject.GetComponent<RotateStructures>().Start();
        BrainParts_2GameObject = GameObject.Find("BrainParts2");
        BrainParts_2GameObject.GetComponent<RotateStructures>().Start();
    }

    /// <summary>
    /// Test Case 6: Check if the brain attribute points to the current selected brain.
    /// </summary>
    [Test]
	public void TEST_CASE_6_PointingToBrain_1() {
        // Use the Assert class to test conditions.
        TestingInitializations();
        Assert.AreEqual("BrainParts", BrainParts_1GameObject.GetComponent<RotateStructures>().brain.name);
    }


    /// <summary>
    /// Test Case 7: Check if the brain attribute points to Brain_2.
    /// </summary>
    [Test]
    public void TEST_CASE_7_PointingToBrain_2()
    {
        // Use the Assert class to test conditions.
        TestingInitializations();
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);
    }

}
