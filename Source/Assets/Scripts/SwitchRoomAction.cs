using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SwitchRoomAction : CommandToExecute {

    public int sceneIndexOr;
    public string sceneName = "Main";
    private AudioSource audioSource;

    override public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        base.Start();
    }

    override protected Action Command()
    {
        return delegate
        {
            audioSource.Play();
            if (sceneIndexOr > 0)
            {
                SceneManager.LoadScene(sceneIndexOr);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        };
    }
}
