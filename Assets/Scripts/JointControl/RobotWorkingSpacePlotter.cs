using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class RobotWorkingSpacePlotter : MonoBehaviour {

	public RobotJointHolder Joints;
	public IRobotJoint EndEffector;
	public int SamplesPerJoints=50;

	List<IRobotJoint> _traverseJointList=new List<IRobotJoint>();

	public Color Color;

	public bool Calculate=false;
	bool _isCalculated=false;

	 Vector3[] points;
	 Color[] colors;
	// Use this for initialization
	void Start () {
	
	}

	int _CalculateJoint(int jointID, int index,int jointNumber)
	{
		if (jointID < 0)
			return index;//something wrong..
		IRobotJoint joint = _traverseJointList [jointID];
		if (EndEffector == joint) {
			joint._UpdateJoint();
			points [index] = transform.InverseTransformPoint(joint.GetAbsPosition());
			colors[index]=Color;
			++index;
		} else {
			//IRobotJoint[] joints = joint.GetComponentsInChildren<IRobotJoint> ();

			if (joint.isFixed) {
				joint._UpdateJoint();
				index=_CalculateJoint (jointID-1,  index,jointNumber+1);
				//	foreach (var i in joints) {
			//		if (i.ParentJont == joint) {
				//		index=_CalculateJoint (i,  index,jointNumber+1);
				//	}
			//	}
			} else
			{
				int samples=SamplesPerJoints;//(int)((float)SamplesPerJoints/(float)(jointNumber+1));
				float step = (joint.MaxLimit - joint.MinLimit) / (float)(samples-1);
				for (int i=0; i<samples; ++i) {
					joint.SetValue(joint.MinLimit+ i*step);
					index=_CalculateJoint (jointID-1,  index,jointNumber+1);
				//	foreach (var j in joints) {
				//		if (j.ParentJont == joint) {
				//			index=_CalculateJoint (j,  index,jointNumber+1);
				//		}
				//	}
				}
			}
		}
		return index;
	}

	void _CalculateWorkingSpace()
	{
		MeshFilter mf = GetComponent<MeshFilter> ();
		if (mf.mesh == null) {
			mf.mesh=new Mesh();
		}
		Mesh mesh = mf.mesh;
		_traverseJointList.Clear ();

		int totalCount = 0;
		int vertCount = 1;
		IRobotJoint jp = EndEffector.ParentJont;
		_traverseJointList.Add(EndEffector);
		while(jp!=null)
		{
			if(!jp.isFixed && jp!=EndEffector)
			{
				++totalCount;
				vertCount=vertCount*SamplesPerJoints;///totalCount;
			}
			_traverseJointList.Add(jp);
			if(jp==Joints.Root)
				break;
			jp=jp.ParentJont;
		}
		Debug.Log ("Joints count:" + totalCount);
		Debug.Log ("Target samples count:" + vertCount);
		if (vertCount >= 65534) {
			Debug.LogError("Vertex count exceeded maximum index!");
			return;
		}
		points = new Vector3[vertCount];
		int[] indecies = new int[vertCount];
		colors = new Color[vertCount];
		for(int i=0;i<vertCount;++i)
		{
			indecies[i]=i;
		}
		
		_CalculateJoint (_traverseJointList.Count-1, 0,1);

		mesh.vertices = points;
		mesh.colors = colors;
		mesh.SetIndices(indecies,MeshTopology.Points, 0);
	}

	// Update is called once per frame
	void Update () {
		if (Calculate && !_isCalculated) {
			_CalculateWorkingSpace();
			_isCalculated=true;
		}
		if (!Calculate)
			_isCalculated = false;
	}
}
