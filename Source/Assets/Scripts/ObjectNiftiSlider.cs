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
    private const float initTimePerObject = 0.5f; // in s
    float timePerObject;
    float timeOnCurrentObject;
    
    public Transform fmriBrain;
    public Transform deltafMRIBrain;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < fmriBrain.childCount; i++)
        {
            GameObject child = fmriBrain.GetChild(i).gameObject;
            fmriGameObjects.Add(child);
            if(i == 0)
            {
                child.SetActive(true);
            } else
            {
                child.SetActive(false);
            }
            /*
            if (deltafMRIBrain.Find((i + 1).ToString()) != null) // If a corresponding deltaFMRI exists 
            {
                Debug.Log("not null");
                deltafMRIGameObjects.Add(deltafMRIBrain.Find((i + 1).ToString()).gameObject);
            } 
            else // If there is no corresponding deltaFMRI, have a corresponding empty object
            {
                deltafMRIGameObjects.Add(new GameObject());
            }*/
        }

        timePerObject = initTimePerObject;
        timeOnCurrentObject = 0f;

        fMRItext = GetComponentInChildren<Text>();
        fMRITotal = fmriGameObjects.Count.ToString();
  
        fMRItext.text = "1/" + fMRITotal;
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
            timeOnCurrentObject += Time.deltaTime;
            if (timeOnCurrentObject > timePerObject)
            {
                timeOnCurrentObject = 0;
                SliderUpdate(index + 1);
            }
        }

    }

    public void SlowDownPlayback()
    {
        timePerObject *= 2;
    }

    public void Skip(int numFrames)
    {
        SliderUpdate(index + numFrames);
    }

    public void Back(int numFrames)
    {
        SliderUpdate(index - numFrames);
    }

    public void SpeedUpPlayback()
    {
        timePerObject /= 2;
    }

    public void TogglePlay()
    {
        if (isPlaying)
        {
            isPlaying = false;
            timePerObject = initTimePerObject;
        }
        else
            isPlaying = true;
    }

    private void UpdateSliderText(int newIndex) {
        fMRItext.text = (newIndex + 1).ToString() + "/" + fMRITotal;
    }

    private void SwitchfMRIObjects(int newIndex)
    {
        fmriGameObjects[newIndex].SetActive(true);
        fmriGameObjects[index].SetActive(false);

       // deltafMRIGameObjects[newIndex].SetActive(true);
        //deltafMRIGameObjects[index].SetActive(false);
    }

    public void SliderUpdate(int newIndex)
    {
        int tempIndex = newIndex;
        if (newIndex >= fmriGameObjects.Count)
        {
            tempIndex = newIndex%fmriGameObjects.Count;
        }
        else if(newIndex < 0)
        {
            tempIndex = fmriGameObjects.Count + newIndex;
        }

        SwitchfMRIObjects(tempIndex);
        UpdateSliderText(tempIndex);
        index = tempIndex;
    }
}
