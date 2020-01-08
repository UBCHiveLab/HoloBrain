// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
//using for data collection start:
using System.IO;
using System.Text;
//using for data collection end

public class GazeGestureManager : MonoBehaviour
{

    //data collection variables start
    private string filename;
    private StreamWriter sw;
    private string path;
    private FileStream fs;
    //data collection variables end

    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    UnityEngine.XR.WSA.Input.GestureRecognizer recognizer;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        //initializing data collection
        filename = System.DateTime.UtcNow.ToString() + ".txt";
        path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("creating file at: " + path);
        if (!Directory.Exists(path))
        {
            File.Create(path);
        }
        FileStream fs = new FileStream(path, FileMode.Open);
        sw = new StreamWriter(fs);

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect");
                sw.WriteLine(FocusedObject.name + " OnSelect");
            }
        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again and send the OnStartGaze and OnEndGaze messages.
        if (FocusedObject != oldFocusObject)
        {
            if(oldFocusObject != null)
            {
                oldFocusObject.SendMessageUpwards("OnEndGaze");
                sw.WriteLine(oldFocusObject.name + " OnEndGaze");
            }
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnStartGaze");
                sw.WriteLine(FocusedObject.name + " OnStartGaze");
            }
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }


}