using UnityEngine;
using System.Collections;

public class HydraulicLookAt : MonoBehaviour {

	public Transform Target;
	public float speed;

	void Start () {

	}
	
	void Update () {

		Vector3 Direction = Target.transform.position - transform.position;
		Direction.Normalize ();
//		Quaternion Rotation = Quaternion.LookRotation (Direction);
//		transform.rotation = Quaternion.Slerp (Direction, Rotation, speed * Time.deltaTime);


		
		
		// and update the gameobject itself
		transform.rotation = Quaternion.LookRotation (Direction,-Vector3.forward);
	}
}
