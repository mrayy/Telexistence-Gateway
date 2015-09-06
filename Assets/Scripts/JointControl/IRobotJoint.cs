using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class IRobotJoint : MonoBehaviour {
	
	public enum JoinType
	{
		Prismatic,
		Revolute
	}


	public int ID;

	//DH Parameters for this link
	public float linkAngle;
	public float linkOffset;
	public float length;
	public float twist;

	public bool isFixed=false;

	public JoinType Type;

	public float MinLimit;
	public float MaxLimit;

	//physics parameters for this joint
	public float Spring;
	public float Damping;

	public IRobotJoint ParentJont;

	public Matrix4x4 DHMatrix;
	public Matrix4x4 DHMatrixWorld;

	// Use this for initialization
	void Start () {
		ParentJont= transform.parent.GetComponent<IRobotJoint>();
	}

	void _CalculateDHMatrix()
	{
		if (ParentJont) {
			Matrix4x4 m1, m2;
			m1 = Matrix4x4.TRS (new Vector3 (0, 0, 0), Quaternion.AngleAxis (linkAngle, Vector3.forward), Vector3.one);
			m2 = Matrix4x4.TRS (new Vector3 (length, 0, linkOffset), Quaternion.AngleAxis (twist, Vector3.right), Vector3.one);

			DHMatrix = m1 * m2;

			if (ParentJont) {
			
				DHMatrixWorld = ParentJont.DHMatrixWorld * DHMatrix;
			} else {
				DHMatrixWorld = DHMatrix;
			}
			transform.FromMatrix4x4 (DHMatrix);
		} else {
			DHMatrixWorld=transform.localToWorldMatrix;
		}
	}


	public void _UpdateJoint()
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
	// Update is called once per frame
	void Update () {
		
		_UpdateJoint ();
		if (ParentJont) {
			Debug.DrawLine (ParentJont.DHMatrixWorld.GetColumn (3), DHMatrixWorld.GetColumn (3), Color.black);

		} 
		Utilities.DrawAxis (DHMatrixWorld);
	}

	public Vector3 GetAbsPosition()
	{
		return this.DHMatrixWorld.GetPosition ();
	}

	public void SetValue(float v)
	{
		if (Type == JoinType.Prismatic) {
			linkOffset = v;
		} else {
			linkAngle = v;
		}
		_UpdateJoint ();
	}

	public float Value
	{
		set
		{
			SetValue (value);
		}
		get{
			if (Type == JoinType.Prismatic) {
				return linkOffset;
			} else {
				return linkAngle;
			}
		}
	}
}
