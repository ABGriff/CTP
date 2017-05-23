using UnityEngine;
//using HoloToolkit.Unity;

public class Commands : MonoBehaviour
{

    Vector3 originalPosition;
    //protected WorldAnchorManager anchorManager;

    // Use this for initialization
    void Start()
    {
        // Grab the original local position of the object when the app starts.
        originalPosition = transform.localPosition;
        Debug.Log(gameObject.name + "position");
    }

    public void StartTest()
    {
        var focusObject = gameObject;
        if (focusObject != null)
        {
            Debug.Log(gameObject.name + " test");
            //anchorManager.RemoveAnchor(gameObject); //Remove anchor temporarily
            Debug.Log("RemoveAnchor");
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
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

        // Put the sphere back into its original local position.
        //Debug.Log("transform");
        this.transform.localPosition = originalPosition;
        //Debug.Log("rotate");
        //originalRotation = transform.rotation;
        Debug.Log("done");
    }

    public void Test()
    {
        Debug.Log("Hello!");
    }
}