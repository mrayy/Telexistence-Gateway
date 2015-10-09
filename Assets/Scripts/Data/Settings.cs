using UnityEngine;
using System.Collections;

public class Settings  {

	INIParser _robotSettings;
	public RobotConnector.TargetPorts TargetPorts;

	 Settings()
	{
		_robotSettings = new INIParser ();
		_robotSettings.Open (Application.dataPath + "\\Data\\RobotSettings.ini");
		/*
		TargetPorts.VideoPort = _robotSettings.ReadValue ("Ports", "VideoPort", 7000);
		TargetPorts.AudioPort = _robotSettings.ReadValue ("Ports", "AudioPort", 7005);
		TargetPorts.HandsPort = _robotSettings.ReadValue ("Ports", "HandsPort", 7010);*/
		TargetPorts.CommPort = _robotSettings.ReadValue ("Ports", "CommPort", 6000);
	}

	public INIParser RobotSettings
	{
		get{
			return _robotSettings;
		}
	}

	public int GetPortValue(string name)
	{
		return _robotSettings.ReadValue ("Ports", name, 0);
	}
	/*
	public string GetValue(string category,string name,string defaultValue)
	{
		return _robotSettings.ReadValue (category, name, defaultValue);
	}*/



	static Settings _instance=new Settings();

	public static Settings Instance
	{
		get
		{
			return _instance;
		}
	}
}
