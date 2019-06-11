using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderToGameObject : MonoBehaviour {
    
    public Slider sliderInstance;
    public Transform sphereInstance;
    public Transform handleInstance;
    private Vector3 mOffset;
    private float mZCoord;

    // Use this for initialization
    void Start()
    {
        sliderInstance.minValue = 0;
        sliderInstance.maxValue = 1;

    }

    // Update is called once per frame
    void Update()
    {
        sphereInstance.position = handleInstance.position;
        sphereInstance.parent = handleInstance.parent;

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
         
    void OnMouseDown()

    {

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;



        // Store offset = gameobject world pos - mouse world pos

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

    }



    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }



    void OnMouseDrag()

    {

        transform.position = GetMouseAsWorldPoint() + mOffset;

    }

}

