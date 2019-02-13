using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelPositionSetter : MonoBehaviour {

    // settings
    float RestingDistFromCamera = 2f;
    float RestingRotationFromCamera = -20f;
    float RotationDownwardsToTriggerUI = 10f;
    float TimeToRotateUp = 2f;

    // global variable to save state
    private GameObject UIPanelUsingTransformRefernece;
    private GameObject UIPanelRestingTransformReference;
    private bool StartRotatingPanelUp = false;
    private bool PanelRotatedUp = false;
    private float CurrentTimeStepWhenRotatingUp = 0f;
    public bool CameraIsDown = false;
    private bool CameraWasJustUpAfterThePreviousRotation;

    // Use this for initialization
    void Start () {
        // initialize UI panel reference objects
        UIPanelUsingTransformRefernece = new GameObject();
        UIPanelRestingTransformReference = new GameObject();
        SetUsingTransform(UIPanelUsingTransformRefernece);
        SetRestingTransform(UIPanelRestingTransformReference);
        UIPanelUsingTransformRefernece.transform.SetParent(Camera.main.transform);
        UIPanelRestingTransformReference.transform.SetParent(Camera.main.transform);

        SetRestingTransform(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (CheckIfCameraRotateDown() && !StartRotatingPanelUp & CameraWasJustUpAfterThePreviousRotation) {
            StartRotatingPanelUp = true;
            CurrentTimeStepWhenRotatingUp = 0f;
        }

        if (CheckIfCameraRotateUp())
        {
            CameraWasJustUpAfterThePreviousRotation = true;
        }
        MoveUIPanelUp();
    }

    private bool CheckIfCameraRotateUp()
    {
        return Camera.main.transform.rotation.eulerAngles.x < RotationDownwardsToTriggerUI;
    }

    private bool CheckIfCameraRotateDown()
    {
        return Camera.main.transform.rotation.eulerAngles.x < 180 && Camera.main.transform.rotation.eulerAngles.x > RotationDownwardsToTriggerUI;
    }

    private void MoveUIPanelUp()
    {
        if (StartRotatingPanelUp)
        {
            if (CurrentTimeStepWhenRotatingUp < TimeToRotateUp)
            {
                CurrentTimeStepWhenRotatingUp += Time.deltaTime;
                LerpTransform(this.transform, CurrentTimeStepWhenRotatingUp / TimeToRotateUp);
            }
            else
            {
                CurrentTimeStepWhenRotatingUp = 0f;
                StartRotatingPanelUp = false;
                CameraWasJustUpAfterThePreviousRotation = false;
            }
        }
    }

    void SetRestingTransform(GameObject toSet)
    {
        toSet.transform.position = new Vector3(Camera.main.transform.position.x,
                                                            Camera.main.transform.position.y,
                                                            Camera.main.transform.position.z + RestingDistFromCamera);
        toSet.transform.rotation = this.transform.rotation;
        toSet.transform.RotateAround(Camera.main.transform.position, Vector3.left, RestingRotationFromCamera);
    }

    void SetUsingTransform(GameObject toSet)
    {
        toSet.transform.position = new Vector3(Camera.main.transform.position.x,
                                                            Camera.main.transform.position.y,
                                                            Camera.main.transform.position.z + RestingDistFromCamera);
        toSet.transform.rotation = this.transform.rotation;
    }

    void LerpTransform(Transform toSet, float t)
    {
        toSet.position = Vector3.Lerp(UIPanelRestingTransformReference.transform.position, UIPanelUsingTransformRefernece.transform.position, t);
        toSet.rotation = Quaternion.Lerp(UIPanelRestingTransformReference.transform.rotation, UIPanelUsingTransformRefernece.transform.rotation, t);
    }
}
