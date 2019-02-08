// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.InputModule;

using HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// GazeManager determines the location of the user's gaze, hit position and normals.
/// DT = destroyable
/// </summary>
public class DTGazeManager : MonoBehaviour
{
    [Tooltip("Maximum gaze distance, in meters, for calculating a hit.")]
    public float MaxGazeDistance = 15.0f;

    [Tooltip("Select the layers raycast should target.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

    /// <summary>
    /// Physics.Raycast result is true if it hits a Hologram.
    /// </summary>
    public bool Hit { get; private set; }

    /// <summary>
    /// HitInfo property gives access
    /// to RaycastHit public members.
    /// </summary>
    public RaycastHit HitInfo { get; private set; }

    /// <summary>
    /// Position of the intersection of the user's gaze and the hologram's in the scene.
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <summary>
    /// RaycastHit Normal direction.
    /// </summary>
    public Vector3 Normal { get; private set; }

    public Vector3 gazeOrigin { get; private set; }
    public Vector3 gazeDirection { get; private set; }

    private float lastHitDistance = 15.0f;

    public BaseRayStabilizer Stabilizer = null;

    public delegate void FocusedChangedDelegate(GameObject previousObject, GameObject newObject);

    /// <summary>
    /// Dispatched when focus shifts to a new object, or focus on current object
    /// is lost.
    /// </summary>
    public event FocusedChangedDelegate FocusedObjectChanged;

    private GameObject previousHitObject;

    private void Update()
    {
        UpdateGazeInfo();

        UpdateRaycast();
    }

    private void UpdateGazeInfo()
    {
        Vector3 newGazeOrigin = Camera.main.transform.position;
        Vector3 newGazeNormal = Camera.main.transform.forward;

        // Update gaze info from stabilizer
        if (Stabilizer != null)
        {
            Stabilizer.UpdateStability(newGazeOrigin, Camera.main.transform.rotation);
            newGazeOrigin = Stabilizer.StablePosition;
            newGazeNormal = Stabilizer.StableRay.direction;
        }

        gazeOrigin = newGazeOrigin;
        gazeDirection = newGazeNormal;
    }

    /// <summary>
    /// Calculates the Raycast hit position and normal.
    /// </summary>
    private void UpdateRaycast()
    {
        // Get the raycast hit information from Unity's physics system.
        RaycastHit hitInfo;
        Hit = Physics.Raycast(gazeOrigin,
                        gazeDirection,
                        out hitInfo,
                        MaxGazeDistance,
                        RaycastLayerMask);

        GameObject newHitObject = null;
        if (hitInfo.collider != null)
        {
            newHitObject = hitInfo.collider.gameObject;
        }

        if (previousHitObject != newHitObject && FocusedObjectChanged != null)
        {
            FocusedObjectChanged(previousHitObject, newHitObject);
        }

        // Update the HitInfo property so other classes can use this hit information.
        HitInfo = hitInfo;
        previousHitObject = newHitObject;

        if (Hit)
        {
            // If the raycast hits a hologram, set the position and normal to match the intersection point.
            Position = hitInfo.point;
            Normal = hitInfo.normal;
            lastHitDistance = hitInfo.distance;
        }
        else
        {
            // If the raycast does not hit a hologram, default the position to last hit distance in front of the user,
            // and the normal to face the user.
            Position = gazeOrigin + (gazeDirection * lastHitDistance);
            Normal = gazeDirection;
        }
    }
}