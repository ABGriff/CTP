using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTest : MonoBehaviour {

    /// <summary>
    /// This script will run the "Bridge Test" once the player has built their bridge from the prefabs available.
    /// Alexander Burey 2017
    /// Common Time Playbox
    /// </summary>

    //Parent of Main GameObject
    private GameObject parentMGO;
    //Main GameObject
    private GameObject mainGO;
    //parent of Partner GameObject
    private GameObject parentPGO;
    //partner game object NOT USED
    private GameObject partnerGO;
    //Snap points of our plank to connect to partner (GameObjects to add component)
    private GameObject snapPointFL;
    private GameObject snapPointBL;
    private GameObject snapPointFR;
    private GameObject snapPointBR;
    //Snap points of partner (Rigidbodies to add component later)
    private Rigidbody partnersnapPointFR;
    private Rigidbody partnersnapPointBR;
    private GameObject closestObject;
    //Closest plank to our plank

        //!start will have to go into another function that can be called from the start test button btw
    // Use this for initialization
    void Start () {
        mainGO = gameObject;
        parentMGO = mainGO.transform.parent.gameObject;
        //connect fixed joints to Main plank
        ConnectFixedJoints();
        //Find the closest plank to attach to
        FindClosestPlank();
        //Connect each plank with respective hinge joints.
        ConnectHingeJoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //goes through each of our connected snap points and fixes them to the plank
    void ConnectFixedJoints() {
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapFL").gameObject.GetComponent<Rigidbody>();
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapBL").gameObject.GetComponent<Rigidbody>();
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapFR").gameObject.GetComponent<Rigidbody>();
        mainGO.AddComponent<FixedJoint>().connectedBody = parentMGO.transform.Find("SnapBR").gameObject.GetComponent<Rigidbody>();
        //old
        //foreach (Transform child in transform) {
        //    Debug.Log("Child" + child);
        //    mainGO.AddComponent<FixedJoint>().connectedBody = child.gameObject.GetComponent<Rigidbody>();
        //}
    }

    //Finds the closest plank to Main plank for later use
    void FindClosestPlank() {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Plank");
        foreach (GameObject obj in objectsWithTag) {
            //Check if looking at our current Plank, If so, ignore because of course its closest to iself
            //deep
            if (obj == mainGO) {
                continue;
            }
            if (!closestObject) {
                closestObject = obj;
            }
            //compares distances
            if (Vector3.Distance(transform.position, obj.transform.position) <=
                Vector3.Distance(transform.position, closestObject.transform.position)) {
                closestObject = obj;
            }
        }
        Debug.Log(mainGO + "'s closest plank is:" + closestObject);
        //Found the closest object, get the parent of this for later hinge use
        parentPGO = closestObject.transform.parent.gameObject;

    }

    void ConnectHingeJoints() {
        //Main Snap points that connect to partner plank with hinges. We need its rigid body for creating the hinge joints
        snapPointFL = parentMGO.transform.Find("SnapFL").gameObject;
        snapPointBL = parentMGO.transform.Find("SnapBL").gameObject;
        snapPointFR = parentMGO.transform.Find("SnapFR").gameObject;
        snapPointBR = parentMGO.transform.Find("SnapBR").gameObject;

        //Adds Hinge joints to our plank to left
        //Partner Snap points that get connected to. Parent > find snap > get GameObject > Get Rigidbody
        partnersnapPointFR = parentPGO.transform.Find("SnapFR").gameObject.GetComponent<Rigidbody>();
        partnersnapPointBR = parentPGO.transform.Find("SnapBR").gameObject.GetComponent<Rigidbody>();
        //Create and connect the hinge joints.
        snapPointFL.AddComponent<HingeJoint>().connectedBody = partnersnapPointFR;
        snapPointBL.AddComponent<HingeJoint>().connectedBody = partnersnapPointBR;
        //Add the Breakforce
        snapPointFL.GetComponent<HingeJoint>().breakForce = 250.0f;
        snapPointBL.GetComponent<HingeJoint>().breakForce = 250.0f;

        //Adds Hinge Joints to anchor left if there is one
    }
}
