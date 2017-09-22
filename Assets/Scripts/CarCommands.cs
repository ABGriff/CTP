using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarCommands : MonoBehaviour {

    /// <summary>
    /// This script will holds the commands for our car!
    /// Alexander Burey 2017
    /// Common Time Playbox
    /// </summary>

    Vector3 velocity;
	Vector3 angularVelocity;

	Quaternion originalRotation;
	Vector3 originalPosition;
	List<GameObject> cars;

	 public void StartTest() {
        //find the car(s)
        cars = new List<GameObject>(GameObject.FindGameObjectsWithTag("Vehicle"));
		foreach (GameObject obj in cars) {
			if (obj.gameObject != null) {
                //Save the position and rotation of the car
                originalPosition = obj.transform.localPosition;
                originalRotation = obj.transform.rotation;
                //Make the car move, disable tap to place, disable isKinematic and enable the car ai control
                obj.gameObject.GetComponent<TapToPlace>().enabled = false;
				obj.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				obj.gameObject.GetComponent<CarAIControl>().enabled = true;
			} else {
				Debug.Log("focusObject is Null!");
			}
		}
    }

	public void Reset() {
        //find the car(s)
        cars = new List<GameObject>(GameObject.FindGameObjectsWithTag("Vehicle"));
		var rigidbody = this.GetComponent<Rigidbody>();
		foreach (GameObject obj in cars) {
            //Reset the car, enable taptoplace, enable is kinematic and disable the car ai control
            obj.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			obj.gameObject.GetComponent<TapToPlace>().enabled = true;
			obj.gameObject.GetComponent<CarAIControl>().enabled = false;

			// Put the object back into its original local position and rotation.
			obj.transform.localPosition = originalPosition;
			obj.transform.localRotation = originalRotation;
		}
	}

    //Pauses or Continues a current test in progress.
    public void PauseContinueTest() {
		cars = new List<GameObject>(GameObject.FindGameObjectsWithTag("Vehicle"));
		foreach (GameObject obj in cars) {
            //Check if car is in Start test mode first. Don't want to pause/continue a car that isnt in a test
            if (obj.transform.GetComponent<TapToPlace>().enabled == false) {
                if (obj.transform.GetComponent<Rigidbody>().isKinematic == false) {
                obj.transform.GetComponent<Rigidbody>().isKinematic = true;
			    obj.gameObject.GetComponent<CarAIControl>().enabled = false;
			} else {
                    obj.transform.GetComponent<Rigidbody>().isKinematic = false;
                    obj.gameObject.GetComponent<CarAIControl>().enabled = true;
                }
			}
		}
	}
}
