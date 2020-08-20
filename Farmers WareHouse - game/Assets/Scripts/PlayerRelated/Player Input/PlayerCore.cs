using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCore : MonoBehaviour
{
    [Header("Input Asset")]
    public float baseVelocity;

    // Reference to the Physics character Controller
    private PhysicsCharacterController physicsCharacter;
    private Animator playerTargetAnimator;

    // Internel vars
    // ref to input controller
    private InputActions inputController;
    public bool stunted { get; private set; }
    // input value
    private Vector2 inputAmount = Vector2.zero;

    private void Awake()
    {
        inputController = new InputActions();

        // enable the event for player movement input
        inputController.PlayerLocomotion.Enable();

        // regist input
        inputController.PlayerLocomotion.Movement.performed +=
            ctx => inputAmount = ctx.ReadValue<Vector2>(); ;
        // 
        inputController.PlayerLocomotion.Movement.canceled +=
            _ => inputAmount = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitController();   // Initialize this controller
    }

    // Update is called once per frame
    void Update()
    {


    }


    private void FixedUpdate()
    {
        // if is stuned, ignore input response
        if (stunted) return;

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

        // init vars
        stunted = false;
    }

    /// <summary>
    /// Method call on fixed for handling input to player movement
    /// </summary>
    private void InputPhysicsResponse()
    {
        Debug.Log($"Changed to {inputAmount}");
    }
    #endregion
}
