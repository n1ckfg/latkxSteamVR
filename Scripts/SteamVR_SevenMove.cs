using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.reddit.com/r/Unity3D/comments/a6rmbh/vr_question_steamvr_2_scale_object_with_both_hands/

public class SteamVR_SevenMove : MonoBehaviour {

    public SteamVR_NewController steamCtlMain;
    public SteamVR_NewController steamCtlAlt;
    public Transform target;
	public bool clicked = false;

    private Vector3 initialHandPosition1; // first controller
    private Vector3 initialHandPosition2; // second controller
    private Quaternion initialObjectRotation; // target rotation
    private Vector3 initialObjectScale; // target scale
    private Vector3 initialObjectDirection; // direction of target to midpoint of both controllers

    private void Update() {
        if (clicked) {
            if (!steamCtlMain.gripped && !steamCtlAlt.gripped) {
                clicked = false;
                return;
            } else {
                updateTarget();
            }
        } else {
            if (steamCtlMain.gripDown || steamCtlAlt.gripDown) {
                attachTarget();
                updateTarget();
                clicked = true;
            }
        }
    }

    private void attachTarget() {
        initialHandPosition1 = steamCtlMain.transform.position;
        initialHandPosition2 = steamCtlAlt.transform.position;
        initialObjectRotation = target.transform.rotation;
        initialObjectScale = target.transform.localScale;
        initialObjectDirection = target.transform.position - (initialHandPosition1 + initialHandPosition2) * 0.5f; 
    }

    private void updateTarget() {
        Vector3 currentHandPosition1 = steamCtlMain.transform.position; // current first hand position
        Vector3 currentHandPosition2 = steamCtlAlt.transform.position; // current second hand position

        Vector3 handDir1 = (initialHandPosition1 - initialHandPosition2).normalized; // direction vector of initial first and second hand position
        Vector3 handDir2 = (currentHandPosition1 - currentHandPosition2).normalized; // direction vector of current first and second hand position 

        Quaternion handRot = Quaternion.FromToRotation(handDir1, handDir2); // calculate rotation based on those two direction vectors
        
        float currentGrabDistance = Vector3.Distance(currentHandPosition1, currentHandPosition2);
        float initialGrabDistance = Vector3.Distance(initialHandPosition1, initialHandPosition2);
        float p = (currentGrabDistance / initialGrabDistance); // percentage based on the distance of the initial positions and the new positions

        Vector3 newScale = new Vector3(p * initialObjectScale.x, p * initialObjectScale.y, p * initialObjectScale.z); // calculate new object scale with p

        target.transform.rotation = handRot * initialObjectRotation; // add rotation
        target.transform.localScale = newScale; // set new scale
        
        // set the position of the object to the center of both hands based on the original object direction relative to the new scale and rotation
        target.transform.position = (0.5f * (currentHandPosition1 + currentHandPosition2)) + (handRot * (initialObjectDirection * p));
    }

}
