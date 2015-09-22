using UnityEngine;
using System.Collections;

public class RigidboyMove : MonoBehaviour {

	public float speed;
	public float maxVelocityChange;
	private bool grounded = false;
	
	
	
	void Awake () {
		GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	void FixedUpdate () {
		if (grounded) {
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;
			
			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);

		
		grounded = false;
	}
}
}