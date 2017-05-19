using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBall : MonoBehaviour {

    Vector3 originalPosition;

    // Use this for initialization
    void Start () {
        originalPosition = this.transform.localPosition;
    }

    public void DropIt()
    {
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            Debug.Log("Testworks");
        }
        Debug.Log("Testnowork");
    }
}
