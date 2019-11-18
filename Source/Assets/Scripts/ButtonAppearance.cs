using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class ButtonAppearance : MonoBehaviour, IFocusable {

    // Use this for initialization

    public Sprite hoverSprite;
    public Sprite defaultSprite;
    public Sprite activeSprite;
    public Sprite disabledSprite;
    public bool oneButtonActiveInGroup = false;
    public bool resetOnSecondClick = false;
    private bool buttonDisabled = false;
    private SpriteRenderer renderer;
    private Image image;
    private bool activeState;
    private AudioSource hoverSound;
    
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            image = GetComponent<Image>();
            if(image == null)
            {
                Debug.Log("no sprite renderer or Image on button where expected");
            }
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
        changeSprite(defaultSprite);
    }

    public void SetButtonHover()
    {
        if(!buttonDisabled)
        {
            changeSprite(hoverSprite);
        }
    }

    public void SetButtonActive()
    {
        if(!buttonDisabled) {
            //reset all other buttons in the group
            if(oneButtonActiveInGroup)
            {
                foreach(Transform t in transform.parent)
                {
                    if (t.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                    {
                        ButtonAppearance b = t.GetComponent<ButtonAppearance>();
                        if(b != null)
                        {
                            b.ResetButton();
                        }
                    }
                }
            }
            //"unpress" the button
            if (activeState && resetOnSecondClick)
            {
                Debug.Log("resetting button: " + activeState + " " + resetOnSecondClick);
                ResetButton();
            }
            else
            {
                Debug.Log("setting button Active");
                changeSprite(activeSprite);
                activeState = true;
            }
        }
    }

    public void SetButtonDisabled()
    {
        buttonDisabled = true;
        changeSprite(disabledSprite);
    }
    
    public void SetButtonEnabled()
    {
        buttonDisabled = false;
        changeSprite(defaultSprite);
    }

    public void ToggleButtonActive()
    {
        if(!buttonDisabled)
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
    }

    public void ResetButton()
    {
        if(!buttonDisabled)
        {
            changeSprite(defaultSprite);
            activeState = false;
        }
    }

    public void OnFocusEnter()
    {
        if(!buttonDisabled)
        {
            if (activeState)
            {
                return;
            }
            hoverSound.Play();
            SetButtonHover();
        }
    }

    public void OnFocusExit()
    {
        if(!buttonDisabled)
        {
            if (activeState)
            {
                return;
            }
            ResetButton();
        }
    }

    private void changeSprite(Sprite sprite)
    {
        if (renderer != null)
        {
            renderer.sprite = sprite;
        }
        else if (image != null)
        {
            image.sprite = sprite;
        }
    }
}
