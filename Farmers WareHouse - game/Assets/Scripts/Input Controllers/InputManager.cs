using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    // SINGLETON PATTERN
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }    // Public  static ref

    //DELEGATEs -   -   -   -   -
    // Jump Delegate
    public delegate void OnJumpDelegate();
    public event OnJumpDelegate jumpDelegate;
    // interation funtions
    public delegate void OnActionDelegate();
    public event OnActionDelegate actionDelegate;

    // State
    private bool IsEnable;   // set if is enable 
    private Rect JoyDetectionArea;   // Safe area for the joy action detection

    // jump action click
    // set if the jumo button has been realease after press
    private bool jumpRealeased = false;

    // action button pressed
    // evaluate if the button as been realeaded after pressed
    private bool actionPressed = false;

    // JOYSTICK INFO    -   -   -   -   -
    private bool JoyStickVisible;   // Current joy visible state
    private float JoyMaxRadius; // Max Radius for the joy

    // Touch information
    private int ValidJoyTouchID;    // Current valid touch
    private Vector2 StartPosition;  // Start and end position of the touch
    private Vector2 InputVector;    // Vector from the position to the current pos

    private RawImage joyBackImg, JoyKnobImg;    // Ref to raw img for the back and knob joy
    private RectTransform joyBackRect, joyKnobRect; // Ref to rect transform for the back and knob joy

    // EXPOSED VARS -   -   -   -   -
    [Header("Input Settings")]
    [Tooltip("Percet of screen from left used to joy Detection"), Range(.1f, .5f)] public float ScreenPercentDetect = 0.25f;

    // awake
    private void Awake()
    {
        // Initialize the singleton
        InitSingleton();
    }

    private void FixedUpdate()
    {
        // call registed delegates
        // Jump action
        if (GetJumpValue()) jumpDelegate();
        // Press Action
        if (GetActionValue()) actionDelegate();
    }

    private void Update()
    {
        // if is enable
        if (!IsEnable) return;
        // Joysitch input
        OnScreenController();
        // ACtion Resolver
    }


    #region  Public Methods
    ///<Summary>
    /// Get the input direction
    ///</Summary>
    public Vector2 GetInputVector()
    {        // Return the a vector based on vertical and horizontal input values
        return new Vector2(-InputVector.x, InputVector.y);
    }

    /// <Summary>
    /// Get the jump value
    /// </Summary>
    private bool GetJumpValue()
    {
        // check if the axis as a valid input
        if (Input.GetAxis("Jump") == 1f && jumpRealeased) { jumpRealeased = false; return true; }
        // reset the press locker
        else if (Input.GetAxis("Jump") == 0f) jumpRealeased = true;
        // return false if not
        return false;
    }

    ///<Summary>
    /// Get the action state press
    ///</Summary>
    private bool GetActionValue()
    {
        // check if the axis as a valid input
        if (Input.GetAxis("Action") == 1f && actionPressed) { actionPressed = false; return true; }
        // reset the press locker
        else if (Input.GetAxis("Action") == 0f) actionPressed = true;
        // return false if not
        return false;
    }

    #endregion

    #region Private Methods
    ///<Summary>
    /// Init Singleton instance
    ///</Summary>
    private void InitSingleton()
    {
        // check if the singleton exists or not
        if (instance == null) instance = this;
        // If exists and is different from this, destroy
        else if (InputManager.Instance != this) Destroy(this);

        // Set as enable
        this.IsEnable = true;

        // calculate the area for joyDetection
        JoyDetectionArea =
            new Rect(Screen.safeArea.x, Screen.safeArea.y,
             Screen.safeArea.width * ScreenPercentDetect, Screen.safeArea.height);

        // get the joyStick REffs, knowing that has a child that is a joyBack and a child of that been the knob
        // back part of the joy
        this.joyBackImg = this.GetComponentInChildren<RawImage>();
        this.joyBackRect = this.joyBackImg.GetComponent<RectTransform>();

        // knoob part of the joy
        this.JoyKnobImg = this.joyBackImg.transform.GetChild(0).GetComponent<RawImage>();
        this.joyKnobRect = this.JoyKnobImg.GetComponent<RectTransform>();

        // Calculate the maximus radius for the joy based on size
        this.JoyMaxRadius = this.joyBackRect.rect.width * 0.5f;

        // Hide all the joy images
        IsJoyStickVisible = false;
    }

    /// <summary>
    /// Evaluates, draw and calculte the input on screen
    /// </summary>
    private void OnScreenController()
    {
        // Check if exists toutchs or the joysitck is desable
        if (Input.touchCount == 0f) return;

        // Evaluate all the touches
        foreach (var touch in Input.touches)
        {
            // check if exist a toutch beeing tracked
            if (this.IsJoyStickVisible && touch.fingerId == ValidJoyTouchID)
            {
                // if the touch ended
                if (touch.phase == TouchPhase.Ended)
                {// set the joy invisible
                    IsJoyStickVisible = false;
                    // jump to the next touch
                    break;
                }
                // if the joy is visible, a toutch is been tracked and not ended, update the 
                // updates the delta vector
                this.InputVector = Vector2.ClampMagnitude((touch.position - this.StartPosition), 1f);
            }
            // if is inside of the joy Detection area and is a new touch
            else if (JoyDetectionArea.Contains(touch.position) && touch.phase == TouchPhase.Began)
            {
                // set the joy to visible
                IsJoyStickVisible = true;
                // Store the information of this new valid touch
                this.ValidJoyTouchID = touch.fingerId;
                this.StartPosition = touch.position;

                // place the joy on the touch position
                this.joyBackRect.anchoredPosition = touch.position;
            }

        }
    }

    #endregion

    #region Getter/Setter
    /// <summary>
    /// Get and set the state of the input controller
    /// </summary>
    /// <value>New State for the controller</value>
    public bool InputState
    {
        get { return IsEnable; }    // Return the current input State
        set { this.IsEnable = value; }   // Set a new State to the input system
    }

    /// <summary>
    /// Set and get the joystick visible state
    /// </summary>
    /// <value>Is Visible value</value>
    public bool IsJoyStickVisible
    {
        // retorn if the joystickIsVisible
        get { return this.JoyStickVisible; }
        // Set the visible state to the new value
        set
        {
            // Stores the new value
            this.JoyStickVisible = value;
            // Check if exists a ref to the imgs
            if (joyBackImg && JoyKnobImg)
            {
                // Set image state to the new value
                joyBackImg.enabled = JoyKnobImg.enabled = this.JoyStickVisible;
                // if the joyStick changed to hided, reset the input vector
                if (!this.JoyStickVisible) this.InputVector = Vector2.zero;
            }
        }
    }
    #endregion
}