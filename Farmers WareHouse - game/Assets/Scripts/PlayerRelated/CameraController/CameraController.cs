using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // public vars
    [Header("Camera Settings")]
    public GameObject cameraTarget;
    [Tooltip("Angular to the camera")] public Vector3 angularValues;
    [Tooltip("Distance to target")] public float distanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        // aplly the values and set camera to target position
        if (cameraTarget) { SetTargetPosition(); }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the camera position
        UpdateCameraPosition();
    }

    //  -   -   -   -   -   PRIVATE -   -   -   -   -
    #region Private Methods
    /// <summary>
    /// Defines the target position of this camera
    /// </summary>
    private void SetTargetPosition()
    {
        // Evaluate if is a controllable object
        if (cameraTarget.GetComponentInParent<PlayerCore>())
            // if is a player
            cameraTarget.GetComponentInParent<PlayerCore>().CameraControlls = this;

        // set the camera to the position based on settings
        this.transform.rotation = Quaternion.Euler(angularValues);
       
    }
    /// <summary>
    /// Updates the camera position
    /// </summary>
    private void UpdateCameraPosition()
    {
        // Set the camera position
        this.transform.position = cameraTarget.transform.position +
            this.transform.forward * distanceToTarget * -1f;

        // set the lock at
        this.transform.LookAt(cameraTarget.transform.position);
    }

    #endregion

    // -    -   -   -   -   PUBLIC  -   -   -   -   -
    #region Public Methods
    /// <summary>
    /// Get the forward vector on world 
    /// </summary>
    /// <return>Vector forward</return>
    public Vector3 GetCameraToScreenForward { get { return new Vector3(this.transform.forward.x, 0f, this.transform.forward.z).normalized; } }
    public Vector3 GetCameraToScreenRight {  get { return Quaternion.AngleAxis(-90f, Vector3.up) * GetCameraToScreenForward; } }
    #endregion

    //  -   -   -   -   -   DEBUG   -   -   -   -   -
    private void OnDrawGizmos()
    {
        // Forward
        //Set Color
        Gizmos.color = Color.yellow;
        // Draw the camera to world forward
        Gizmos.DrawLine(cameraTarget.transform.position,
            cameraTarget.transform.position + GetCameraToScreenForward);

        // Right
        // Set Color
        Gizmos.color = Color.green;
        // Draw the camera to world forward
        Gizmos.DrawLine(cameraTarget.transform.position,
            cameraTarget.transform.position + GetCameraToScreenRight);


    }
}
