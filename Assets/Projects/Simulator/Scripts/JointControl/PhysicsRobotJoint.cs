using UnityEngine;
using System.Collections;

public class PhysicsRobotJoint : IRobotJoint {

	public enum JointAxis
	{
		X,Y,Z
	}

	public JointAxis Axis;

	float _jointValue;

	HingeJoint _jointHandle;

	void _createJoint()
	{
		if (ParentJoint != null && ParentJoint.GetComponent<Rigidbody> () != null) {
			_jointHandle = gameObject.GetComponent<HingeJoint>();
			if(_jointHandle==null)
				_jointHandle=gameObject.AddComponent<HingeJoint> ();

			_jointHandle.connectedBody=ParentJoint.GetComponent<Rigidbody>();

			_jointHandle.connectedAnchor=transform.localPosition;

			switch(Axis)
			{
			case JointAxis.X:
				_jointHandle.axis=Vector3.left;
				break;
			case JointAxis.Y:
				_jointHandle.axis=Vector3.up;
				break;
			case JointAxis.Z:
				_jointHandle.axis=Vector3.forward;
				break;
			}
			
		}

	}
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		_createJoint ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
	}


	
	public override void _UpdateJoint()
	{
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
