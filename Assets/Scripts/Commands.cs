using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class Commands : MonoBehaviour
{
    Vector3 velocity;
    Vector3 angularVelocity;

    Quaternion originalRotation;
    Vector3 originalPosition;
    protected WorldAnchorManager anchorManager;

    // Use this for initialization
    void Start()
    {
        // Grab the original local position of the object when the app starts.
        originalPosition = transform.localPosition;
        originalRotation = transform.rotation;

        //Debug.Log(gameObject.name + "position");
    }

    public void StartTest()
    {
        var focusObject = gameObject;
        if (focusObject != null)
        {
            //anchorManager.RemoveAnchor(gameObject); //Remove anchor temporarily
            //Debug.Log("RemoveAnchor");
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            Debug.Log("Test Started");
        }
        else
        {
            Debug.Log("focusObject is Null!");
        }
    }

    public void Reset()
    {
        // If the sphere has a Rigidbody component, remove it to disable physics.
        Debug.Log(gameObject.name + " reset");
        var rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            DestroyImmediate(rigidbody);
        }

        // Put the object back into its original local position and rotation.
        //Debug.Log("transform");
        this.transform.localPosition = originalPosition;
        this.transform.localRotation = originalRotation;
        //Debug.Log("rotate");
        //originalRotation = transform.rotation;
    }

    public void PauseTest()
    {
        //Pauses a current test in progress.
        Debug.Log(gameObject.name + "Pausing Test");
        var rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            //Grab velocity and angular velocity for resume later
            velocity = rigidbody.velocity;
            angularVelocity = rigidbody.angularVelocity;
            rigidbody.isKinematic = true;
        }
    }

    public void ContinueTest()
    {
        //Pauses a current test in progress.
        Debug.Log(gameObject.name + "Resuming Test");
        var rigidbody = this.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            //Grab velocity and angular velocity for resume later
            rigidbody.isKinematic = false;
            rigidbody.AddForce(velocity, ForceMode.VelocityChange);
            rigidbody.AddTorque(angularVelocity, ForceMode.VelocityChange);
            Debug.Log("Resumed Test");
        }
    }

    public void Test()
    {
        Debug.Log("Hello!");
    }
}