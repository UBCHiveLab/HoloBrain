// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour
{

    private string STUDENT_BUTTON;
    private string PROFESSOR_BUTTON;
    private string JOIN_LAB_BUTTON;
    private string SOLO_BUTTON;
    private string BACK_BUTTON;
    private string ONE_BRAIN_BUTTON;
    private string TWO_BRAINS_BUTTON;

    private Stack<string> sceneStack;

    // Use this for initialization
    public void Start()
    {
#if UNITY_EDITOR
#else
        DontDestroyOnLoad(transform.gameObject);
#endif
        STUDENT_BUTTON = "Student";
        PROFESSOR_BUTTON = "Professor";
        JOIN_LAB_BUTTON = "Join Lab";
        SOLO_BUTTON = "Solo";
        BACK_BUTTON = "Back";
        ONE_BRAIN_BUTTON = "OneBrain";
        TWO_BRAINS_BUTTON = "TwoBrains";

        sceneStack = new Stack<string>();
    }

    public void OnTap(string buttonName)
    {
        if (buttonName == STUDENT_BUTTON)
        {
            LoadStudentOrSoloScene();
        }
        else if (buttonName == PROFESSOR_BUTTON)
        {
            HandleProfessorTapped();
        }
        else if (buttonName == JOIN_LAB_BUTTON)
        {
            LoadEnterSessionIDScene();
        }
        else if (buttonName == SOLO_BUTTON)
        {
            HandleSoloTapped();
        }
        else if (buttonName == BACK_BUTTON)
        {
            HandleBackButton();
        }
        else if (buttonName == ONE_BRAIN_BUTTON)
        {
            LoadOneBrainScene();
        }
        else if (buttonName == TWO_BRAINS_BUTTON)
        {
            LoadTwoBrainsScene();
        }

    }

    private void LoadStudentOrSoloScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log("Inside LoadStudentOrSoloScene()");
        HTGazeManager gazeManager = HTGazeManager.Instance;
        if (gazeManager.Hit)
            WriteAdjustmentAngle(gazeManager.Position);
        else
            PlayerPrefs.SetFloat("adjustmentAngle", 0.0f);

        PushSceneName("StartAppScene");
        SceneManager.LoadScene("StudentOrSoloScene");
    }

    private void LoadEnterSessionIDScene()
    {
        HTGazeManager gazeManager = HTGazeManager.Instance;
        if (gazeManager.Hit)
            WriteAdjustmentAngle(gazeManager.Position);
        else
            PlayerPrefs.SetFloat("adjustmentAngle", 0.0f);

        PushSceneName("StudentOrSoloScene");
        SceneManager.LoadScene("EnterSessionIDScene");
    }

    private void HandleProfessorTapped()
    {
        PlayerPrefs.SetString("mode", "professor");
        HTGazeManager gazeManager = HTGazeManager.Instance;
        if (gazeManager.Hit)
            WriteAdjustmentAngle(gazeManager.Position);
        else
            PlayerPrefs.SetFloat("adjustmentAngle", 0.0f);

        PushSceneName("StartAppScene");
        SceneManager.LoadScene("BrainNumSelectScene");
    }

    private void HandleSoloTapped()
    {
        PlayerPrefs.SetString("mode", "solo");
        HTGazeManager gazeManager = HTGazeManager.Instance;
        if (gazeManager.Hit)
            WriteAdjustmentAngle(gazeManager.Position);
        else
            PlayerPrefs.SetFloat("adjustmentAngle", 0.0f);

        PushSceneName("StudentOrSoloScene");
        SceneManager.LoadScene("BrainNumSelectScene");
    }

    private void HandleBackButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("In HandleBackButton()");
        Debug.Log("Top of stack is " + sceneStack.Peek());
        HTGazeManager gazeManager = HTGazeManager.Instance;
        if (gazeManager.Hit)
            WriteAdjustmentAngle(gazeManager.Position);
        else
            PlayerPrefs.SetFloat("adjustmentAngle", 0.0f);

        SceneManager.LoadScene(PopSceneName());
    }

    private void PushSceneName(string name)
    {
        sceneStack.Push(name);
        Debug.Log(sceneStack.Peek());
    }

    private string PopSceneName()
    {
        return sceneStack.Pop();
    }

    private void WriteAdjustmentAngle(Vector3 gazePosition)
    {
        GameObject uiManager = GameObject.Find("UIManager");
        if (uiManager != null)
        {
            Vector3 canvasPosition = uiManager.transform.GetChild(0).position;
            float angle = Vector3.Angle(canvasPosition, gazePosition);
            if (canvasPosition.x < gazePosition.x)
                angle = -angle;

            PlayerPrefs.SetFloat("adjustmentAngle", angle);
        }
        else
        {
            PlayerPrefs.SetFloat("adjustmentAngle", 0.0f);
        }
    }

    private void LoadOneBrainScene()
    {
        PlayerPrefs.SetString("brainMode", "one");
        SceneManager.LoadScene("HoloBrainOne");
    }

    private void LoadTwoBrainsScene()
    {
        PushSceneName("BrainNumSelectScene");
        PlayerPrefs.SetString("brainMode", "two");
        SceneManager.LoadScene("HoloBrain");
    }
}