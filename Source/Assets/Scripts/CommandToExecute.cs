using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(ButtonCommands))]
public abstract class CommandToExecute : MonoBehaviour {

    public virtual void Start()
    {
        ButtonCommands bc = gameObject.GetComponent<ButtonCommands>();
        if (bc == null)
        {
            Debug.Log("no button commands on " + gameObject.name);
        } else
        {
            bc.AddCommand(Command());
        }
    }

    protected abstract Action Command();
}
