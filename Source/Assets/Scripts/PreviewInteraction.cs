using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class PreviewInteraction : MonoBehaviour, IFocusable {
    public Sprite[] frames;

    public void OnFocusEnter()
    {
        GameObject.Find("PreviewPanel").GetComponent<PreviewPlayer>().ChangePreview(frames);
    }

    public void OnFocusExit()
    {
        GameObject.Find("PreviewPanel").GetComponent<PreviewPlayer>().StopPreview();
    }
}
