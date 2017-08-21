using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCommands : MonoBehaviour {

	Vector3 velocity;
	Vector3 angularVelocity;

	Quaternion originalRotation;
	Vector3 originalPosition;
	// Use this for initialization
	void Start () {
		// Grab the original local position of the object when the app starts.
		originalPosition = transform.localPosition;
		originalRotation = transform.rotation;

		Debug.Log(gameObject.name + "position");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	 public void StartTest()
    {
        var focusObject = gameObject;
        if (focusObject != null)
        {
			//anchorManager.RemoveAnchor(gameObject); //Remove anchor temporarily
			//Debug.Log("RemoveAnchor");
			this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            //rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            Debug.Log("Test Started");
        }
        else
        {
            Debug.Log("focusObject is Null!");
        }
    }

	public void Reset() {
		// If the sphere has a Rigidbody component, remove it to disable physics.
		Debug.Log(gameObject.name + " reset");
		var rigidbody = this.GetComponent<Rigidbody>();
		if (rigidbody != null) {
			this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}

		// Put the object back into its original local position and rotation.
		//Debug.Log("transform");
		this.transform.localPosition = originalPosition;
		this.transform.localRotation = originalRotation;
		//Debug.Log("rotate");
		//originalRotation = transform.rotation;
	}

    public void PauseContinueTest() {
        //Pauses or Continues a current test in progress.

        var rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null) {
            if (rigidbody.isKinematic == false) {
                rigidbody.isKinematic = true;
                Debug.Log(gameObject.name + "Test Paused");
            }
            else {
                rigidbody.isKinematic = false;
                Debug.Log(gameObject.name + "Test Continued");
            }
        }
    }

}
