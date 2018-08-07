using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBrainCellModeButtonAction : MonoBehaviour
{
    public bool isStatic;
    private AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void OnSelect()
    {
        audio.Play();
        BrainCellUIManager.Instance.SetStaticMode(isStatic);
    }
}
