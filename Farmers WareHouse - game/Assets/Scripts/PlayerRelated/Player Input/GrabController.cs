using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    // private reff
    private ConfigurableJoint leftHandJoint, rightHandJoint;    // ref to the hand joints
    private BoxCollider actionTrigger;    // reff to the action trigger
    private bool isEnable;    // controller state

    #region public Methods
    // init and enable
    /// <summary>
    /// Init the controller and and set the controller state
    /// </summary>
    ///<param name="controllerState">Set the state of the controller when init</param>
    public void Init(bool controllerState, ref ConfigurableJoint lHand, ref ConfigurableJoint rHand)
    {        // set the ref to the hand configurable joints
        leftHandJoint = lHand; rightHandJoint = rHand; isEnable = controllerState;
        // get ref to the action triggert
        actionTrigger = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// Callback that is called when the player presses the action button
    /// </summary>
    public void ActionCallback()
    {
        // only runs if the controller is enable
        if (!isEnable) return;

    }

    #endregion
}
