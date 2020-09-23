using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    // private reff
    private FixedJoint leftHandJoint, rightHandJoint;    // ref to the hand joints
    private BoxCollider actionTrigger;    // reff to the action trigger
    private bool isEnable;    // controller state

    private GameObject inTriggerArea;   // Reff to object in action area

    #region public Methods
    // init and enable
    /// <summary>
    /// Init the controller and and set the controller state
    /// </summary>
    ///<param name="controllerState">Set the state of the controller when init</param>
    public void Init(bool controllerState, ref FixedJoint lHand, ref FixedJoint rHand)
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

        // if is enable and has a targetobject, check if is grabbing 
        if (leftHandJoint.connectedBody || rightHandJoint.connectedBody)
        {
            // if any hand as a object attatch, realease 
            leftHandJoint.connectedBody = null;
            rightHandJoint.connectedBody = null;

            // Debug to information pannel
            InformationPanel.DebugConsoleInput("Realeased Object");
        }
        // if there is notting grabbed and exist a target object
        else if (!leftHandJoint.connectedBody && !rightHandJoint.connectedBody && inTriggerArea)
        {
            // set the target obj to the connected body on configurable joints
            leftHandJoint.connectedBody = inTriggerArea.GetComponent<Rigidbody>();
            rightHandJoint.connectedBody = leftHandJoint.connectedBody;

            // debug to information pannet
            InformationPanel.DebugConsoleInput("Oject grabbed!");
        }
    }

    #endregion

    #region Private Methods
    // On trigger enter
    private void OnTriggerEnter(Collider other)
    {
        // check if exist some objecto been observed from this system
        if (inTriggerArea) return;
        // if this controller doesnt have a observed grabbable
        // evaluate if the object is a grabblable one
        if (other.CompareTag("Grabbable") && other.GetComponent<Rigidbody>())
            // if is a grabbable object and has a rigidbody add to observed reff
            this.inTriggerArea = other.gameObject;
    }

    // on trigger exit
    private void OnTriggerExit(Collider other)
    {
        // check if this object is the same been observed, if exist
        if (inTriggerArea && inTriggerArea.Equals(other.gameObject))
            // clear the observer reff
            inTriggerArea = null;
    }

    #endregion
}
