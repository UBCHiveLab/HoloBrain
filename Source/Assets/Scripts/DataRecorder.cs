using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;
#if WINDOWS_UWP
    using System.Collections.Concurrent;
    using Windows.Storage;
    using Windows.Storage.Streams;
#endif


public class DataRecorder : MonoBehaviour {

#if WINDOWS_UWP
    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
    StorageFile dataFile;
    IRandomAccessStream stream;
    private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
    private int fileIndex;
    bool recordingData = false;
    int dataTimer = 0;
    int dataInterval = 60;
    int objectTrackingTimer = 0;
    int objectTrackingInterval = 1;
#endif
    Keyboard keyboard;
    public static string keyboardText = "";
    private bool isNameSet = false;
    private bool isConditionSet = false;
    public bool IsNameSet { get { return isNameSet; } private set { isNameSet = value; } }
    public bool IsConditionSet { get { return isConditionSet; } private set { isConditionSet = value; } }
    private bool presentedNameKeyboard = false;
    private bool presentedConditionKeyboard = false;

    // Use this for initialization
    void Start()
    {
        Keyboard.Instance.OnTextSubmitted += participantHandler;
    }

    private void participantHandler(object sender, EventArgs e)
    {
        Debug.Log("participant event");
        keyboard = (Keyboard)sender;
        keyboardText = keyboard.InputField.textComponent.text;
        IsNameSet = true;
        keyboard.InputField.placeholder.GetComponent<Text>().text = "Enter condition...";
        Debug.Log("finished participant event");
    }
    
    private void conditionHandler(object sender, EventArgs e)
    {
        Debug.Log("condition event");
        keyboard = (Keyboard)sender;
        keyboardText += "_" + keyboard.InputField.textComponent.text;
        IsConditionSet = true;
    }

#if WINDOWS_UWP
    async void startRecordingData(string participant)
    {
        //Debug.Log("recording data: " + participant);
        string filename = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + participant + ".txt";
        dataFile = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        stream = await dataFile.OpenAsync(FileAccessMode.ReadWrite);
        recordingData = true;
    }

    async void writeToDataFile()
    {
        if(recordingData) 
        {
            //Debug.Log("writing data");
            string text;
            using(var outputStream = stream.GetOutputStreamAt((UInt64)fileIndex)) {
                using(var dataWriter = new DataWriter(outputStream)) {
                    while(messages.TryDequeue(out text)) {    
                        //Debug.Log("message: " + text);
                        dataWriter.WriteString(text + "\n");
                        fileIndex = fileIndex + text.Length + 1;
                    }
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                    //Debug.Log("Finished writing");
                }
            }
        }
    }

#endif

    public void QueueMessage(string text)
    {
#if WINDOWS_UWP
        messages.Enqueue(Time.time + ";" + text + "\r\n");
#endif
    }


    // Update is called once per frame
    void Update () {
        if(!IsNameSet && !presentedNameKeyboard)
        {
            Debug.Log("presenting name keyboard");
            Keyboard.Instance.PresentKeyboard(Keyboard.LayoutType.Alpha);
            presentedNameKeyboard = true;
        }
        if(!IsConditionSet && !presentedConditionKeyboard && presentedNameKeyboard && IsNameSet)
        {
            Keyboard.Instance.OnTextSubmitted -= participantHandler;
            Keyboard.Instance.OnTextSubmitted += conditionHandler;
            Debug.Log("presenting condition keyboard");
            Keyboard.Instance.PresentKeyboard(Keyboard.LayoutType.Alpha);
            presentedConditionKeyboard = true;
        }
#if WINDOWS_UWP
        if(IsNameSet && IsConditionSet && !recordingData) {
            startRecordingData(keyboardText);
            keyboard.gameObject.SetActive(false);
        }
                
        if(recordingData) {
            if(dataTimer >= dataInterval) 
            {
                writeToDataFile();
                dataTimer = 0;
            } else 
            {
                dataTimer += 1;
            }
        }

#endif
    }

    private void OnDestroy()
    {
#if WINDOWS_UWP
        stream.Dispose();
        recordingData = false;
#endif
    }
}
