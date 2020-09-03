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

/// <summary>
/// Struct that returns valid information for collision check
/// </summary>
public struct RayOutInfo
{
    public bool collided;           // define if the collision is valid
    public Vector3 collisionPoint;  // world position of the collision
    public Vector3 collisionNormal; // normal for the collision point
}

public class PhysicsCharacterController : MonoBehaviour
{
    #region Vars
    //Exposed Vars  -   -   -   -   -
    // List of parts
    [SerializeField] private List<Part> bodyParts = new List<Part>();
    public float BaseMassScale = 30f;  // Base mass Scale
    public Vector3 CharacterGravity = Vector3.down; // Custom gameobject gravity
    public LayerMask collisionMask;     // mask for collision

    // referenc to the "StandUp" joint
    private ConfigurableJoint StandJoint;
    private Rigidbody mainRB;

    // Bool states
    public bool IsStandingUp { get; private set; }
    // Behaviour
    private float maxVelocity = 0f;

    //Ground collision information
    public float groundTestsize = 0.3f;
    private RayOutInfo tempRayInfo = new RayOutInfo { collided = false };
    private CapsuleCollider targetSphereCollider;   // reff to the target controller sphere coll
    #endregion

    // Fixed Update
    private void FixedUpdate()
    {
        // Update all the parts to rotation
        this.UpdatePartsToTarget();
        // add the gravity force 
    }


    #region Pulbic Methods
    /// <summary>
    /// Collect data from the main objects, get the joint and check if have a valid target
    /// </summary>
    public void InitParts()
    {
        // Setting to the stangin pos
        IsStandingUp = true;

        // stores the reference for the standoUp Joint
        StandJoint = GetComponent<ConfigurableJoint>();
        StandJoint.connectedMassScale = BaseMassScale;
        // ref to the main rigidbody
        mainRB = GetComponent<Rigidbody>();
        // change the current gravity
        mainRB.useGravity = false;
        // store a reference to the capsule colide
        this.targetSphereCollider = this.GetComponent<CapsuleCollider>();

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
        if (IsStandingUp) { IsStandingUp = false; StandJoint.connectedMassScale = 0f; }
        else { IsStandingUp = true; StandJoint.connectedMassScale = BaseMassScale; }
    }

    /// <summary>
    /// Define the max velocity for the caracter
    /// </summary>
    /// <param name="value">Velocity</param>
    public void SetCharacterMaxSpeed(float value) { maxVelocity = value; }

    /// <summary>
    /// Apply motion to this rigid body
    /// </summary>
    /// <param name="direction">Target Motion</param>
    public void Move(Vector3 direction, float angularVel)
    {
        // temp collision information
        tempRayInfo = GroundCheck();

        //  check if the main rigid body exists
        if (!mainRB || !tempRayInfo.collided) return;

        // if so aply the motion
        // Draw the debug line
        Debug.DrawLine(this.transform.position, this.transform.position + direction);

        // add fore to the rb
        mainRB.AddForce(MovementForceCalculation(direction, tempRayInfo), ForceMode.Impulse);

        // keep the maximum velocity inside range
        if (GetCurrentVelocity > maxVelocity)
            mainRB.velocity = Vector3.ClampMagnitude(mainRB.velocity, maxVelocity);

        // call the orient player method
        OrientPhyscsCharacter(direction, angularVel);
    }

    /// <summary>
    /// Active drag
    /// </summary>
    /// <param name="value">Drag amount</param>
    public void DragInput(float value)
    {
        // check if righid body exists
        if (!mainRB) return;
        // negative velocitu
        mainRB.AddForce(-new Vector3(mainRB.velocity.x, 0f, mainRB.velocity.z) * value, ForceMode.Impulse);
    }
    #endregion



    #region Getters
    /// <summary>
    /// Get The physics forward
    /// </summary>
    /// <return>Return the vector forward</return>
    public Vector3 GetPhysicsForward { get { return this.transform.forward; } }
    /// <summary>
    /// Get the current gameVelocity
    /// </summary>
    public float GetCurrentVelocity { get { return mainRB.velocity.magnitude; } }
    #endregion



    #region Private Methods
    /// <summary>
    /// Orient the player based on velocity direction
    /// </summary>
    /// <param name="value">Angular velocity</param>
    private void OrientPhyscsCharacter(Vector3 value, float angularVel)
    {
        // if velocity is relevant
        if (GetCurrentVelocity != 0f)
            // rotate the player towards the input direction
            this.transform.rotation =
                Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(value, Vector3.up), angularVel);
    }

    // Update all the dynamic rag rotation to target
    void UpdatePartsToTarget()
    {
        // correct gravity
        this.tempRayInfo = GroundCheck();   // save the current groundCheck state
        if (!this.tempRayInfo.collided)    // if not on the ground, add gravity
            this.mainRB.AddForce(CharacterGravity, ForceMode.Impulse); // add gravity        


        // update bodyparts target rotation
        foreach (var part in bodyParts)
            // aply the diff to the start joint rotation
            if (part.inverting)
                part.partJoint.targetRotation = Quaternion.Inverse(part.partTarget.transform.localRotation);
            else
                part.partJoint.targetRotation = part.partTarget.transform.localRotation;

    }

    // MOVEMENT -   -   -   -   -
    /// <summary>
    /// Evaluate the correct force to be added
    /// </summary>
    private Vector3 MovementForceCalculation(Vector3 moveDirection, RayOutInfo colInfo)
    {
        // calculate the correct movement force based on the ground slope

        //SLOPE  -   -   -   -   -
        // calculate the direction of motion based on slop normal
        // Calculate the move direction based on the surface normal
        Vector3 calculateForce = Quaternion.FromToRotation(this.transform.up, colInfo.collisionNormal) *
            ((1f - Vector3.Dot(colInfo.collisionNormal.normalized, -this.transform.right)) * moveDirection);

        // Debug force
        Debug.DrawLine(this.transform.position, this.transform.position + calculateForce, Color.cyan);

        // DRIFT PREVENTING -   -   -   -   -
        // add counter force for preventing drift
        //calculateForce += Vector3.Reflect(-mainRB.velocity, calculateForce).normalized *
        //    Vector3.Dot(mainRB.velocity.normalized, calculateForce);

        // Return the calculate vector
        return calculateForce;
    }

    /// <summary>
    /// check if the player is grounded
    /// </summary>
    private RayOutInfo GroundCheck()
    {
        // evaluate if the player is grounded
        Ray tempRay = new Ray(this.targetSphereCollider.transform.position + this.targetSphereCollider.center, Vector3.down);

        // check for collisions for the ground
        if (Physics.SphereCast(tempRay, this.groundTestsize, out RaycastHit tempHitInfo, 3f * this.groundTestsize, collisionMask))
            return new RayOutInfo()
            { collided = true, collisionNormal = tempHitInfo.normal, collisionPoint = tempHitInfo.point };    // if collided with ground
        // if not
        return new RayOutInfo() { collided = false };
    }


    // GIZMOS   -   -   -   -   -
    // Debug information
    private void OnDrawGizmos()
    {
        // Draw the movement direction
        DrawCurrenteMoveTarget();
    }

    /// <summary>
    /// Draw the current movement direction
    /// </summary>
    private void DrawCurrenteMoveTarget()
    {
        // Draw Forward
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2f);

        // ground area
        Gizmos.color = tempRayInfo.collided ? Color.green : Color.red;  // color based on groundedState
        if (this.targetSphereCollider) Gizmos.DrawWireSphere(this.targetSphereCollider.transform.position + this.targetSphereCollider.center, this.groundTestsize);

        // draw grounded info
        if (tempRayInfo.collided)
        {
            // Set color
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, this.transform.position + tempRayInfo.collisionNormal);
        }
    }

    #endregion
}
