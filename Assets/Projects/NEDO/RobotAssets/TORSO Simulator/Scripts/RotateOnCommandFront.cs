using UnityEngine;
using System.Collections;

public class RotateOnCommandFront : MonoBehaviour {

	Vector3 rotationLimitFront;
	public float speed = 30.0f;
	//public Rigidbody owner;


	// Use this for initialization
	void Start () {
		rotationLimitFront.x = 270;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKey (KeyCode.U)) {
			rotationLimitFront.x-= speed*Time.fixedDeltaTime;
			//transform.Rotate(-Vector3.right * speed * Time.fixedDeltaTime);
			//owner.WakeUp();
				}
		if (Input.GetKey (KeyCode.I)) {
			rotationLimitFront.x+= speed * Time.fixedDeltaTime;
			//transform.Rotate(Vector3.right * speed * Time.fixedDeltaTime);
			//owner.WakeUp();
		}

		rotationLimitFront.x = Mathf.Clamp (rotationLimitFront.x, 180, 315);
		transform.localRotation = Quaternion.AngleAxis(rotationLimitFront.x,new Vector3(1,0,0));
	}
}