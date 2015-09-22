using UnityEngine;
using System.Collections;

public class NewMove: MonoBehaviour {

	public float Speed;
	public float turnSpeed;

	void Start() {
	}

	void Update() {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		if (Input.GetKey (KeyCode.W)) {
			Vector3 moveAmountForward = transform.forward * v * Speed;
			GetComponent<Rigidbody>().MovePosition (transform.position + moveAmountForward * Time.deltaTime);
			//GetComponent<Rigidbody>().velocity = moveAmountForward + Vector3.Scale (GetComponent<Rigidbody>().velocity, new Vector3 (0, 1, 0));
		}

		if (Input.GetKey (KeyCode.S)) {
			Vector3 moveAmountBackward = transform.forward * v * Speed;
			GetComponent<Rigidbody>().MovePosition (transform.position + moveAmountBackward * Time.deltaTime);
			//GetComponent<Rigidbody>().velocity = moveAmountBackward + Vector3.Scale (GetComponent<Rigidbody>().velocity, new Vector3 (0, 1, 0));
		}

		if (Input.GetKey (KeyCode.A)) {
			GetComponent<Rigidbody>().AddTorque(transform.up * h * turnSpeed, ForceMode.Force);
		}

		if (Input.GetKey (KeyCode.D)) {
			GetComponent<Rigidbody>().AddTorque(transform.up * h * turnSpeed, ForceMode.Force);
		}
	}
}