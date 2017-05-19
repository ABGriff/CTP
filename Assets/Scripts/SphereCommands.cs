using UnityEngine;

public class SphereCommands : MonoBehaviour
{
	Vector3 originalPosition;

	// Use this for initialization
	void Start()
	{
		// Grab the original local position of the sphere when the app starts.
		originalPosition = this.transform.localPosition;
	}

	// Called by SpeechManager when the user says the "Reset world" command
	void OnReset()
	{
		// If the sphere has a Rigidbody component, remove it to disable physics.
		var rigidbody = this.GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			DestroyImmediate(rigidbody);
		}

		// Put the sphere back into its original local position.
		this.transform.localPosition = originalPosition;
	}

	// Called by SpeechManager when the user says the "Drop sphere" command
	void OnDrop()
	{
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }
}