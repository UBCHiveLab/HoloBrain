// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

﻿using UnityEngine;
using System.Collections;

using HoloToolkit.Unity;
#if WINDOWS_UWP
    using System;
    using System.Collections.Concurrent;
    using Windows.Storage;
    using Windows.Storage.Streams;
#endif
using System.IO;
using System.Text;

/// <summary>
/// GestureManager creates a gesture recognizer and signs up for a tap gesture.
/// When a tap gesture is detected, GestureManager uses GazeManager to find the game object.
/// GestureManager then sends a message to that game object.
/// </summary>
[RequireComponent(typeof(HTGazeManager))]
public class HTGestureManager : Singleton<HTGestureManager>
{
    /// <summary>
    /// To select even when a hologram is not being gazed at,
    /// set the override focused object.
    /// If its null, then the gazed at object will be selected.
    /// </summary>
    public GameObject OverrideFocusedObject
    {
        get; set;
    }

#if WINDOWS_UWP
    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
    StorageFile dataFile;
    IRandomAccessStream stream;
    private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
    private int fileIndex;
    bool recordingData = false;
    int dataTimer = 0;
#endif

    //data collection variables start
    /*
    private string filename;
    private StreamWriter sw;
    private string path;
    private FileStream fs;*/

    //data collection variables end

    private UnityEngine.XR.WSA.Input.GestureRecognizer gestureRecognizer;
    private GameObject focusedObject;
    private GameObject brainParts;
    void Start()
    {
        // Create a new GestureRecognizer. Sign up for tapped events.

//initializing data collection
#if WINDOWS_UWP
        startRecordingData();
#endif

        gestureRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(UnityEngine.XR.WSA.Input.GestureSettings.Tap);

        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;

        // Start looking for gestures.
        gestureRecognizer.StartCapturingGestures();

        brainParts = GameObject.Find("BrainParts");
    }

    private void GestureRecognizer_TappedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (focusedObject != null)
        {
            focusedObject.SendMessage("OnSelect");
#if WINDOWS_UWP
            queueMessage(focusedObject.name + "OnSelect"); 
#endif
            //sw.WriteLine(focusedObject.name + " OnSelect");
        }
        //UNCOMMENT THIS FOR GAZE MARKER
        else if (brainParts != null)
        {
            brainParts.SendMessage("OnEmptyTap");
#if WINDOWS_UWP
            queueMessage("BrainParts OnEmptyTap"); 
#endif
            //sw.WriteLine("BrainParts OnEmptyTap");
        }
    }

#if WINDOWS_UWP
    async void startRecordingData()
    {
        string filename = System.DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm") + ".txt";
        dataFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        stream = await dataFile.OpenAsync(FileAccessMode.ReadWrite);
        recordingData = true;
    }

    async void writeToDataFile()
    {
        if(recordingData) 
        {
            string text;
            using(var outputStream = stream.GetOutputStreamAt((UInt64)fileIndex)) {
                using(var dataWriter = new DataWriter(outputStream)) {
                    while(messages.Count > 0) { 
                        if(messages.TryDequeue(out text))
                        {
                            Debug.Log("writing to file " + text);               
                            dataWriter.WriteString(text + "\n");
                            fileIndex = fileIndex + text.Length + 1;
                        }
                    }
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
        }
    }

    void queueMessage(string text) {
        messages.Enqueue(text);
    }
#endif

    void LateUpdate()
    {
        GameObject oldFocusedObject = focusedObject;

        if (HTGazeManager.Instance.Hit &&
            OverrideFocusedObject == null &&
            HTGazeManager.Instance.HitInfo.collider != null)
        {
            // If gaze hits a hologram, set the focused object to that game object.
            // Also if the caller has not decided to override the focused object.
            focusedObject = HTGazeManager.Instance.HitInfo.collider.gameObject;
        }
        else
        {
            // If our gaze doesn't hit a hologram, set the focused object to null or override focused object.
            focusedObject = OverrideFocusedObject;
        }

        if (focusedObject != oldFocusedObject)
        {
            if (oldFocusedObject != null)
            {
                oldFocusedObject.SendMessageUpwards("OnEndGaze");
#if WINDOWS_UWP
                queueMessage(oldFocusedObject.name + " OnEndGaze"); 
#endif
                //sw.WriteLine(oldFocusedObject.name + " OnEndGaze");
            }
            if (focusedObject != null)
            {
                focusedObject.SendMessageUpwards("OnStartGaze");
#if WINDOWS_UWP
                queueMessage(focusedObject.name + " OnStartGaze"); 
#endif
                //sw.WriteLine(oldFocusedObject.name + " OnStartGaze");
            }

            // If the currently focused object doesn't match the old focused object, cancel the current gesture.
            // Start looking for new gestures.  This is to prevent applying gestures from one hologram to another.
            gestureRecognizer.CancelGestures();
            gestureRecognizer.StartCapturingGestures();
        }
#if WINDOWS_UWP
        if(dataTimer >= 60) 
        {
            writeToDataFile();
            dataTimer = 0;
        } else 
        {
            dataTimer += 1;
        }
#endif
    }


    void OnDestroy()
    {
        // gestureRecognizer.StopCapturingGestures();
        // gestureRecognizer.TappedEvent -= GestureRecognizer_TappedEvent;
#if WINDOWS_UWP
        stream.Dispose();
#endif
    }
}