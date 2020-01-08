using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelPositionSetter : MonoBehaviour
{
    // The way this works
    // Create 3 reference gameObjects for UI Panel locations:
    // 1. UIPanelRestingTransformReference ->                       where the UI Panel rest / where it starts its upward rotation
    //                                                              rotation from horizon is set by "RestingRotationFromCamera", and distance from camera is set by "RestingDistFromCamera"
    // 2. UIPanelUsingTransformReference_parentedToCamera ->        use for UIPanelUsingTransformReference_alwaysOnHorizontalPlane to get x and z position and rotation
    // 3. UIPanelUsingTransformReference_alwaysOnHorizontalPlane -> where the UI Panel should be when user call it / where it ends its upward rotation
    //                                                              distance from camera is set by "RestingDistFromCamera"
    // When user tilts head down below threshold "RotationDownwardsToTriggerUI", UI Panel resets position to UIPanelRestingTransformReference,
    // and starts rotating up to UIPanelUsingTransformReference_alwaysOnHorizontalPlane, taking up amount of time "TimeToRotateUp"
    // UI Panel stays there until user tilts head down again (automatic pinning + no weird floaty-ness)

    // settings
    float RestingDistFromCamera = 1.5f;
    float RestingRotationFromCamera = -35f;
    float RotationDownwardsToTriggerUI = 20f;
    float TimeToRotateUp = 2f;
    float OffsetFromCameraHorizon = -.3f;

    // global variable to save state
    private GameObject UIPanelUsingTransformReference_alwaysOnHorizontalPlane;
    private GameObject UIPanelUsingTransformReference_parentedToCamera;
    private GameObject UIPanelRestingTransformReference;
    private bool StartRotatingPanelUp = false;
    private bool PanelRotatedUp = false;
    private float CurrentTimeStepWhenRotatingUp = 0f;
    private bool CameraIsDown = false;
    private bool CameraWasJustUpAfterThePreviousRotation;

    // Use this for initialization
    void Start()
    {
        // initialize UI panel reference objects
        UIPanelUsingTransformReference_parentedToCamera = new GameObject();
        UIPanelUsingTransformReference_alwaysOnHorizontalPlane = new GameObject();
        UIPanelRestingTransformReference = new GameObject();
        SetUsingTransform(UIPanelUsingTransformReference_alwaysOnHorizontalPlane);
        SetUsingTransform(UIPanelUsingTransformReference_parentedToCamera);
        SetRestingTransform(UIPanelRestingTransformReference);
        UIPanelUsingTransformReference_parentedToCamera.transform.SetParent(Camera.main.transform);
        UIPanelRestingTransformReference.transform.SetParent(Camera.main.transform);

        // initialize this UI Panel position and rotation
        SetRestingTransform(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // check if user has tilted head down or not, panel has started rotating up already or not, user has tilted head back up after this UI Panel started rotating or not
        // if all is true, then starting rotating this UI panel up and reset rotation clock
        if (CheckIfCameraRotateDown() && !StartRotatingPanelUp & CameraWasJustUpAfterThePreviousRotation)
        {
            StartRotatingPanelUp = true;
            CurrentTimeStepWhenRotatingUp = 0f;
        }

        // check if user has tiled head up or not
        // if tilted up, then camera must be up since the last time this UI Panel has rotated (= true)
        // (otherwise this bool would be turned false right after this UI Panel is done rotating up, and cannot be turned true until user tilted head back up again)
        if (CheckIfCameraRotateUp())
        {
            CameraWasJustUpAfterThePreviousRotation = true;
        }

        // check against the bools set in this update function and does (or does not) do the UI Panel rotation
        MoveUIPanelUp();
    }

    // helper to check if user has tilted head up
    private bool CheckIfCameraRotateUp()
    {
        return Camera.main.transform.rotation.eulerAngles.x < RotationDownwardsToTriggerUI;
    }

    // helper to check if user has tilted head down
    private bool CheckIfCameraRotateDown()
    {
        return Camera.main.transform.rotation.eulerAngles.x < 180 && Camera.main.transform.rotation.eulerAngles.x > RotationDownwardsToTriggerUI;
    }

    private void MoveUIPanelUp()
    {
        if (StartRotatingPanelUp)
        {
            UpdateUIPanelUsingTransformReference();
            if (CurrentTimeStepWhenRotatingUp <= TimeToRotateUp)
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

    // helper to update location of UIPanelUsingTransformReference_alwaysOnHorizontalPlane to make it always stay on horizontal plane by referencing y coords from camera
    private void UpdateUIPanelUsingTransformReference()
    {
        UIPanelUsingTransformReference_alwaysOnHorizontalPlane.transform.position = new Vector3(UIPanelUsingTransformReference_parentedToCamera.transform.position.x,
                                                                                                Camera.main.transform.position.y + OffsetFromCameraHorizon,
                                                                                                UIPanelUsingTransformReference_parentedToCamera.transform.position.z);
        UIPanelUsingTransformReference_alwaysOnHorizontalPlane.transform.eulerAngles = new Vector3(UIPanelUsingTransformReference_parentedToCamera.transform.rotation.eulerAngles.x,
                                                                                                            Camera.main.transform.rotation.eulerAngles.y,
                                                                                                            UIPanelUsingTransformReference_parentedToCamera.transform.rotation.eulerAngles.z);
    }

    // helper to set initial resting (inactive) transform for UI Panel
    void SetRestingTransform(GameObject toSet)
    {
        toSet.transform.position = new Vector3(Camera.main.transform.position.x,
                                                            Camera.main.transform.position.y,
                                                            Camera.main.transform.position.z + RestingDistFromCamera);
        toSet.transform.rotation = this.transform.rotation;
        toSet.transform.RotateAround(Camera.main.transform.position, Vector3.left, RestingRotationFromCamera);
    }

    // helper to set initial using (active) transform for UI Panel
    void SetUsingTransform(GameObject toSet)
    {
        toSet.transform.position = new Vector3(Camera.main.transform.position.x,
                                                            Camera.main.transform.position.y,
                                                            Camera.main.transform.position.z + RestingDistFromCamera);
        toSet.transform.rotation = this.transform.rotation;
    }

    // helper to make UI Panel rotate up
    void LerpTransform(Transform toSet, float t)
    {
        toSet.position = Vector3.Lerp(UIPanelRestingTransformReference.transform.position, UIPanelUsingTransformReference_alwaysOnHorizontalPlane.transform.position, t);
        toSet.rotation = Quaternion.Lerp(UIPanelRestingTransformReference.transform.rotation, UIPanelUsingTransformReference_alwaysOnHorizontalPlane.transform.rotation, t);
    }
}
