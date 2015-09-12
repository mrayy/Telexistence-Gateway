using UnityEngine;
using System.Collections;

public class DebugHeadPose : DebugInterface.IDebugElement {

	public RobotConnector _robot;
	public DebugHeadPose (RobotConnector r)
	{
		_robot = r;
	}
	
	public string GetDebugString()
	{

		string str= "Position= " + _robot.HeadPosition.ToString()+"\n";
		str+= "Rotation= " + _robot.HeadRotation.eulerAngles.ToString();
		return str;
	}
}
