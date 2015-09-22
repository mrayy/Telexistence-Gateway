using UnityEngine;
using System.Collections;

public class DebugRobotStatus : DebugInterface.IDebugElement {

	public RobotConnectionComponent Robot;
	public DebugRobotStatus (RobotConnectionComponent r)
	{
		Robot = r;
	}
	
	public string GetDebugString()
	{
		RobotConnector _connector = Robot.Connector;

		string str = "";
		str+="Is  Connected: "+Robot.IsConnected+"\n";
		if (Robot.IsConnected) {
			str += "Is Robot Connected: " + Robot.IsRobotConnected + "\n";
			str += "Robot Status: " + Robot.RobotStatus + "\n";
		}
		str+="Head Position= " + _connector.HeadPosition.ToString()+"\n";
		str+= "Head Rotation= " + _connector.HeadRotation.eulerAngles.ToString();
		return str;
	}
}
