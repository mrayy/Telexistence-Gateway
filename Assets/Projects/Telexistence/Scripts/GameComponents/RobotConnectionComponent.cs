﻿using UnityEngine;
using System.Collections;
using System.IO;

public class RobotConnectionComponent : DependencyRoot {

	RobotConnector _connector;

	public AppManager.HeadControllerType HeadControllerType;
	public AppManager.BaseControllerType BaseControllerType;

	//public TelubeeOVRCamera VideoStream;
	public DebugInterface Debugger;

	public bool NullValues=false;
	bool _connected=false;
	public int RobotIndex;

	//incase we using PLC lock
	public PLCDriverObject PLCDriverObject;

	RobotInfo _RobotIP;
	public RobotInfo RobotIP
	{
		get{
			return _RobotIP;
		}
	}

	float[] _RobotJointValues;


	public delegate void Delg_OnRobotConnected(RobotConnector.TargetPorts ports);
	public event Delg_OnRobotConnected OnRobotConnected;
	
	public delegate void Delg_OnRobotDisconnected();
	public event Delg_OnRobotDisconnected OnRobotDisconnected;

	public delegate void Delg_OnServiceNetValue(string serviceName,int port);
	public event Delg_OnServiceNetValue OnServiceNetValue;

	RobotDataCommunicator.ERobotControllerStatus _robotStatus=RobotDataCommunicator.ERobotControllerStatus.EStopped;
	public RobotDataCommunicator.ERobotControllerStatus RobotStatus
	{
		get{
			return _robotStatus;
		}
	}
	
	public bool IsConnected
	{
		get
		{
			return _connected;
		}
	}
	public bool IsRobotConnected
	{
		get
		{
			return Connector.IsRobotConnected;
		}
	}
	
	public RobotConnector Connector
	{
		get{
			return _connector;
		}
	}
	public float[] RobotJointValues {
		get {
			return _RobotJointValues;
		}
	}
	// Use this for initialization
	void Start () {
		_connector = new RobotConnector ();
		switch (HeadControllerType) {
		case AppManager.HeadControllerType.Oculus:
		default:
			_connector.HeadController=new OculusHeadController();
			break;
		}
		switch (BaseControllerType) {
		case AppManager.BaseControllerType.Oculus:
		default:
			_connector.BaseController=new OculusBaseController();
			break;
		}
		_connector.RobotCommunicator = new RemoteRobotCommunicator ();
		_connector.DataCommunicator.OnMessage += OnMessage;
		_connector.DataCommunicator.OnRobotInfoDetected += OnRobotInfoDetected;
		_connector.DataCommunicator.OnRobotStatus += OnRobotStatus;
		_connector.DataCommunicator.OnJointValues += OnJointValues;
		_connector.DataCommunicator.OnBumpSensor += OnBumpSensor;
		_connector.DataCommunicator.OnServiceNetValue += _OnServiceNetValue;

		_connector.StartDataCommunicator ();
		
		if(RobotIndex>=0)
			_RobotIP=AppManager.Instance.RobotManager.GetRobotInfo(RobotIndex);


		//Send Detect Message to scan network for the available robots
		_connector.RobotCommunicator.SetData ("detect", _connector.DataCommunicator.Port.ToString(), true,false);
		_connector.RobotCommunicator.BroadcastMessage (Settings.Instance.GetPortValue("CommPort",6000));
		_connector.RobotCommunicator.RemoveData ("detect");

		if (Debugger != null) {
			Debugger.AddDebugElement(new DebugRobotStatus(this));
		}

		//VideoStream.SetConnectionComponent (this);

		_OnStarted ();
	}
	void OnMessage(int message,BinaryReader reader)
	{
	//	Debug.Log ("Message Arrived: " + message.ToString());
		if (message == (int)RobotDataCommunicator.Messages.RobotStatus) {
			int status= reader.ReadInt32();
			
		//	Debug.Log ("Robot Status: " + ((RobotDataCommunicator.ERobotControllerStatus)status).ToString());
		}
	}

	void OnJointValues(float[] values)
	{
		_RobotJointValues = values;

		if (PLCDriverObject != null) {
			PLCDriverObject.OnTorsoJointValues(values);
		}
	}

	void OnRobotStatus(RobotDataCommunicator.ERobotControllerStatus status)
	{
		_robotStatus = status;

		if (PLCDriverObject != null) {
			PLCDriverObject.OnRobotStatus(status);
		}
	}
	void OnRobotInfoDetected(RobotInfo ifo)
	{
		Debug.Log ("Robot detected: " + ifo.IP);
		if(_RobotIP==null)
			_RobotIP = ifo;
	}
	void OnBumpSensor(float[] v)
	{
	}
	
	public void OnCameraFPS(int c0,int c1)
	{
		if (PLCDriverObject != null) {
			PLCDriverObject.OnCameraFPS(c0,c1);
		}
	}
	public void _OnServiceNetValue(string serviceName,int port)
	{
		Debug.Log (serviceName + ":" + port);
		if(OnServiceNetValue!=null)
			OnServiceNetValue(serviceName,port);
	}
	void OnDestroy()
	{
		_connector.Dispose();
		_connector = null;
	}

	// Update is called once per frame
	void Update () {
		_connector.NullValues = NullValues;
		_connector.Update ();

		if (IsConnected) {
			_connector.SendData("query","",false);
			_connector.SendData("jointVals","",false);
		}
	}
	
	public void ConnectRobot()
	{
		RobotInfo ifo=_RobotIP;
		ConnectRobot(ifo);
	}
	public void ConnectRobot(RobotInfo r)
	{
		if (_connected) {
			if(r.IP==_RobotIP.IP)
				return;
			DisconnectRobot ();
		}
		_RobotIP = r;

		RobotConnector.TargetPorts ports = new RobotConnector.TargetPorts ();

		ports.CommPort = Settings.Instance.GetPortValue ("CommPort", 6000);
		ports.RobotIP = _RobotIP.IP;
		_connector.ConnectRobotIP (ports);
		_connected=true;
/*
		if (VideoStream != null) {
			VideoStream.SetRemoteHost(_RobotIP.IP,ports);
		}*/

		if (OnRobotConnected!=null) {
			OnRobotConnected(ports);
		}

		

		LogSystem.Instance.Log ("Connecting to Robot:" + _RobotIP.Name, LogSystem.LogType.Info);
	}

	public void DisconnectRobot()
	{
		if (!_connected)
			return;
		_connector.DisconnectRobot ();
		if (OnRobotDisconnected != null) {
			OnRobotDisconnected();
		}
		_connected = false;
		LogSystem.Instance.Log ("Disconnecting Robot:" + _RobotIP.Name, LogSystem.LogType.Info);
	}

	public void StartUpdate()
	{
		if (!_connected) 
			return;
		Recalibrate();
		_connector.StartUpdate();
		LogSystem.Instance.Log ("Start Updating Robot:" + _RobotIP.Name, LogSystem.LogType.Info);
	}
	
	public void EndUpdate()
	{
		if (!_connected) 
			return;
		_connector.EndUpdate();
		LogSystem.Instance.Log ("End Updating Robot:" + _RobotIP.Name, LogSystem.LogType.Info);
	}
	public bool IsRobotStarted()
	{
		return _connected && _connector.IsRobotConnected;
	}
	public void Recalibrate()
	{
		_connector.Recalibrate ();
	}

}
