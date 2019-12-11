using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class RecordEvents : CommandToExecute, IFocusable {

    private DataRecorder dataRecorder;

    override public void Start()
    {
        dataRecorder = GameObject.Find("PreLoad").GetComponent<DataRecorder>();
        base.Start();
    }

    protected override Action Command()
    {
        return delegate
        {
            if (dataRecorder != null)
            {
                dataRecorder.QueueMessage(gameObject.name + " click");
            } else
            {
                Debug.Log("data recorder null, not recording click on " + gameObject.name);
            }
        };
    }

    public void OnFocusEnter()
    {
        if(dataRecorder != null)
        {
            dataRecorder.QueueMessage(gameObject.name + " focus enter");
        } else
        {
            Debug.Log("data recorder null, not recording focus enter on " + gameObject.name);
        }
    }

    public void OnFocusExit()
    {
        if(dataRecorder != null)
        {
            dataRecorder.QueueMessage(gameObject.name + " focus exit");
        } else
        {
            Debug.Log("data recorder null, not recording focus exit on " + gameObject.name);
        }
    }
}
