using UnityEditor;
using UnityEngine;
using System.Collections;

public class PlayerCore : MonoBehaviour
{

    // Reference to the Physics character Controller
    private PhysicsCharacterController physicsCharacter;
    private Animator playerTargetAnimator;
    private GrabController playerGrabController;

    // Internel vars
    // ref to input controller
    private CameraController cameraController;

    // input value
    private Vector2 inputAmount = Vector2.zero;
    // Is the player stunted
    private bool stunted;

    // Exposed Vars
    [Header("Character Configuration")]
    [Header("Player State")]
    [Tooltip("Character Stunted time")] public float stuntedTime = 2f;
    [Tooltip("Character kill velocity when falling"), Range(10f, 50f)] public float killVelocity = 25f;
    [Header("Character physic values")]
    [Tooltip("Character base acceleration")] public float characterAcceleration = 200f;
    [Tooltip("Character base angular acceleration")] public float characterAngularAcceleration = 5f;
    [Tooltip("Character max velocity")] public float characterMaxVelocity = 0f;
    [Tooltip("Active drag"), Range(0f, 1f)] public float activeDrag = 0.25f;
    [Tooltip("Jump force based on mass"), Range(2f, 3f)] public float JumpValue = .8f;

    [Header("Animation Controls")]
    [Tooltip("Walking animation based on current speed"), Range(0f, 1f)] public float animationSpeedInfluence = 0.2f;

    [Header("Grab System")]
    [Tooltip("Left Hand Joint")] public FixedJoint LeftHandJoint;
    [Tooltip("Right Hand Joint")] public FixedJoint RighttHandJoint;

    // Start is called before the first frame update
    void Start() => InitController();   // Initialize this controller

    // Update is called once per frame
    void Update()
    {
        // Update Animator State
        UpdateAnimator();
    }
    private void FixedUpdate()
    {
        // if is stuned, ignore input response
        if (stunted || !cameraController) return;

        // Physics input for player
        InputPhysicsResponse();
    }

    #region Internal Methods

    /// <summary>
    /// initial setup of this controller
    /// </summary>
    private void InitController()
    {
        // Get the reff for the physics controller
        physicsCharacter = GetComponentInChildren<PhysicsCharacterController>();
        // get the ref of the target animator
        playerTargetAnimator = GetComponentInChildren<Animator>();

        // Validate the reff
        // if fails to find the physics controller
        if (!physicsCharacter || !playerTargetAnimator)
        { EditorApplication.ExitPlaymode(); return; }

        // Debug information and initialize  the parts
        InformationPanel.DebugConsoleInput("Physics System connected!");
        physicsCharacter.InitParts();

        // Define the maximum velocity
        physicsCharacter.SetCharacterMaxSpeed(characterMaxVelocity);
        // define the kill speed
        physicsCharacter.SetCharacterKillVelocity(this.killVelocity);

        // Stunted  -   -   -   -   -
        // regist the for the stunted event
        physicsCharacter.stuntedDelegate += StuntedCallback;
        // init vars
        stunted = false;

        // Regists for jump delegate
        InputManager.Instance.jumpDelegate += () => { physicsCharacter.Jump(this.JumpValue); };

        // Grab controller  -   -   -   -   -
        // get ref to controller
        this.playerGrabController = this.GetComponentInChildren<GrabController>();
        // Init with enable tag
        this.playerGrabController.Init(controllerState: true, lHand: ref LeftHandJoint, rHand: ref RighttHandJoint);
        // regist the controller to the action event
        InputManager.Instance.actionDelegate += playerGrabController.ActionCallback;
    }

    /// <summary>
    /// Method call on fixed for handling input to player movement
    /// </summary>
    private void InputPhysicsResponse()
    {
        // check if exist a input manager
        if (InputManager.Instance == null) return;

        // Get the input vector
        inputAmount = InputManager.Instance.GetInputVector();

        // if exists motin to be apllied to the rb
        if (inputAmount != Vector2.zero)
            // move the p+layer
            physicsCharacter.Move(
                ((cameraController.GetCameraToScreenForward * inputAmount.y) +
                (cameraController.GetCameraToScreenRight * inputAmount.x)) *
                characterAcceleration, characterAngularAcceleration);
        else
            // if not, try to stop the player
            physicsCharacter.DragInput(activeDrag);

    }

    /// <summary>
    /// Update the animator state
    /// </summary>
    private void UpdateAnimator()
    {
        // update the moving bool
        playerTargetAnimator.SetBool("IsMoving", physicsCharacter.GetCurrentVelocity > 0.1f);
        playerTargetAnimator.SetFloat("WalkingSpeed", physicsCharacter.GetCurrentVelocity * animationSpeedInfluence);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Set the camera controller
    /// </summary>
    public CameraController CameraControlls
    {
        get => cameraController;
        // Custom set
        set { cameraController = value; }
    }

    ///<Summary>
    /// Set the player State
    ///</Summary>
    public bool StuntedState
    {
        // return the current state
        get => this.stunted;
        // if the new state is different from the current physic state
        set
        {
            if (value != physicsCharacter.IsStunted)
            {
                // change the current state
                physicsCharacter.Switchstanding();
                // state the new state
                this.stunted = value;
            }
        }
    }

    /// <Summary>
    /// Callback called when the physics controller set the state to stunted
    /// </Summary>
    public void StuntedCallback()
    {
        // if the player is current not stunted
        if (!stunted) { StuntedState = true; StartCoroutine(StuntedCouroutine()); }

        // Debug
        InformationPanel.DebugConsoleInput("Player is stunted");

    }
    #endregion

    #region Coroutines
    // couroutine for stunted timer
    private IEnumerator StuntedCouroutine()
    {
        // wait for the player to completly stop
        while (physicsCharacter.GetCurrentVelocity >= 1f) { yield return null; }
        // wait stunted time for wake up
        yield return new WaitForSecondsRealtime(stuntedTime);

        // after wait, change the state
        StuntedState = false;
        InformationPanel.DebugConsoleInput("Player is no longer stunted");
    }

    #endregion
}
