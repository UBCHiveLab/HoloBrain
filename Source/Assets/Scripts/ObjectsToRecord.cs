using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsToRecord : MonoBehaviour {

    DataRecorder recorder;
    // Use this for initialization
    void Start()
    {
        recorder = (DataRecorder)FindObjectOfType(typeof(DataRecorder));
    }

    private void Update()
    {
        //want to make recording interval adjustable
        if(recorder != null)
        {
            recorder.QueueMessage(gameObject.name + ";" + gameObject.transform.position.ToString("F3") + ";" + gameObject.transform.rotation.ToString("F3"));
        }
    }
}
