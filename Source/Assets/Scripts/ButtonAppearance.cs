using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAppearance : MonoBehaviour {

    // Use this for initialization

    public Sprite hoverSprite;
    public Sprite defaultSprite;
    public Sprite activeSprite;
    private SpriteRenderer renderer;
    private bool activeState;
    private AudioSource hoverSound;
    
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.Log("no sprite renderer on button where expected");
        } else
        {
            ResetButton();
        }
        activeState = false;
        hoverSound = GameObject.Find("menu").GetComponent<AudioSource>();
    }

    void OnDisable()
    {
        activeState = false;
        renderer.sprite = defaultSprite;
    }

    public void SetButtonHover()
    {
        renderer.sprite = hoverSprite;
    }

    public void SetButtonActive()
    {
        renderer.sprite = activeSprite;
        activeState = true;
    }

    public void ToggleButtonActive()
    {
        if (activeState == false)
        {
            SetButtonActive();
        }
        else
        {
            ResetButton();
        }
    }

    public void ResetButton()
    {
        renderer.sprite = defaultSprite;
        activeState = false;
    }

    public void OnStartGaze()
    {
        if (activeState)
        {
            return;
        }
        hoverSound.Play();
        SetButtonHover();
    }

    public void OnEndGaze()
    {
        if(activeState)
        {
            return;
        }
        ResetButton();
    }
}
