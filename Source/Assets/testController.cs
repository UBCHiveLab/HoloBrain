// Draws a line in the scene view going through a point 200 pixels
// from the lower-left corner of the screen
using UnityEngine;
using System.Collections;

public class testController : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
    }
}