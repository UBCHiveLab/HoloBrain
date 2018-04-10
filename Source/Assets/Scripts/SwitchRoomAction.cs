using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchRoomAction : MonoBehaviour {

    public int sceneToSwitchTo;
    private AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void OnSelect()
    {
        audio.Play();
        SceneManager.LoadScene(sceneToSwitchTo);
    }
}
