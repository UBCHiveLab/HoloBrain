using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSwap : MonoBehaviour {

    public Sprite StartIcon;
    public Sprite EndIcon;
    bool StartIconisOn;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        StartIconisOn = true;
    }

    public void ToggleButtonImage()
    {
        if (StartIconisOn)
        {
            if (EndIcon != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = EndIcon;

            }
            StartIconisOn = false;
        }
        else
        {
            if (StartIcon != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = StartIcon;

            }
            StartIconisOn = true;
        }

    }


}


