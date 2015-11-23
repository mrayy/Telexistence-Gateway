using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {

	public float speed;

	void Start () {


	
	}
	
	void Update () {

		if (Input.GetKey (KeyCode.F)) {
			transform.position += transform.TransformDirection (Vector3.right) * speed;
		}
	}
}
