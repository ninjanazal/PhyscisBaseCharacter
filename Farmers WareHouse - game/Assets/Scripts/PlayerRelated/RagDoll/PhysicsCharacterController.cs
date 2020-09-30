using UnityEngine;
using System;
using System.Collections.Generic;

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
    //Exposed Vars  -   -   -   -   -
    // List of parts
    [SerializeField] private List<Part> bodyParts = new List<Part>();
    [SerializeField] private float CharacterMass = 10f; // base mass influence on main mass
    public float BaseMassScale = 30f;  // Base mass Scale
    public Vector3 CharacterGravity = Vector3.down; // Custom gameobject gravity
    public LayerMask collisionMask;     // mask for collision
    public float driftPrevention = 0f;  // preventing character from drifting
    public float groundTestsize = 0.3f; // Ground detection distance
    public float stepSearchDistance = 0.2f;    // Distance to check for step
    public float maxStepSize = 0.5f;     // max step size


    // referenc to the "StandUp" joint
    private ConfigurableJoint StandJoint;
    private Rigidbody mainRB;       // physics controller RigidBody
    private float previousVelocity = 0f; // last frame velocity

    // Bool states
    public bool IsStunted { get; private set; }

    // Behaviour
    private float maxVelocity = 0f;
    private float killVelocity = 0f;

    //Ground collision information
    private RayOutInfo tempRayInfo = new RayOutInfo { collided = false };
    private CapsuleCollider targetSphereCollider;   // reff to the target controller sphere coll

    // Stunted delegate and event
    public delegate void OnStuntedDelegate();
    public event OnStuntedDelegate stuntedDelegate;

    // Fixed Update
    private void FixedUpdate()
    {
        // Update controller physics
        this.UpdateCharacterPhysics();
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
        IsStunted = false;
        // stores the reference for the standoUp Joint
        StandJoint = GetComponent<ConfigurableJoint>();
        StandJoint.connectedMassScale = BaseMassScale;

        // ref to the main rigidbody
        mainRB = GetComponent<Rigidbody>();
        // change the current gravity
        mainRB.useGravity = false;
        // set the character mass
        mainRB.mass = CharacterMass;

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
                        // close aplication
                        AplicationFuncs.CloseApp();
                    }
                }
                else
                {
                    //if fails and its in editor, stop and debug
                    Debug.LogError($"GO -> {bPart.partGO.name} doesnt have a Configurable joint!");
                    // close aplication
                    AplicationFuncs.CloseApp();
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
        if (!IsStunted) { IsStunted = true; StandJoint.connectedMassScale = 0f; }
        else { IsStunted = false; StandJoint.connectedMassScale = BaseMassScale; }
    }

    /// <summary>
    /// Define the max velocity for the caracter
    /// </summary>
    /// <param name="value">Velocity</param>
    public void SetCharacterMaxSpeed(float value) { maxVelocity = value; }

    /// <Summary>
    /// set the kill fall velocity on the physics controller
    /// </Summary>
    /// <param name="value"> Kill velocity value </param>
    public void SetCharacterKillVelocity(float value) { killVelocity = (float)Math.Pow(value, 2.0); }

    /// <summary>
    /// Apply motion to this rigid body
    /// </summary>
    /// <param name="direction">Target Motion</param>
    public void Move(Vector3 direction, float angularVel)
    {
        //  check if the main rigid body exists
        if (!mainRB || !tempRayInfo.collided) return;

        // if so aply the motion
        // Draw the debug line
        Debug.DrawLine(this.transform.position, this.transform.position + direction);

        // STAIR DETECTION  -   -   -   -   -
        // calculate de height for the step
        //mainRB.MovePosition(mainRB.position + (Vector3.up * StepDetection()));

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
        if (!mainRB || !tempRayInfo.collided) return;
        // negative velocitu
        mainRB.AddForce(-new Vector3(mainRB.velocity.x, 0f, mainRB.velocity.z) * (mainRB.mass * value), ForceMode.Impulse);
    }

    /// <summary>
    /// Function called for jumping
    /// </summary>
    /// <param name="force">Jumping force</param>
    public void Jump(float force)
    {
        // check if the player is grounded or is stunned
        if (!tempRayInfo.collided || IsStunted) return;

        // if grounded add force based on the force
        mainRB.AddForce(Vector3.up * (float)Math.Pow(mainRB.mass, force), ForceMode.Impulse);
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

    /// <summary>
    /// Get the state for the current physics state
    /// </summary>
    /// <return>Return true if on ground at this physical update</return>
    public bool isGroundedNow { get { return this.tempRayInfo.collided; } }
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

    /// <Summary>
    /// Calculates the physics states
    /// </Summary>
    private void UpdateCharacterPhysics()
    {
        // correct gravity
        this.tempRayInfo = GroundCheck();   // save the current groundCheck state

        if (!this.tempRayInfo.collided)    // if not on the ground, add gravity
            this.mainRB.AddForce(CharacterGravity, ForceMode.Impulse); // add gravity     
        else CheckDeathState();

        // Stores the currenct velocity
        this.previousVelocity = this.mainRB.velocity.sqrMagnitude;
    }

    // Update all the dynamic rag rotation to target
    private void UpdatePartsToTarget()
    {
        if (!IsStunted)
            // update bodyparts target rotation
            foreach (var part in bodyParts)
                // aply the diff to the start joint rotation
                part.partJoint.targetRotation = (part.inverting) ?
                        Quaternion.Inverse(part.partTarget.transform.localRotation) :
                        part.partTarget.transform.localRotation;
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
        // the velocity is manipulated by the inclination
        Vector3 calculateForce = Quaternion.FromToRotation(this.transform.up, colInfo.collisionNormal) * (moveDirection *
            Vector3.Dot(colInfo.collisionNormal, Vector3.up));


        // Drift compensation
        calculateForce *= Mathf.Clamp((1f - Math.Abs(Vector3.Dot(calculateForce.normalized, mainRB.velocity.normalized)))
         * mainRB.mass * this.driftPrevention
            , 1f, mainRB.mass);

        // Debug force
        Debug.DrawLine(this.transform.position, this.transform.position + calculateForce, Color.cyan);
        // Return the calculate vector
        return calculateForce;
    }

    // STATES
    /// <Summary>
    /// Check if the character should be stunned
    /// </Summary>
    private void CheckDeathState()
    {
        // KILL STATE CHECK
        // check if the player fall killed the player  
        if (this.mainRB.velocity.sqrMagnitude >= killVelocity || this.previousVelocity >= killVelocity)
            // the player should be stunted
            stuntedDelegate();

    }

    /// <summary>
    /// check if the player is grounded
    /// </summary>
    private RayOutInfo GroundCheck()
    {
        // evaluate if the player is grounded
        Ray tempRay = new Ray(this.transform.TransformPoint(this.targetSphereCollider.center), Vector3.down);

        // check for collisions for the ground
        if (Physics.SphereCast(tempRay, this.targetSphereCollider.radius * 0.85f, out RaycastHit tempHitInfo, 3f * this.groundTestsize, collisionMask))
            // if collided with ground
            return new RayOutInfo()
            {
                collided = true,
                collisionNormal = tempHitInfo.normal,
                collisionPoint = tempHitInfo.point
            };

        // if not
        return new RayOutInfo() { collided = false };
    }

    /// <summary>
    /// Detect if there is any step
    /// </summary>
    /// <returns>
    /// Return the detected step Height, if not return zero
    ///</returns>
    private float StepDetection()
    {
        // temp hit information
        RaycastHit hit;
        Vector3 tempPostion = this.targetSphereCollider.center;
        var distance = 0f;

        // Check if there is any obstacle in front of feet
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, stepSearchDistance, collisionMask))
        {
            // if is ground in front of player
            // check if the step is bigger than max size
            // generate the position for the vertical test
            // position calculated based on detected distance
            tempPostion += this.transform.forward * Vector3.Distance(this.transform.position, hit.point);
            InformationPanel.DebugConsoleInput("In front step distance");
            // Evaluate if the step is in range for steping up
            if (Physics.Raycast(tempPostion, Vector3.down, out hit, this.targetSphereCollider.height * 0.5f) &&
                (distance = Vector3.Distance(hit.point, this.transform.position)) <= maxStepSize)
            {// if the vertical ray hit a objecto based on the distance from the horizontal one
                // and the distance from the ground pos to the vertical hit is less than step size, return the step position
                InformationPanel.DebugConsoleInput("valid step height");
                Debug.DrawLine(this.transform.position, hit.point, Color.yellow);
                return distance;
            }
        }
        // return 0 if not detected
        return 0f;
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
        if (this.targetSphereCollider)
            Gizmos.DrawWireSphere(this.transform.TransformPoint(this.targetSphereCollider.center), this.targetSphereCollider.radius * 0.85f);

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
