using UnityEngine;
using System.Collections;

public class DHRobotJoint : IRobotJoint {
	
	//DH Parameters for this link
	public float linkAngle;
	public float linkOffset;
	public float length;
	public float twist;

	
	public Matrix4x4 DHMatrix;
	public Matrix4x4 DHMatrixWorld;

	
	void _CalculateDHMatrix()
	{
		if ((ParentJoint as DHRobotJoint)!=null) {
			Matrix4x4 m1, m2;
			m1 = Matrix4x4.TRS (new Vector3 (0, 0, 0), Quaternion.AngleAxis (linkAngle, Vector3.forward), Vector3.one);
			m2 = Matrix4x4.TRS (new Vector3 (length, 0, linkOffset), Quaternion.AngleAxis (twist, Vector3.right), Vector3.one);
			
			DHMatrix = m1 * m2;

			DHMatrixWorld = (ParentJoint as DHRobotJoint).DHMatrixWorld * DHMatrix;
			transform.FromMatrix4x4 (DHMatrix);
		} else {
			DHMatrixWorld=transform.localToWorldMatrix;
		}
	}
	
	
	public override void _UpdateJoint()
	{
		if (!isFixed) {
			if (Type == JoinType.Prismatic) {
				linkOffset = Mathf.Clamp (linkOffset, MinLimit, MaxLimit);
			} else {
				linkAngle = Mathf.Clamp (linkAngle, MinLimit, MaxLimit);
			}
		}
		
		
		_CalculateDHMatrix ();
	}
	
	
	protected override void _DebugRender ()
	{
		if (ParentJoint) {
			Debug.DrawLine ((ParentJoint as DHRobotJoint).DHMatrixWorld.GetColumn (3), DHMatrixWorld.GetColumn (3), Color.black);
			
		} 
		Utilities.DrawAxis (DHMatrixWorld);
	}

	// Use this for initialization
	protected virtual void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		base.Update ();
	
	}

	public override Vector3 GetAbsPosition ()
	{
		
		return this.DHMatrixWorld.GetPosition ();
	}
	
	public override void SetValue(float v)
	{
		if (Type == JoinType.Prismatic) {
			linkOffset = v;
		} else {
			linkAngle = v;
		}
		_UpdateJoint ();
	}
	
	public override float GetValue()
	{
		if (Type == JoinType.Prismatic) {
			return linkOffset;
		} else {
			return linkAngle;
		}
	}
}
