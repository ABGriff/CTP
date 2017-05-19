using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class BallDrop : MonoBehaviour
{

    Vector3 originalPosition;

    // Use this for initialization
    void Start()
    {
        // Grab the original local position of the sphere when the app starts.
        originalPosition = this.transform.localPosition;
    }

    public void OnDrop()
    {
        Debug.Log("start"); //gets here. 
        var focusObject = GazeGestureManager.Instance.FocusedObject;
        Debug.Log("beforeif");
        if (focusObject != null)
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            Debug.Log("Itworks");
        }
        Debug.Log("NO");
    }
}