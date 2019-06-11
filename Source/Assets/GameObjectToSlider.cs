using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectToSlider : MonoBehaviour {

    public Slider sliderInstance;
    public Transform gameObjectInstance;
    private float newSliderValue;
    public enum ObjectAxis { X, Y, Z };
    public ObjectAxis selectedAxis;
    private float localPositionOnAxis;
    public float maxGameObjectPosition;
    public float minGameObjectPosition;

    // Use this for initialization
    void Start()
    {
        sliderInstance.minValue = 0;
        sliderInstance.maxValue = 100;

    }

    // Update is called once per frame
    void Update()
    {

        if (selectedAxis == ObjectAxis.X) { localPositionOnAxis = gameObjectInstance.localPosition.x; }
        else if (selectedAxis == ObjectAxis.Y) { localPositionOnAxis = gameObjectInstance.localPosition.y; }
        else { localPositionOnAxis = gameObjectInstance.localPosition.z; }
        if (maxGameObjectPosition < 0) { maxGameObjectPosition = Mathf.Abs(maxGameObjectPosition); }
        if (localPositionOnAxis > 0) { localPositionOnAxis = (-1) * localPositionOnAxis; }



        newSliderValue = ( localPositionOnAxis + maxGameObjectPosition);

        newSliderValue = (Mathf.Abs(newSliderValue))/0.16f;

        sliderInstance.value = (newSliderValue);
        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            sliderInstance.value = 0.6F;
        }
        */

    }
    
    public void OnValueChanged(float value)
    {
    }

}
