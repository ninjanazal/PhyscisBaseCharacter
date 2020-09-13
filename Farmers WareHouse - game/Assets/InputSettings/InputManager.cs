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

    // awake
    private void Awake()
    {
        // Initialize the singleton
        InitSingleton();
    }

    private void FixedUpdate()
    {
        // call registed delegates
        if (GetJumpValue()) jumpDelegate();
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
    ///
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
        if (Input.GetAxis("Jump") == 1f) return true;
        // return false if not
        return false;
    }

}