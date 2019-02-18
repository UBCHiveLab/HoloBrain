using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewInteraction : MonoBehaviour {
    public Sprite[] frames;

    public void OnStartGaze()
    {
        Debug.Log("gaze enter preview: " + gameObject.name);
        GameObject.Find("PreviewPanel").GetComponent<PreviewPlayer>().ChangePreview(frames);
    }

    public void OnEndGaze()
    {
        Debug.Log("gaze exit preview: " + gameObject.name);
        GameObject.Find("PreviewPanel").GetComponent<PreviewPlayer>().StopPreview();
    }
}
