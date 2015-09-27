using UnityEngine;
using System.Collections;
using System;

public class RobotConnector:IDisposable{
	
	class RobotInfo
	{
		public bool Connected=false;//is connected with robot
		public bool Status=false;   //is robot connected and running
		public Vector2 Speed;
		public Quaternion HeadRotation;
		public Vector3 HeadPosition;
		public float Rotation=0;
	}
	
	public struct TargetPorts
	{
		public TargetPorts(string ip)
		{
			RobotIP=ip;
			 CommPort=-1;
			 VideoPort=-1;
			 AudioPort=-1;
			 HandsPort=-1;
			 ClockPort=-1;
			 Rtcp=false;
		}
		public string RobotIP;
		public int CommPort;
		public int VideoPort;
		public int AudioPort;
		public int HandsPort;
		public int ClockPort;
		public bool Rtcp;
	}

	public IRobotCommunicator RobotCommunicator;
	public IRobotHeadControl HeadController;
	public IRobotBaseControl BaseController;
	public RobotDataCommunicator DataCommunicator;

	RobotInfo _robotIfo;
	TargetPorts _ports;

	public bool NullValues=false;

	public TargetPorts Ports
	{
		get{
			return _ports;
		}
	}

	public Vector2 Speed
	{
		get{
			return _robotIfo.Speed;
		}
	}
	public Quaternion HeadRotation
	{
		get{
			return _robotIfo.HeadRotation;
		}
	}
	public Vector3 HeadPosition
	{
		get{
			return _robotIfo.HeadPosition;
		}
	}
	public float Rotation
	{
		get{
			return _robotIfo.Rotation;
		}
	}

	public bool IsRobotConnected
	{
		get
		{
			return _robotIfo.Status;
		}
	}


	public RobotConnector()
	{
		_robotIfo = new RobotInfo ();
		_ports = new TargetPorts ();
		DataCommunicator = new RobotDataCommunicator ();
	}

	public void StartDataCommunicator(int port)
	{
		DataCommunicator.Start(port);
	}
	public void StopDataCommunicator()
	{
		DataCommunicator.Stop ();
	}
	public void Dispose()
	{
		DisconnectRobot ();
		StopDataCommunicator ();
		RobotCommunicator = null;
		DataCommunicator = null;
	}

	public void ConnectRobot()
	{
		if (RobotCommunicator==null)
			return;
		if (_robotIfo.Connected)
			RobotCommunicator.Disconnect();


		_robotIfo.Connected = RobotCommunicator.Connect(_ports.RobotIP, _ports.CommPort);
		RobotCommunicator.ClearData(true);
		//	m_roboComm->Connect("127.0.0.1",3000);
		RobotCommunicator.SetUserID("yamens");
		RobotCommunicator.ConnectUser(true);
		string addrStr = Utilities.LocalIPAddress();
		addrStr += "," + _ports.VideoPort.ToString();
		addrStr += "," + _ports.AudioPort.ToString();
		addrStr += "," + _ports.HandsPort.ToString();
		addrStr += "," + _ports.ClockPort.ToString();
		addrStr += "," + _ports.Rtcp.ToString();
		RobotCommunicator.SetData("Connect", addrStr,true);
		
		addrStr =  _ports.CommPort.ToString();
		RobotCommunicator.SetData("CommPort", addrStr, true);
		
	}
	public void ConnectRobotIP(TargetPorts ports)
	{
		_ports = ports;
		ConnectRobot ();
	}
	public void DisconnectRobot()
	{
		if (RobotCommunicator==null)
			return;
		if (!_robotIfo.Connected)
			return;
		string ipAddr=Utilities.LocalIPAddress ();

		string addrStr = ipAddr+","+_ports.VideoPort;
		RobotCommunicator.SetData ("shutdown", "", false);
		RobotCommunicator.SetData("Disconnect", addrStr, true);
		RobotCommunicator.Update(0);//only once
		EndUpdate();
		RobotCommunicator.Disconnect();
		_robotIfo.Connected = false;
	}
	public void StartUpdate()
	{
		if (RobotCommunicator==null)
			return;
		if (!_robotIfo.Connected)
			return;
		_robotIfo.Status = true;
		
		RobotCommunicator.ConnectRobot(true);
	}
	public void EndUpdate()
	{
		if (RobotCommunicator==null)
			return;
		_robotIfo.Status = false;
		RobotCommunicator.SetData("HeadRotation", Quaternion.identity.ToString(), false);
		RobotCommunicator.Update(0);//only once
		RobotCommunicator.ConnectRobot(false);

	}

	 void HandleController()
	{
		if (BaseController != null) {
			_robotIfo.Speed = BaseController.GetSpeed ();
			if (_robotIfo.Speed.x < 0)
				_robotIfo.Speed *= 0.1f;
			_robotIfo.Rotation = BaseController.GetRotation ();
		}
		if (HeadController!=null) {
			HeadController.GetHeadOrientation(out _robotIfo.HeadRotation, false);
			HeadController.GetHeadPosition(out _robotIfo.HeadPosition, false);
		}
	}
	public void Update()
	{
		if (!IsRobotConnected)
			return;
		if (RobotCommunicator == null)
			return;
		if (!_robotIfo.Connected)
			return;
		
		if (NullValues) {
			_robotIfo.HeadRotation=Quaternion.identity;
			_robotIfo.HeadPosition=Vector3.zero;
			_robotIfo.Speed=Vector2.zero;
			_robotIfo.Rotation=0;
		} else {
			HandleController ();
		}
		RobotCommunicator.SetData ("HeadRotation", _robotIfo.HeadRotation.ToExportString (), false);
		RobotCommunicator.SetData ("HeadPosition", _robotIfo.HeadPosition.ToExportString (), false);
		RobotCommunicator.SetData ("Speed", _robotIfo.Speed.ToExportString (), false);
		RobotCommunicator.SetData ("Rotation", _robotIfo.Rotation.ToString ("f6"), false);
	}

	public void SendData(string name,string value,bool statusData=false)
	{
		if (RobotCommunicator==null)
			return;
		RobotCommunicator.SetData (name,value,statusData);
	}

	public void Recalibrate()
	{
		if (BaseController != null) {
			BaseController.Recalibrate();
		}
		if (HeadController!=null) {
			HeadController.Recalibrate();
		}
	}
}



