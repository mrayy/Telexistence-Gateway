using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public GameObject target;
	Vector3 offset;

	void Start() {
		offset = target.transform.position - transform.position;
	}

	void LateUpdate(){

		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;

		Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);
		transform.position = target.transform.position - (rotation * offset);

		transform.LookAt (target.transform);

		}
	
}
