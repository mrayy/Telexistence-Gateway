using UnityEngine;
using System.Collections;

public class MoveStick : MonoBehaviour {

	public float Speed;
	Vector3 moveAmount;
	public float MinValue;
	public float MaxValue;

	// Use this for initialization
	void Start () {
		moveAmount = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.T)) {
			moveAmount.z+=Speed*Time.deltaTime;
			//transform.Translate (Vector3.forward * Speed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.G)) {
			moveAmount.z-=Speed*Time.deltaTime;
			//transform.Translate (-Vector3.forward * Speed * Time.deltaTime);
		}

		moveAmount.z = Mathf.Clamp (moveAmount.z, MinValue, MaxValue);
		transform.localPosition= moveAmount;
		//transform.position.z = Mathf.Clamp (transform.position.z, Min, Max);
	}

}
