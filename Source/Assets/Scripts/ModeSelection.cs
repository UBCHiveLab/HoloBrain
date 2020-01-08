// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour {

    private string STUDENT_BUTTON;
    private string PROFESSOR_BUTTON;
    private string JOIN_LAB_BUTTON;
    private string SOLO_BUTTON;
    private string BACK_BUTTON;

    private Stack<string> sceneStack;

	// Use this for initialization
	void Start () {
        STUDENT_BUTTON = "Student";
        PROFESSOR_BUTTON = "Professor";
        JOIN_LAB_BUTTON = "Join Lab";
        SOLO_BUTTON = "Solo";
        BACK_BUTTON = "Back";

        sceneStack = new Stack<string>();
	}

    public void OnTap(string buttonName)
    {   
        if (buttonName == STUDENT_BUTTON)
        {
            LoadStudentOrSoloScene();
        }else if (buttonName == PROFESSOR_BUTTON)
        {
            HandleProfessorTapped();
        }else if (buttonName == JOIN_LAB_BUTTON)
        {
            LoadEnterSessionIDScene();
        }else if (buttonName == SOLO_BUTTON)
        {
            HandleSoloTapped();
        }else if (buttonName == BACK_BUTTON)
        {
            HandleBackButton();
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
        SceneManager.LoadScene("StudentOrSoloScene", LoadSceneMode.Single);
    }

    private void LoadEnterSessionIDScene()
    {
        HTGazeManager gazeManager = HTGazeManager.Instance;
        if (gazeManager.Hit)
            WriteAdjustmentAngle(gazeManager.Position);
        else
            PlayerPrefs.SetFloat("adjustmentAngle", 0.0f);

        PushSceneName("StudentOrSoloScene");
        SceneManager.LoadScene("EnterSessionIDScene", LoadSceneMode.Single);
    }

    private void HandleProfessorTapped() {
        PlayerPrefs.SetString("mode", "professor");
 
        SceneManager.LoadScene("EducationalRoom", LoadSceneMode.Single);
    }

    private void HandleSoloTapped()
    {
        PlayerPrefs.SetString("mode", "solo");
        SceneManager.LoadScene("EducationalRoom", LoadSceneMode.Single);
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
        
        SceneManager.LoadScene(PopSceneName(), LoadSceneMode.Single);
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
}
