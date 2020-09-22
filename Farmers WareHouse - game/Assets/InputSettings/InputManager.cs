using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // SINGLETON PATTERN
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }    // Public  static ref

    // Jump Delegate
    public delegate void OnJumpDelegate();
    public event OnJumpDelegate jumpDelegate;

    // jump action click
    // set if the jumo button has been realease after press
    private bool jumpRealeased = false;

    // interation funtions
    public delegate void OnActionDelegate();
    public event OnActionDelegate actionDelegate;
    // action button pressed
    // evaluate if the button as been realeaded after pressed
    private bool actionPressed = false;

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

    ///<Summary>
    /// Init Singleton instance
    ///</Summary>
    public void InitSingleton()
    {
        // check if the singleton exists or not
        if (instance == null) instance = this;
        // If exists and is different from this, destroy
        else if (InputManager.Instance != this) Destroy(this);
    }

    ///<Summary>
    /// Get the input direction
    ///</Summary>
    public Vector2 GetInputVector()
    {
        // Return the a vector based on vertical and horizontal input values
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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

}