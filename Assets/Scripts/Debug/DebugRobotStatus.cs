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
		str+= "Head Rotation= " + _connector.HeadRotation.eulerAngles.ToString()+"\n";

		float[] jv = Robot.RobotJointValues;
		if (jv != null) {
			str+="Robot Joint Values: \n";
			for(int i=0;i<jv.Length;)
			{
				str+=jv[i].ToString() + "/ " +jv[i+1];
				if(i!=jv.Length-2)
					str+="\n";
				i+=2;
			}
		}
		return str;
	}
}
