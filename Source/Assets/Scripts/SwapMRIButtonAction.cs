using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapMRIButtonAction : CommandToExecute {
    
    private GameObject container;
    private Image appearance;

    public MoveClippingPlane moveClippingPlane;
    public Transform MRICollection;
    public Transform mriSlice;
    public bool isWall;
    public bool vertical;
    public Color highlightColor = new Color(1f, 1f, 1f, 1f);
    public Color defaultColor = new Color(0.5411f, 0.5411f, 0.5411f, 0.5411f);

	// Use this for initialization
	override public void Start () {
        if(isWall)
        {
            appearance = GetComponent<Image>();
            MRICollection.GetComponent<MRIEvents>().HighlightMRIEvent += HighlightWallButton;
        }
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void HighlightWallButton(string mriName) {
        if(mriName == mriSlice.name)
        {
            appearance.color = highlightColor;
        } else
        {
            appearance.color = defaultColor;
        }
    }

    override protected Action Command()
    {
        return delegate
        {
            if (vertical)
            {
                Debug.Log("Sending message to move clipping plane: " + moveClippingPlane.transform.InverseTransformPoint(mriSlice.position).x);
                moveClippingPlane.changePlaneXPosition(MRICollection.InverseTransformPoint(mriSlice.position).x);
            } else
            {
                Debug.Log("Sending message to move clipping plane: " + moveClippingPlane.transform.InverseTransformPoint(mriSlice.position).x);
                moveClippingPlane.changePlaneYPosition(MRICollection.InverseTransformPoint(mriSlice.position).y);
            }
            MRICollection.GetComponent<MRIEvents>().publishMRIHighlightEvent(mriSlice.name);
        };
    }


    /*
    void OnSelect()
    {
        //MRIManager.Instance.ReturnFromDisplaySingleMRI();
        //MRIIconManager.Instance.DeselectAll();
        //gameObject.GetComponent<ButtonSwapFeedback>().ToggleButtonImage();
        //slice.SelectMRI();
        foreach(SwapMRIButtonAction current in container.GetComponentsInChildren<SwapMRIButtonAction>())
        {
            current.gameObject.GetComponent<ButtonAppearance>().ResetButton();
        }
        GetComponent<ButtonAppearance>().SetButtonActive();
    }*/
}
