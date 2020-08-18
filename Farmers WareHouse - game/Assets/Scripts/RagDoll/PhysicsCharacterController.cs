using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Struct that organizes the target with the GO and the transform
/// </summary>
[System.Serializable]
public class Part
{
    // public information
    public GameObject partGO;               // reff to the base GO
    public GameObject partTarget;           // reff to the target GO
    [HideInInspector] public ConfigurableJoint partJoint;     // reff to the target GO
    public bool inverting;                  // bool telling if mirroring the target 
}

public class PhysicsCharacterController : MonoBehaviour
{
    #region Private Vars
    // List of parts
    [SerializeField] private List<Part> bodyParts = new List<Part>();
    #endregion

    // On Start
    private void Start()
    {
        // Init all the partes in list
        InitParts();
    }
  
    // Fixed Update
    private void FixedUpdate()
    {
        // update bodyparts target rotation
        foreach (var part in bodyParts)
        {
            // aply the diff to the start joint rotation
            if (part.inverting)
                part.partJoint.targetRotation = Quaternion.Inverse(part.partTarget.transform.localRotation);
            else
                part.partJoint.targetRotation = part.partTarget.transform.localRotation;
        }
    }

    /// <summary>
    /// Collect data from the main objects, get the joint and check if have a valid target
    /// </summary>
    private void InitParts()
    {
        // evaluate all the elements in list
        foreach (var bPart in bodyParts)
        {
            // evaluate if the main go exist
            if (bPart.partGO)
            {
                // checks if has a configurable joint
                if (bPart.partGO.GetComponent<ConfigurableJoint>())
                {
                    // stores the ref for the joint
                    bPart.partJoint = bPart.partGO.GetComponent<ConfigurableJoint>();
                    // evaluate the target GO
                    if (!bPart.partTarget)
                    {
                        // if not setted properly, debug and close
                        Debug.LogError($"GO -> {bPart.partGO.name} doesnt have a valid target!");
                        EditorApplication.ExitPlaymode();
                    }
                }
                else
                {
                    //if fails and its in editor, stop and debug
                    Debug.LogError($"GO -> {bPart.partGO.name} doesnt have a Configurable joint!");
                    EditorApplication.ExitPlaymode();
                }
            }
        }
    }
}
