using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ButtonAppearance : MonoBehaviour, IFocusable {

    // Use this for initialization

    public Sprite hoverSprite;
    public Sprite defaultSprite;
    public Sprite activeSprite;
    public bool oneButtonActiveInGroup;
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
        if(oneButtonActiveInGroup)
        {
            foreach(ButtonAppearance b in transform.parent.GetComponentsInChildren<ButtonAppearance>(true))
            {
                if (b.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    b.ResetButton();
                }
            }
        }
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
        if (renderer != null)
        {
            renderer.sprite = defaultSprite;
            activeState = false;
        }
    }

    public void OnFocusEnter()
    {
        if (activeState)
        {
            return;
        }
        hoverSound.Play();
        SetButtonHover();
    }

    public void OnFocusExit()
    {
        if(activeState)
        {
            return;
        }
        ResetButton();
    }
}
