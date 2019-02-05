using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchRoomAction : MonoBehaviour {

    public int sceneIndexOr;
    public string sceneName = "Main";
    private AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void OnSelect()
    {
        audio.Play();
        if (sceneIndexOr > 0)
        {
            SceneManager.LoadScene(sceneIndexOr);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }   
    }
}
