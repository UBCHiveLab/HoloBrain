using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectNiftiSlider : MonoBehaviour
{

    //public List<GameObject> list;
    public List<List<Material>> fmriList = new List<List<Material>>();
    int index = 0;

    List<Material> currentMats = new List<Material>();
    List<Material> nextMats = new List<Material>();

    private List<GameObject> fmriGameObjects = new List<GameObject>();
    private List<GameObject> deltafMRIGameObjects = new List<GameObject>();

    private Text fMRItext;
    private string fMRITotal;

    public bool isPlaying;
    float speed = 5f;
    private const float initTimePerObject = 250f; // in ms
    float timePerObject;

    Slider slider;
    public Transform fmriBrain;
    public Transform deltafMRIBrain;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < fmriBrain.childCount; i++)
        {

            GameObject child = fmriBrain.GetChild(i).gameObject;
            Debug.Log(child.name);
            fmriGameObjects.Add(child);

            if (deltafMRIBrain.Find((i + 1).ToString()) != null) // If a corresponding deltaFMRI exists 
            {
                Debug.Log("not null");
                deltafMRIGameObjects.Add(deltafMRIBrain.Find((i + 1).ToString()).gameObject);
            } 
            else // If there is no corresponding deltaFMRI, have a corresponding empty object
            {
                deltafMRIGameObjects.Add(new GameObject());
            }
        }

        timePerObject = initTimePerObject;
        slider = GetComponent<Slider>();
        fMRItext = GetComponentInChildren<Text>();
        fMRITotal = fmriGameObjects.Count.ToString();
  
        fMRItext.text = "1/" + fMRITotal;
        slider.maxValue = fmriGameObjects.Count * timePerObject;
        slider.onValueChanged.AddListener(delegate { SliderUpdate(); });

    }


    // Update is called once per frame
    void Update()
    {
        //if ((Input.GetKeyDown(KeyCode.P)) || (voiceRecognitionKeywords.TryGetValue("Play", out keywordAction)))
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePlay();
        }

        if (isPlaying)
        {
            slider.value += Time.deltaTime * 1000;  
            if (slider.value >= slider.maxValue)
            {
                slider.value = slider.minValue;
            }
        }
    }

    public void SlowDownPlayback()
    {
        timePerObject *= 2;
        slider.maxValue = fmriGameObjects.Count * timePerObject;
        slider.value = index * timePerObject;
    }

    public void Skip(int numFrames)
    {
        int newIndex = Mathf.Min(index + numFrames, fmriGameObjects.Count);
        slider.value = index * timePerObject;
        if (newIndex != index)
        {
            SwitchfMRIObjects(newIndex);
            index = newIndex;
            UpdateSliderText();
        }
    }

    public void Back(int numFrames)
    {
        int newIndex = Mathf.Max(0, index - numFrames);
        slider.value = index * timePerObject;
        if (newIndex != index)
        {
            SwitchfMRIObjects(newIndex);
            index = newIndex;
            UpdateSliderText();
        }
    }

    public void SpeedUpPlayback()
    {
        timePerObject /= 2;
        slider.maxValue = fmriGameObjects.Count * timePerObject;
        slider.value = index * timePerObject;
    }

    public void TogglePlay()
    {
        if (isPlaying)
        {
            isPlaying = false;
            timePerObject = initTimePerObject;
            slider.maxValue = fmriGameObjects.Count * timePerObject;
        }
        else
            isPlaying = true;
    }

    private void UpdateSliderText() {

        fMRItext.text = (index + 1).ToString() + "/" + fMRITotal;

    }

    private void SwitchfMRIObjects(int newIndex)
    {
        fmriGameObjects[newIndex].SetActive(true);
        fmriGameObjects[index].SetActive(false);

        deltafMRIGameObjects[newIndex].SetActive(true);
        deltafMRIGameObjects[index].SetActive(false);
    }

    public void SliderUpdate()
    {
        int newIndex = Mathf.FloorToInt(slider.value / timePerObject);
        print(slider.value);
        print(newIndex);


        // if slider is a whole number, just display the corresponding indexed object in the list at full opacity
        if ( newIndex != index)
        {

            //Debug.Log("Current index: " + index.ToString() + "\n New index: " + newIndex.ToString());
            //Debug.Log("Old object active: " + fmriGameObjects[index].activeSelf + "\n New object active: " + fmriGameObjects[newIndex].activeSelf);

            SwitchfMRIObjects(newIndex);

            //adding 1 so that list starts from 1 
           // Debug.Log("Old object active: " + fmriGameObjects[index].activeSelf + "\n New object active: " + fmriGameObjects[newIndex].activeSelf);

            index = newIndex;
            UpdateSliderText();

        }

    }
}
