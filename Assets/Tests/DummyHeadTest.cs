using UnityEngine;
using System.Collections;

public class DummyHeadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 e=OVRManager.display.GetEyePose (OVREye.Left).orientation.eulerAngles;
		transform.localRotation = Quaternion.Euler(e);
	}
}
