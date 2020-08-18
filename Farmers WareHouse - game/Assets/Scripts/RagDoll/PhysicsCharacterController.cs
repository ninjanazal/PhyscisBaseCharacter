using System;
using UnityEngine;

public class PhysicsCharacterController : MonoBehaviour
{
    // Variaveis publicas

    // private var
    private GameObject[] parts = new GameObject[11];

    #region Private Methods
    // On Start
    private void Start()
    {
        // evaluate if all the parts have a GO reffered
        CheckAllComponents();

    }

    // Internal methods
    /// <summary>
    /// check if all the body components are referend correctly
    /// </summary>
    private void CheckAllComponents()
    {
    }
    #endregion

    #region Public Methods



    #endregion
}
