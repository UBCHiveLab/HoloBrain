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
public class TEST_RestUnitTestCases {

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
        BrainParts_2GameObject = GameObject.Find("BrainParts2");
        BrainParts_1GameObject.GetComponent<RotateStructures>().Start();
        BrainParts_2GameObject.GetComponent<RotateStructures>().Start();
    }

    /// <summary>
    /// Test Case 6: Check if the brain attribute points to the current selected brain.
    /// </summary>
    [Test]
	public void TEST_CASE_06()
    {
        // Use the Assert class to test conditions.
        TestingInitializations();
        Assert.AreEqual("BrainParts", BrainParts_1GameObject.GetComponent<RotateStructures>().brain.name);
    }


    /// <summary>
    /// Test Case 7: Check if the brain attribute points to Brain_2.
    /// </summary>
    [Test]
    public void TEST_CASE_07()
    {
        // Use the Assert class to test conditions.
        TestingInitializations();
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);
    }

    /// <summary>
    /// TEST_CASE_8: When “Brain” is selected, SelectBrain returns “Brain”
    /// </summary>
    [Test]
    public void TEST_CASE_08()
    {
        // Setting up initial conditions
        TestingInitializations();
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().Awake();
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().Start();

        // To get selected brain to be Brain and not Brain2
        while (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain != BRAIN_1_GAME_OBJECT_NAME)
        {
            changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        }

        // Test
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        Assert.AreEqual(BRAIN_2_GAME_OBJECT_NAME, selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

    }

    /// <summary>
    /// TEST_CASE_9: When “Brain2” is selected, SelectBrain returns “Brain2”
    /// </summary>
    [Test]
    public void TEST_CASE_09()
    {
        TestingInitializations();
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().Awake();
        selectBrainControlGameObject.GetComponent<BrainSelectControl>().Start();

        // To get selected brain to be Brain2 and not Brain
        while (selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain != BRAIN_2_GAME_OBJECT_NAME)
        {
            changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        }

        // Test
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        Assert.AreEqual(BRAIN_1_GAME_OBJECT_NAME, selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

    }

    /// <summary>
    /// TEST_CASE_10: SelectBrain does not return null in any cases.
    /// </summary>
    [Test]
    public void TEST_CASE_10()
    {
        // Use the Assert class to test conditions.
        Assert.IsNotNull(selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

    }

    /// <summary>
    /// TEST_CASE_11: currentBrain is not null, and it points to the current selected brainparts object
    /// </summary>
    [Test]
    public void TEST_CASE_11()
    {
        // Use the Assert class to test conditions.
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        Assert.AreEqual(BRAIN_2_GAME_OBJECT_NAME, selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

    }

    /// <summary>
    /// TEST_CASE_12: brain attribute is not null and points to the currently selected brain object.
    /// </summary>
    [Test]
    public void TEST_CASE_12()
    {
        // Use the Assert class to test conditions.
        changeBrainIcon.GetComponent<ChangeSelectedBrainButtonAction>().OnSelect();
        Assert.AreEqual(BRAIN_1_GAME_OBJECT_NAME, selectBrainControlGameObject.GetComponent<BrainSelectControl>().SelectedBrain);

    }

    /*
     * Needs fixing!
    /// <summary>
    /// TEST_CASE_13: MRICollection attribute points to the child MRICollection object of the selected brain object.
    /// </summary>
    [Test]
    public void TEST_CASE_13()
    {
        // Use the Assert class to test conditions.
       

    }*/

    /*
    Stubs for integration testing have been created but testing proved to be much more efficient with Playmaker Asset.
    Also, above tests while setup in different modes can we used to reproduce each test in a automated manner.

    /// <summary>
    /// TEST_CASE_14: When “Expand” button is tapped, every bilateral structure moves away from the center of the brain, proportional to how far they are from the center initially.
    /// </summary>
    [Test]
    public void TEST_CASE_14()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_15: When the user says “Expand”, every bilateral structure moves away from the center of the brain, proportional to how far they are from the center initially.
    /// </summary>
    [Test]
    public void TEST_CASE_15()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_16: While the brain is in this exploded mode, the “Expand” button is replaced with a “Collapse” button.
    /// </summary>
    [Test]
    public void TEST_CASE_16()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_17: When “Collapse” button is tapped, every bilateral structure moves towards the center of the brain, into its anatomical position. 
    /// </summary>
    [Test]
    public void TEST_CASE_17()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_18: When user says “Collapse”, every bilateral structure moves towards the center of the brain, into its anatomical position. 
    /// </summary>
    [Test]
    public void TEST_CASE_18()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_19: While the brain is not in exploded mode, the “Collapse” button is replaced with an “Expand” button.
    /// </summary>
    [Test]
    public void TEST_CASE_19()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_20: When “Scale Up” button is tapped, the brain hologram increases in size by a factor of 2.
    /// </summary>
    [Test]
    public void TEST_CASE_20()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_21: When “Scale Down” button is tapped, the brain hologram decreases in size by a factor of ½.
    /// </summary>
    [Test]
    public void TEST_CASE_21()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_22: When the user says “Scale Up”, the brain hologram increases in size by a factor of 2.
    /// </summary>
    [Test]
    public void TEST_CASE_22()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_23: When the user says “Scale Down”, the brain hologram decreases in size by a factor of ½.
    /// </summary>
    [Test]
    public void TEST_CASE_23()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_24: When “Rotate” button is tapped, the brain continuously rotates counterclockwise. 
    /// </summary>
    [Test]
    public void TEST_CASE_24()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_25: When the user says “Rotate”,  the brain continuously rotates counterclockwise. 
    /// </summary>
    [Test]
    public void TEST_CASE_25()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_26: While the brain is rotating, the “Rotate” button is replaced with a “Stop” button.
    /// </summary>
    [Test]
    public void TEST_CASE_26()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_27: When the “Stop” button is tapped, the brain stops rotating.
    /// </summary>
    [Test]
    public void TEST_CASE_27()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_28: When the user says “Stop”, the brain stops rotating.
    /// </summary>
    [Test]
    public void TEST_CASE_28()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_29: While the brain is not rotating, the “Stop” button is replaced with a “Rotate” button.
    /// </summary>
    [Test]
    public void TEST_CASE_29()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_30: When “Reposition” button is tapped, the selected brain hologram moves with the user’s gaze.
    /// </summary>
    [Test]
    public void TEST_CASE_30()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_31: When the user says “Reposition”, the selected brain hologram moves with user’s gaze. 
    /// </summary>
    [Test]
    public void TEST_CASE_31()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_32: The brain hologram is fixed to its current position when the user performs the “air-tap” gesture. 
    /// </summary>
    [Test]
    public void TEST_CASE_32()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_33: When “Reset” button is tapped, the selected brain  reverts to the default state seen when the user first enters the application.
    /// </summary>
    [Test]
    public void TEST_CASE_33()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_34: When the user says “Reset”, the selected brain reverts to the default state seen when the user first enters the application.
    /// </summary>
    [Test]
    public void TEST_CASE_34()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_35: When “Change Brain 1” button is tapped, the user is only able to control brain 1.
    /// </summary>
    [Test]
    public void TEST_CASE_35()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_36: When “Change Brain 2” button is tapped, the user is only able to control brain 2.
    /// </summary>
    [Test]
    public void TEST_CASE_36()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_37: While brain 2 is selected, the “Change Brain” button is replaced with a “Change Brain 1” button.
    /// </summary>
    [Test]
    public void TEST_CASE_37()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_38: While brain 1 is selected, the “Change Brain” button is replaced with a “Change Brain 2” button.
    /// </summary>
    [Test]
    public void TEST_CASE_38()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_39: When the user says “Change Brain” while brain 2 is selected, the selected brain changes to brain 1.
    /// </summary>
    [Test]
    public void TEST_CASE_39()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_40: When the user says “Change Brain” while brain 1 is selected, the selected brain changes to brain 2.
    /// </summary>
    [Test]
    public void TEST_CASE_40()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_41: When the “Toggle fMRI” button is tapped while the fMRI data is not visible, the fMRI data on the brain hologram becomes visible. 
    /// </summary>
    [Test]
    public void TEST_CASE_41()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_42: When the user says “Toggle fMRI” while the fMRI data is not visible, the fMRI data on the brain hologram becomes visible.
    /// </summary>
    [Test]
    public void TEST_CASE_42()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_43: When the “Toggle fMRI” button is tapped while the fMRI data is visible, the fMRI data on the brain hologram disappears.
    /// </summary>
    [Test]
    public void TEST_CASE_43()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }

    /// <summary>
    /// TEST_CASE_44: When the user says “Toggle fMRI” while the fMRI data is visible, the fMRI data on the brain hologram disappears.
    /// </summary>
    [Test]
    public void TEST_CASE_44()
    {
        // Use the Assert class to test conditions.
        Assert.AreEqual("BrainParts2", BrainParts_2GameObject.GetComponent<RotateStructures>().brain.name);

    }




    */


}
