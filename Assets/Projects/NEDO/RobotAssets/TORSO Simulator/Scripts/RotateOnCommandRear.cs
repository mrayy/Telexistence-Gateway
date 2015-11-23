using UnityEngine;
using System.Collections;

public class RotateOnCommandRear : MonoBehaviour {

	Vector3 rotationLimitRear;
	public float speed = 5.0f;
	//public Rigidbody owner;
	// Use this for initialization
	void Start () {
		rotationLimitRear.x = 270;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.J)) {
			rotationLimitRear.x+=speed*Time.fixedDeltaTime;
			//transform.Rotate(Vector3.right * speed * Time.fixedDeltaTime);
			//owner.WakeUp();
				}
		if (Input.GetKey (KeyCode.K)) {
			rotationLimitRear.x-=speed*Time.fixedDeltaTime;
			//transform.Rotate(-Vector3.right * speed * Time.fixedDeltaTime);
			//owner.WakeUp();
		}

		rotationLimitRear.x = Mathf.Clamp (rotationLimitRear.x, 225, 360);
		transform.localRotation = Quaternion.AngleAxis(rotationLimitRear.x,new Vector3(1,0,0));
	}
}
