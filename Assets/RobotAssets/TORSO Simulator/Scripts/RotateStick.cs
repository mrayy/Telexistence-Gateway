using UnityEngine;
using System.Collections;

public class RotateStick : MonoBehaviour {

	public float Speed;
	Vector3 rotateAmount;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKey (KeyCode.Y)) {
			//rotateAmount = transform.right;
			rotateAmount.x+=Speed * Time.fixedDeltaTime;// (rotateAmount * Speed * Time.fixedDeltaTime, Space.Self);
			//Debug.Log(rotateAmount.x.ToString());
		}
		if (Input.GetKey (KeyCode.H)) {
			//rotateAmount = transform.right;
			rotateAmount.x-=Speed * Time.fixedDeltaTime;// (rotateAmount * Speed * Time.fixedDeltaTime, Space.Self);
		}
		
		rotateAmount.x = Mathf.Clamp(rotateAmount.x, 0, 90);// * Mathf.Rad2Deg;
		transform.localRotation=Quaternion.Euler(rotateAmount);
		}
}
