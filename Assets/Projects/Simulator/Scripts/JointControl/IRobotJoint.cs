using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public abstract class IRobotJoint : MonoBehaviour {
	
	public enum JoinType
	{
		Prismatic,
		Revolute
	}


	public int ID;

	public bool isFixed=false;

	public JoinType Type;

	public float MinLimit;
	public float MaxLimit;

	//physics parameters for this joint
	public float Spring;
	public float Damping;

	public IRobotJoint ParentJoint;


	// Use this for initialization
	protected virtual void Start () {
		if(transform.parent!=null)
			ParentJoint= transform.parent.GetComponent<IRobotJoint>();

	}
	
	public abstract void _UpdateJoint();

	protected abstract void _DebugRender ();

	// Update is called once per frame
	protected virtual void Update () {
		
		_UpdateJoint ();

		_DebugRender ();

	}

	public abstract Vector3 GetAbsPosition ();

	public abstract void SetValue (float v);
	public abstract float GetValue ();

	public float Value
	{
		set
		{
			SetValue (value);
		}
		get{
			return GetValue();
		}
	}
}
