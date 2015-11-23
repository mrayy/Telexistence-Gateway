using UnityEngine;
using System.Collections;

public class FWKinematicsRobotJoint : IRobotJoint {

	public enum JointAxis
	{
		X,Y,Z
	}

	public JointAxis Axis;

	float _jointValue;

	public float Value;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
		SetValue (Value);
	}


	
	public override void _UpdateJoint()
	{
		switch (Axis) {
		case JointAxis.X:
			this.transform.localRotation=Quaternion.Euler(_jointValue,0,0);
			break;
		case JointAxis.Y:
			this.transform.localRotation=Quaternion.Euler(0,_jointValue,0);
			break;
		case JointAxis.Z:
			this.transform.localRotation=Quaternion.Euler(0,0,_jointValue);
			break;
		}
	}
	protected override void _DebugRender()
	{
	}

	
	public override Vector3 GetAbsPosition ()
	{
		
		return this.transform.position;
	}
	
	public override void SetValue(float v)
	{
		_jointValue = v;
		_UpdateJoint ();
	}
	
	public override float GetValue()
	{
		return _jointValue;
	}
}
