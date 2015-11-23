using UnityEngine;
using System.Collections;

public class IRobotJointLink : MonoBehaviour {
	public enum JoinType
	{
		Prismatic,
		Revolute
	}
	
	public enum AxisName
	{
		X,
		Y,
		Z
	}
	
	public int ID;
	public AxisName Axis;
	public JoinType Type;

	

	public bool IsLimited;
	public float MinLimit;
	public float MaxLimit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
