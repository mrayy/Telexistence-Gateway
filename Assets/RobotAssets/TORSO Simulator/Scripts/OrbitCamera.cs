using UnityEngine;
using System.Collections;

public class OrbitCamera: MonoBehaviour {

	public GameObject pivot;

	Vector3 offset;

	private Transform goTransform;
	private Transform pivotTransform;

	public float RotateSpeed;
	public float CameraHeight;
	float theta;
	public float Radius;

	float Theta0 = -90;

	void Awake() {
		//goTransform = this.GetComponent<Transform> ();
		//pivotTransform = GameObject.FindWithTag ("Pivot").GetComponent<Transform> ();
	}

	void Start() {
		//offset = pivot.transform.position - transform.position;
	}

	void Update() {
		/*
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = pivot.transform.eulerAngles.y;
		
		Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);
		transform.position = pivot.transform.position - (rotation * offset);
		
		transform.LookAt (pivot.transform);
		*/
	}

	void LateUpdate() {
		/*
		if (Input.GetKey (KeyCode.N)) {
			goTransform.RotateAround(pivotTransform.position, Vector3.up, RotateSpeed);
		}

		if (Input.GetKey (KeyCode.M)) {
			goTransform.RotateAround(pivotTransform.position, -Vector3.up, RotateSpeed);
		}*/

		float x, y, z;
		float radTheta = (Theta0 - pivot.transform.eulerAngles.y) * Mathf.Deg2Rad;
		x = pivot.transform.position.x + Radius * Mathf.Cos (theta+radTheta);
		y = pivot.transform.position.y + CameraHeight;
		z = pivot.transform.position.z + Radius * Mathf.Sin (theta+radTheta);

		transform.position = new Vector3 (x, y, z);

		transform.LookAt (pivot.transform);

		if (Input.GetKey (KeyCode.N)) {
			theta+=RotateSpeed*Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.M)) {
			theta-=RotateSpeed*Time.deltaTime;
		}

	}

	public void AdjustHeightSlider(float newCameraHeight) {
		CameraHeight = newCameraHeight;
		//Debug.Log(CameraHeight.ToString());
	}

	public void AdjustDistanceSlider(float newDistance) {
		Radius = newDistance;
	}

	public void AdjustRadiusSlider(float newtheta) {
		theta = newtheta;
	}
}
