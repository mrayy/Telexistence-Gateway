using UnityEngine;
using System.Collections;

public class DummyHeadTest : MonoBehaviour {
	
	void QuatToEuler(Quaternion q, ref Vector3 euler)
	{
		var rotation = q;
		float q0 = rotation.w;
		float q1 = rotation.y;
		float q2 = rotation.x;
		float q3 = rotation.z;
		
		euler.y = Mathf.Rad2Deg*(float)Mathf.Atan2(2 * (q0 * q1 + q2 * q3), 1 - 2 * (q1*q1 + q2*q2));
		euler.x = Mathf.Rad2Deg*(float)Mathf.Asin(2 * (q0 * q2 - q3 * q1));
		euler.z = Mathf.Rad2Deg*(float)Mathf.Atan2(2 * (q0 * q3 + q1 * q2), 1 - 2 * (q2*q2 + q3*q3));

	}

	public RobotConnectionComponent connector;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 euler =connector.Connector.HeadRotation.eulerAngles;
		QuatToEuler (connector.Connector.HeadRotation,ref euler);
		euler.y = -euler.y;
		euler.x = -euler.x;
		transform.localPosition = connector.Connector.HeadPosition;
		transform.localRotation = Quaternion.Euler(euler);;
	}
}
