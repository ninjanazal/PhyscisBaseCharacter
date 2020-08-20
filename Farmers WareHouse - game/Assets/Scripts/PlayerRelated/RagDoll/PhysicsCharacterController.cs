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
    public float BaseMassScale = 30f;  // Base mass Scale

    // referenc to the "StandUp" joint
    private ConfigurableJoint StandJoint;
    public bool isStandingUp { get; private set; }

    #endregion

    // On Start
    private void Start()
    {
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
    public void InitParts()
    {
        // Setting to the stangin pos
        isStandingUp = true;

        // stores the reference for the standoUp Joint
        StandJoint = GetComponent<ConfigurableJoint>();
        StandJoint.connectedMassScale = BaseMassScale;

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

    /// <summary>
    /// switch the standing state
    /// </summary>
    public void Switchstanding()
    {
        // invert the state
        if (isStandingUp) { isStandingUp = false; StandJoint.connectedMassScale = 0f; }
        else { isStandingUp = true; StandJoint.connectedMassScale = BaseMassScale; }
    }
}
