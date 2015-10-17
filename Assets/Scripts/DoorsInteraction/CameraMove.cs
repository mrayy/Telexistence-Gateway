using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	GameObject _OvrCamera;

	// Use this for initialization
	void Start () {
		_OvrCamera = GameObject.Find ("OVRCameraRig");
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = this.transform.position;
		if (Input.GetKey(KeyCode.LeftArrow)){
			v.x -= 0.02f;
		}
		if (Input.GetKey(KeyCode.RightArrow)){
			v.x += 0.02f;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			v.z += 0.02f;
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			v.z -= 0.02f;
		}
		this.transform.position = v;
	
	}
}
