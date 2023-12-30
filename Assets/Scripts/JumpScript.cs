using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    public float jumpHeight = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
		{
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
			// Remove vertical velocity to avoid "super jumps" on slope ends.
            RemoveVerticalVelocity();
			// Set jump vertical impulse velocity.
			float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
			velocity = Mathf.Sqrt(velocity);
			GetComponent<Rigidbody>().AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
		}
    }

    private void RemoveVerticalVelocity()
	{
		Vector3 horizontalVelocity = GetComponent<Rigidbody>().velocity;
		horizontalVelocity.y = 0;
		GetComponent<Rigidbody>().velocity = horizontalVelocity;
	}
}
