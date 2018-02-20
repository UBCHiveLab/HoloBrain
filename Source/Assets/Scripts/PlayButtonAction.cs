using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonAction : MonoBehaviour {

    private const string FMRI_SLIDER = "CrossfadeSlider";
    private string[] PLAY_MODE_BUTTONS = new string[] {"faster-icon", "slower-icon"};
    private string[] PAUSE_MODE_BUTTONS = new string[] { "skip-one-icon", "skip-ten-icon", "back-one-icon", "back-ten-icon" };
    private List<GameObject> playButtons = new List<GameObject>();
    private List<GameObject> pauseButtons = new List<GameObject>();
    private ObjectNiftiSlider crossfadeSlider;

    // Use this for initialization
    void Start () {

        crossfadeSlider = GameObject.Find(FMRI_SLIDER).GetComponent<ObjectNiftiSlider>();
        foreach (string button in PLAY_MODE_BUTTONS)
        {
            playButtons.Add(GameObject.Find(button));
        }
        foreach (string button in PAUSE_MODE_BUTTONS)
        {
            pauseButtons.Add(GameObject.Find(button));
        }
        foreach (GameObject obj in playButtons)
        {
            obj.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnSelect()
    {
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
