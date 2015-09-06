using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RobotJointHolder : MonoBehaviour {

	[Serializable]
	public class JointInfo
	{
		public IRobotJoint Joint;
		public float Value;
	}
	public JointInfo[] Joints;
	public IRobotJoint Root;
	public bool Control=false;
	// Use this for initialization
	void Start () {
		IRobotJoint[] joints = GetComponentsInChildren<IRobotJoint> ();
		List<JointInfo> jList = new List<JointInfo> ();
	//	Root = null;
		foreach (var j in joints) {
			if(Root==null && j.ParentJont==null)
			{
				Root=j;
			}
			if(j.isFixed)
				continue;
			JointInfo ifo=new JointInfo();
			ifo.Joint=j;
			ifo.Value=0;
			jList.Add(ifo);
		}
		Joints = jList.ToArray ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Control) {
			foreach (var j in Joints) {
				j.Joint.Value= j.Value;
				if(j.Joint.Value!=j.Value)
					j.Value=j.Joint.Value;//get the updated value
			}
		}
	}
}
