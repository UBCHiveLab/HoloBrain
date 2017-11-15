using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCrossfader : MonoBehaviour {

    public List<GameObject> list;
    int index = 0;
    float frameTime; // # of seconds spent on one object (not including crossfade into next object)
    float crossfadeTimer; 
    float crossfadeTime = 2; // # of seconds spent crossfading to next object
    Material currentMat;
    Material nextMat;
    bool isCrossfading;

	// Use this for initialization
	void Start () {
        currentMat = list[0].GetComponent<MeshRenderer>().material;
        Reset();
	}

    void Reset()
    {
        CancelInvoke();
        index = 0;
        currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 0);
        currentMat = list[index].GetComponent<MeshRenderer>().material;
        currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 1);
        nextMat = list[index + 1].GetComponent<MeshRenderer>().material;
        crossfadeTimer = crossfadeTime;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.C))
        {
            isCrossfading = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if (isCrossfading)
        {
            Crossfade();
        }

	}

    void Play()
    {
        InvokeRepeating("MakeCrossfadingForwardTrue", 0.1f, frameTime+crossfadeTime);
    }

    void MakeCrossfadingTrue()
    {
        isCrossfading = true;
    }

    void Crossfade()
    {
        if (index == list.Count - 1) // if this is the last object in the list 
        {
            print("this is the last frame");
            isCrossfading = false;
            CancelInvoke();
            return;
        }
        crossfadeTimer -= Time.deltaTime;
        if (crossfadeTimer <= 0)
        {
            currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, 0);
            nextMat.color = new Color(nextMat.color.r, nextMat.color.g, nextMat.color.b, 1);
            currentMat = nextMat;
            index++;
            nextMat = list[index + 1].GetComponent<MeshRenderer>().material;
            
            crossfadeTimer = crossfadeTime;
            isCrossfading = false;
        }
        else
        {
            // fade out the current object
            currentMat.color = new Color(currentMat.color.r, currentMat.color.g, currentMat.color.b, crossfadeTimer / crossfadeTime);
            // fade in the next object
            nextMat.color = new Color(nextMat.color.r, nextMat.color.g, nextMat.color.b, (1-crossfadeTimer / crossfadeTime));
        }
    }
}
