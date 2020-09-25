using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    // private reff
    private GameObject leftHandGO, rightHandGO;    // ref to the hand joints
    private ConfigurableJoint leftHandJoint, rightHandJoint;    // ref to joints connecting obj to and
    private BoxCollider actionTrigger;    // reff to the action trigger
    private bool isEnable;    // controller state

    private GameObject inTriggerArea;   // Reff to object in action area

    #region public Methods
    // init and enable
    /// <summary>
    /// Init the controller and and set the controller state
    /// </summary>
    ///<param name="controllerState">Set the state of the controller when init</param>
    public void Init(bool controllerState, ref GameObject lHand, ref GameObject rHand)
    {        // set the ref to the hand configurable joints
        leftHandGO = lHand; rightHandGO = rHand; isEnable = controllerState;
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
        if (leftHandJoint || rightHandJoint)
        {
            // remove the joint component
            Destroy(leftHandJoint);
            Destroy(rightHandJoint);

            // if any hand as a object attatch, realease 
            leftHandJoint = null;
            leftHandJoint = null;

            // enable the action trigger
            actionTrigger.enabled = true;

            //reset the rigidbody
            inTriggerArea.GetComponent<Rigidbody>().isKinematic = false;
            inTriggerArea.GetComponent<Rigidbody>().useGravity = true;

            // remove object from parent
            inTriggerArea.transform.parent = null;
            // reset the observed obj
            inTriggerArea = null;


            // Debug to information pannel
            InformationPanel.DebugConsoleInput("Realeased Object");
        }
        // if there is notting grabbed and exist a target object
        else if (!leftHandJoint && !rightHandJoint && inTriggerArea)
        {
            // create joints on target obj to move hads to obj
            leftHandJoint = inTriggerArea.AddComponent<ConfigurableJoint>();
            rightHandJoint = inTriggerArea.AddComponent<ConfigurableJoint>();

            // Commun setting
            var tempJointDrive = new JointDrive();
            tempJointDrive.positionSpring = 300f;
            tempJointDrive.maximumForce = Mathf.Infinity;

            // set Up
            // Left hand joint
            leftHandJoint.connectedBody = leftHandGO.GetComponent<Rigidbody>();
            leftHandJoint.autoConfigureConnectedAnchor = false;
            leftHandJoint.connectedAnchor = Vector3.zero;
            leftHandJoint.xDrive = leftHandJoint.yDrive = leftHandJoint.zDrive = tempJointDrive;
            leftHandJoint.connectedMassScale = 500f;

            // right hand joint
            rightHandJoint.connectedBody = rightHandGO.GetComponent<Rigidbody>();
            rightHandJoint.autoConfigureConnectedAnchor = false;
            rightHandJoint.connectedAnchor = Vector3.zero;
            rightHandJoint.xDrive = rightHandJoint.yDrive = rightHandJoint.zDrive = tempJointDrive;
            rightHandJoint.connectedMassScale = 500f;

            // obj to player
            // set grabbed obj to chilled
            inTriggerArea.transform.SetParent(this.transform);
            inTriggerArea.transform.localPosition += Vector3.up;
            inTriggerArea.GetComponent<Rigidbody>().isKinematic = true;
            inTriggerArea.GetComponent<Rigidbody>().useGravity = false;

            // deactivate the action trigger
            actionTrigger.enabled = false;

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
