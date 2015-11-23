using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;

	void FixedUpdate ()
	{
		if(Input.GetKey(KeyCode.UpArrow))
		{
			GetComponent<Rigidbody>().transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
			//GetComponent<Rigidbody>().AddForce(Vector3.forward * moveSpeed);
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			GetComponent<Rigidbody>().transform.Translate (Vector3.back * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.LeftArrow))
		{
			GetComponent<Rigidbody>().transform.Rotate(0, 6, 0);
		}

		if (Input.GetKey (KeyCode.RightArrow)) {
			GetComponent<Rigidbody>().transform.Rotate(0, -6, 0);
		}

	}

}
