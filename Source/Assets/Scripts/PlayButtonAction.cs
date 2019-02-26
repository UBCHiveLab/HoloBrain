using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonAction : MonoBehaviour {

    private AudioSource audio;
    public GameObject crossfade;
    public List<GameObject> playButtons = new List<GameObject>();
    public List<GameObject> pauseButtons = new List<GameObject>();
    private ObjectNiftiSlider crossfadeSlider;

    // Use this for initialization
    void Start () {

        audio = GetComponent<AudioSource>();
        crossfadeSlider =crossfade.GetComponent<ObjectNiftiSlider>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnSelect()
    {
        audio.Play();
        crossfadeSlider.TogglePlay();

        if (crossfadeSlider.isPlaying)
        {
            foreach (GameObject obj in playButtons)
            {
                Debug.Log(obj);
               obj.SetActive(true);
            }

            foreach (GameObject obj in pauseButtons)
            {
                Debug.Log(obj);
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in playButtons)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in pauseButtons)
            {
                obj.SetActive(true);
            }
        }
    }
}
