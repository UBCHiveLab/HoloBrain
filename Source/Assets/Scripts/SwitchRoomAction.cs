using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchRoomAction : MonoBehaviour {

    public int sceneIndex = -1;
    public string sceneName;
    private AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void OnSelect()
    {
        audio.Play();
        if (sceneIndex != -1)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }   
    }
}
