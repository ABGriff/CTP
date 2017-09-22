using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTest : MonoBehaviour {

    /// <summary>
    /// This script will run the "Bridge Test" once the player has built their bridge from the prefabs available.
    /// Alexander Burey 2017
    /// Common Time Playbox
    /// </summary>
    /// 

    //Parent of Main GameObject
    private GameObject parentMGO;
    //Main GameObject
    private GameObject mainGO;
    //parent of Partner GameObject LEFT and RIGHT
    private GameObject parentPGOLeft;
    private GameObject parentPGORight;
    //check if the object to the right of us is a plank. If it is, ignore it. Otherwise its an anchor. connect to anchor with hinge.
    private bool parentPGORightPlankCheck = false;
    //check if the object to the left of us is a plank. If it is, connect hinge to FR and FL Snap points, if anchor, connect with hinge
    private bool parentPGOLeftPlankCheck = false;
    //partner game object //unused
    private GameObject partnerGO;
    //Closest Object to our Left and right
    private GameObject closestObjectLeft;
    private GameObject closestObjectRight;
    //Snap points of our plank to connect to partner (GameObjects to add component)
    private GameObject snapPointFL;
    private GameObject snapPointBL;
    private GameObject snapPointFR;
    private GameObject snapPointBR;
    //Snap points of partner plank to left (Rigidbodies to add component later)
    private Rigidbody partnersnapPointFR;
    private Rigidbody partnersnapPointBR;
    //snap points of partner ANCHOR to Right (Rigidbodies to add component later)
    private Rigidbody partnerAnchorF;
    private Rigidbody partnerAnchorB;
    //Original Position and rotation for resetting a test
    Quaternion originalRotation;
    Vector3 originalPosition;
    //Velocity and angular velocity for pausing a test
    Vector3 velocity;
    Vector3 angularVelocity;
    //Check to see if test has been started before. We don't want to re-create connections.
    bool resetCheck = false;

    // Use this for initialization
    void Start () {
        mainGO = gameObject;
        parentMGO = mainGO.transform.parent.gameObject;
       
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Commands for the GUI. Controls Start, Reset, Pause, Continue, Undo
    //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void startTest() {
        //Step 1: Save our objects position before the test and disable tap to place so the user can't mopve objects during a test 
        //avoid accidentally saving positions on pieces that went flying away 
        if (Vector3.Distance(originalPosition, transform.position) <= 5) {
            originalPosition = transform.localPosition;
            originalRotation = transform.rotation;
        }
        disableTapToPlace();

        ////Step 2: Connect all the pieces up
        //Find the closest planks to attach to
        FindClosestPlankLeft();
        FindClosestPlankRight();
        //Connect each plank/Anchor with respective hinge joints.
        ConnectHingeJointsLeft();
        //We only create hinge joints to the right if it's an anchor piece
        ConnectHingeJointsRight();
        resetCheck = true;

        ////Step 3: Activate Gravity
        mainGO.GetComponent<Rigidbody>().isKinematic = false;
        mainGO.GetComponent<Rigidbody>().useGravity = true;
    }

    //Resets the test for the planks
    public void resetTest() {
        var rigidbody = mainGO.GetComponent<Rigidbody>();
        if (rigidbody != null) {
            //Disable Gravity on planks, make kinematic true and enable tap to place
            mainGO.GetComponent<Rigidbody>().useGravity = false;
            mainGO.GetComponent<Rigidbody>().isKinematic = true;
            mainGO.GetComponent<TapToPlace>().enabled = true;
        }
        //Delete our hinges
        if (snapPointFR.GetComponent<HingeJoint>() != null) {
            Debug.Log("Delete Hinges FR/BR");
            Destroy(snapPointFR.GetComponent<HingeJoint>());
            Destroy(snapPointBR.GetComponent<HingeJoint>());
        }
        if (snapPointFL.GetComponent<HingeJoint>() != null) {
            Debug.Log("Delete Hinges FL/BL");
            Destroy(snapPointFL.GetComponent<HingeJoint>());
            Destroy(snapPointBL.GetComponent<HingeJoint>());
        }

        // Put the object back into its original local position and rotation.
        this.transform.localPosition = originalPosition;
        this.transform.localRotation = originalRotation;
    }
    //Pause and continue a bridge test
    public void pauseContinueTest() {
        //rigidbody is mainGO or our main GameObject
        var rigidbody = mainGO.GetComponent<Rigidbody>();
        if (rigidbody != null) {
            //if not kinematic (car/bridge is moving)
            if (rigidbody.isKinematic == false) {
                //save the velocity and angular velocity for when we continue
                velocity = rigidbody.velocity;
                angularVelocity = rigidbody.angularVelocity;
                //make that car/bridge freeze
                rigidbody.isKinematic = true;
            }
            //else we are already paused, let's continue
            else {
                rigidbody.isKinematic = false;
                //give back our pre-pause velocity and angular veloctiy
                rigidbody.AddForce(velocity, ForceMode.VelocityChange);
                rigidbody.AddTorque(angularVelocity, ForceMode.VelocityChange);
            }
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Step 1: Disable Tap To Place
    //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void disableTapToPlace() {
        mainGO.GetComponent<TapToPlace>().enabled = false;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Step 2A: Connect Fixed Joints for Plank to Snap Points
    //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //goes through each of our connected snap points and fixes them to the plank
    void ConnectFixedJoints() {
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapFL").gameObject.GetComponent<Rigidbody>();
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapBL").gameObject.GetComponent<Rigidbody>();
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapFR").gameObject.GetComponent<Rigidbody>();
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapBR").gameObject.GetComponent<Rigidbody>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Step 2B: Find the Closest Plank/Anchor to the left and Connect Hinge Joints to EITHER Plank or Anchor
    // 
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Finds the closest Plank/Anchor to THE LEFT
    //Apologies for the mess of If statements here
    void FindClosestPlankLeft() {
        List<GameObject> objectsLeft = new List<GameObject>(GameObject.FindGameObjectsWithTag("Anchor"));
        objectsLeft.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Plank")));

        foreach (GameObject obj in objectsLeft) {
            //Check if looking at our current Plank, If so, ignore because of course its closest to iself
            //deep
            if (obj == mainGO) {
                continue;
            }
            //if no closest object
            if (!closestObjectLeft) {
                closestObjectLeft = obj;
            }
            //Compare distances
            //if Distance between The Plank's SnapFL and next Object's AnchorPointF/SnapFL (Anchor/Plank) is less than distance between The Plank's SnapFL and the current 'closestObject' then update closestObject
            //if our current object we're checking is an anchor
            if (obj.gameObject.CompareTag("Anchor")) {
                //if our current closest object to check is an anchor
                if (closestObjectLeft.gameObject.CompareTag("Anchor")) {
                    if (Vector3.Distance(parentMGO.transform.Find("SnapFL").position, obj.transform.parent.Find("AnchorPointF").position) <= Vector3.Distance(parentMGO.transform.Find("SnapFL").position, closestObjectLeft.transform.parent.Find("AnchorPointF").position)) {
                        closestObjectLeft = obj;
                        parentPGOLeftPlankCheck = false;
                    }
                }
                //else the closest object to our left is a plank
                else {
                    if (Vector3.Distance(parentMGO.transform.Find("SnapFL").position, obj.transform.parent.Find("AnchorPointF").position) <= Vector3.Distance(parentMGO.transform.Find("SnapFL").position, closestObjectLeft.transform.parent.Find("SnapFR").position)) {
                        closestObjectLeft = obj;
                        parentPGOLeftPlankCheck = false;
                    }
                }
            }
            //if our current object we're checking is an anchor
            else if (obj.gameObject.CompareTag("Plank")) {
                //Have an if that checks if closestObjectLeft is plank or anchor. if its an anchor we need to check its specific point instead of its transform position, otherwise other planks can end up closer.
                if (closestObjectLeft.gameObject.CompareTag("Anchor")) {
                    if (Vector3.Distance(parentMGO.transform.Find("SnapFL").position, obj.transform.parent.Find("SnapFR").position) <= Vector3.Distance(parentMGO.transform.Find("SnapFL").position, closestObjectLeft.transform.parent.Find("AnchorPointF").position)) {
                        closestObjectLeft = obj;
                        parentPGOLeftPlankCheck = true;
                    }
                }
                else {
                    if (Vector3.Distance(parentMGO.transform.Find("SnapFL").position, obj.transform.parent.Find("SnapFR").position) <= Vector3.Distance(parentMGO.transform.Find("SnapFL").position, closestObjectLeft.transform.parent.Find("SnapFR").position)) {
                        closestObjectLeft = obj;
                        parentPGOLeftPlankCheck = true;
                    }
                }
            }
        }
        //Found the closest object, get the parent of this for later hinge use
        parentPGOLeft = closestObjectLeft.transform.parent.gameObject;
        Debug.Log(parentMGO + "'s closest Plank/Anchor to the Left is:" + parentPGOLeft);

    }

    void ConnectHingeJointsLeft() {
        //Main Snap points that connect to the anchor with hinges. We need its rigid body for creating the hinge joints
        snapPointFL = parentMGO.transform.Find("SnapFL").gameObject;
        snapPointBL = parentMGO.transform.Find("SnapBL").gameObject;

        //Check if anchor, if so create appropriate hinge joints
        if (!parentPGOLeftPlankCheck) {
            if (parentPGOLeft.transform.Find("RampL").gameObject.CompareTag("Anchor")) {
                //Partner Snap points that get connected to. Parent > find snap > get GameObject > Get Rigidbody
                partnerAnchorF = parentPGOLeft.transform.Find("AnchorPointF").gameObject.GetComponent<Rigidbody>();
                partnerAnchorB = parentPGOLeft.transform.Find("AnchorPointB").gameObject.GetComponent<Rigidbody>();
                //Create and connect the hinge joints.
                snapPointFL.AddComponent<HingeJoint>().connectedBody = partnerAnchorF;
                snapPointBL.AddComponent<HingeJoint>().connectedBody = partnerAnchorB;
                //Add the Breakforce
                snapPointFL.GetComponent<HingeJoint>().breakForce = 250.0f;
                snapPointBL.GetComponent<HingeJoint>().breakForce = 250.0f;
            }
        }
        //If not an anchor, it's a plank.
        else {
            //Partner Snap points that get connected to. Parent > find snap > get GameObject > Get Rigidbody
            partnerAnchorF = parentPGOLeft.transform.Find("SnapFR").gameObject.GetComponent<Rigidbody>();
            partnerAnchorB = parentPGOLeft.transform.Find("SnapBR").gameObject.GetComponent<Rigidbody>();
            //Create and connect the hinge joints.
            snapPointFL.AddComponent<HingeJoint>().connectedBody = partnerAnchorF;
            snapPointBL.AddComponent<HingeJoint>().connectedBody = partnerAnchorB;
            //Add the Breakforce
            snapPointFL.GetComponent<HingeJoint>().breakForce = 250.0f;
            snapPointBL.GetComponent<HingeJoint>().breakForce = 250.0f;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Step 2C: Find the Closest Plank/Anchor to the Right and Connect Hinge Joints if there's an Anchor ONLY
    //
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Finds the closest plank/anchor to THE RIGHT
    void FindClosestPlankRight() {
        List<GameObject> objectsWithTag = new List<GameObject>(GameObject.FindGameObjectsWithTag("Plank"));
        objectsWithTag.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Anchor")));

        foreach (GameObject obj in objectsWithTag) {
            //Check if looking at our current Plank, If so, ignore because of course its closest to iself
            //deep
            if (obj == mainGO) {
                continue;
            }
            //if no closest object
            if (!closestObjectRight) {
                closestObjectRight = obj;
            }
            //Compare distances
            //if Distance between The Plank's SnapFR and next Object's AnchorPointF/SnapFL (Anchor/Plank) is less than distance between The Plank's SnapFR and the current 'closestObject' then update closestObject
            if (obj.gameObject.CompareTag("Anchor")) {
                if (Vector3.Distance(parentMGO.transform.Find("SnapFR").position, obj.transform.parent.Find("AnchorPointF").position) <= Vector3.Distance(parentMGO.transform.Find("SnapFR").position, closestObjectRight.transform.position)) {
                    closestObjectRight = obj;
                    parentPGORightPlankCheck = false;
                }
            }
            else if (obj.gameObject.CompareTag("Plank")) {
                if (Vector3.Distance(parentMGO.transform.Find("SnapFR").position, obj.transform.parent.Find("SnapFL").position) <= Vector3.Distance(parentMGO.transform.Find("SnapFR").position, closestObjectRight.transform.position)) {
                    closestObjectRight = obj;
                    parentPGORightPlankCheck = true;
                }
            }
        }
        //Found the closest object, get the parent of this for later hinge use
        parentPGORight = closestObjectRight.transform.parent.gameObject;
        Debug.Log(parentMGO + "'s closest Plank/Anchor to the RIGHT is:" + parentPGORight);
        
    }

    void ConnectHingeJointsRight() {
        //Main Snap points that connect to the anchor with hinges. We need its rigid body for creating the hinge joints
        snapPointFR = parentMGO.transform.Find("SnapFR").gameObject;
        snapPointBR = parentMGO.transform.Find("SnapBR").gameObject;

        //if anchor, make hinges, otherwise dont do anything
        //check if it's an anchor to our right first so we avoid reference error.
        if (!parentPGORightPlankCheck) {
            if (parentPGORight.transform.Find("RampR")) {
                if (parentPGORight.transform.Find("RampR").gameObject.CompareTag("Anchor")) {
                    //Partner Snap points that get connected to. Parent > find snap > get GameObject > Get Rigidbody
                    partnerAnchorF = parentPGORight.transform.Find("AnchorPointF").gameObject.GetComponent<Rigidbody>();
                    partnerAnchorB = parentPGORight.transform.Find("AnchorPointB").gameObject.GetComponent<Rigidbody>();
                    //Create and connect the hinge joints.
                    snapPointFR.AddComponent<HingeJoint>().connectedBody = partnerAnchorF;
                    snapPointBR.AddComponent<HingeJoint>().connectedBody = partnerAnchorB;
                    //Add the Breakforce
                    snapPointFR.GetComponent<HingeJoint>().breakForce = 250.0f;
                    snapPointBR.GetComponent<HingeJoint>().breakForce = 250.0f;
                }
            }
        }
    }
}


