using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLookAt : MonoBehaviour
{
    private LineRenderer line;                           // Line Renderer
    public GameObject beamTarget;
    public GameObject beamArrow;
    //public float distanceThreshold = 1.0f;
    //float myDistance = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        // Add a Line Renderer to the GameObject
        line = this.gameObject.AddComponent<LineRenderer>();
        line.name = "Line";
        // Set the width of the Line Renderer
        line.SetWidth(0.01F, 0.01F);
        // Set the number of vertex fo the Line Renderer
        line.SetVertexCount(2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(beamTarget.transform.position);
        
        line.SetPosition(0, beamArrow.transform.position);
        line.SetPosition(1, beamTarget.transform.position);
        // Update position of the two vertex of the Line Renderer
        //myDistance = Vector3.Distance(targetCube.transform.position, transform.position);
        //Debug.Log("Current Distance is: " + myDistance);
        /*
        if (myDistance < distanceThreshold)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targetCube.transform.position);
        }
        */
    }
}