using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectCrossfadeSlider : MonoBehaviour {

    public List<GameObject> list;
    int index = 0;
    Material currentMat;
    Material nextMat;
    bool isPlaying;
    float speed = 0.5f;

    Slider slider;
    public Transform parentOfArray;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < parentOfArray.childCount; i++)
        {

            GameObject child = parentOfArray.GetChild(i).gameObject;
            GameObject grandChild = child.transform.GetChild(0).gameObject;

            
            Material grandchildMat = grandChild.GetComponent<MeshRenderer>().material;
            grandchildMat.color = new Color(grandchildMat.color.r, grandchildMat.color.g, grandchildMat.color.b, 0f);
            //if (i % 2 == 0) {
            //    grandChild.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            //} else{
            //    grandChild.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);

            //}
            list.Add(grandChild);
        }

        slider = GetComponent<Slider>();
        slider.maxValue = list.Count-1;
        slider.onValueChanged.AddListener(delegate { SliderUpdate(); });
        currentMat = list[0].GetComponent<MeshRenderer>().material;
        currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 1);
        nextMat = list[1].GetComponent<MeshRenderer>().material;
        nextMat.color = new Color(nextMat.color.r, nextMat.color.g, nextMat.color.b, 0);
    }
    

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePlay();
        }

        if (isPlaying)
        {
            slider.value += speed * Time.deltaTime;
            if (slider.value >= slider.maxValue) {
                isPlaying = false;
            }
        }
        //print(currentMat.)
	}

    public void TogglePlay()
    {
        if (isPlaying)
            isPlaying = false;
        else
            isPlaying = true;
    }

    public void SliderUpdate()
    {
        int newIndex = Mathf.FloorToInt(slider.value);
        print(newIndex);
        if (newIndex == list.Count-1)
        {
            index = newIndex;
            return;
        }

        // if slider is a whole number, just display the corresponding indexed object in the list at full opacity
        if (slider.value % 1 == 0)
        {

            index = newIndex;
            currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 0);
            currentMat = list[index].GetComponent<MeshRenderer>().material;
            currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 1);
        }
        // if it's not a whole number, display the appropriate crossfade between the 2 indexes
        else
        {
            index = newIndex;
            currentMat = list[index].GetComponent<MeshRenderer>().material;
            nextMat = list[index + 1].GetComponent<MeshRenderer>().material;
            float remainder = slider.value - (float)index;
            currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 1 - remainder);
            nextMat.color = new Color(nextMat.color.r, nextMat.color.g, nextMat.color.b, remainder);
        }
    }

}
