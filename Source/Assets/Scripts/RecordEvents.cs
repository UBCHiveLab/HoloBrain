using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class RecordEvents : CommandToExecute, IFocusable {

    private DataRecorder dataRecorder;

    public bool isButton;

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
                if (isButton)
                {
                    dataRecorder.QueueMessage(gameObject.name + "button click");
                } else
                {
                    dataRecorder.QueueMessage(gameObject.name + " click");
                }
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
            if(isButton)
            {

                dataRecorder.QueueMessage(gameObject.name + "button focus enter");
            } else
            {
                dataRecorder.QueueMessage(gameObject.name + " focus enter");
            }
        } else
        {
            Debug.Log("data recorder null, not recording focus enter on " + gameObject.name);
        }
    }

    public void OnFocusExit()
    {
        if(dataRecorder != null)
        {
            if(isButton)
            {
                dataRecorder.QueueMessage(gameObject.name + "button focus exit");
            } else
            {
                dataRecorder.QueueMessage(gameObject.name + " focus exit");
            }
        } else
        {
            Debug.Log("data recorder null, not recording focus exit on " + gameObject.name);
        }
    }
}
