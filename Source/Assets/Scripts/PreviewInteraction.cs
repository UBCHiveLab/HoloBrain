using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewInteraction : MonoBehaviour {
    public Sprite[] frames;

    public void OnStartGaze()
    {
        GameObject.Find("PreviewPanel").GetComponent<PreviewPlayer>().ChangePreview(frames);
    }

    public void OnEndGaze()
    {
        GameObject.Find("PreviewPanel").GetComponent<PreviewPlayer>().StopPreview();
    }
}
