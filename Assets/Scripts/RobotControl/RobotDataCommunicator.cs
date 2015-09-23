using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System;

public class RobotDataCommunicator {
	
	
	public enum Messages
	{
		DepthData = 1,
		DepthSize = 2,
		IsStereo = 3,
		CameraConfig = 4,
		CalibrationDone = 5,
		ReportMessage = 6,
		IRSensorMessage = 7,
		BumpSensorMessage = 8,
		BatteryLevel = 9,
		ClockSync = 10,
		ReinitializeRobot = 11,
		RobotStatus=12,
		JointValues=13,
		Detect = 14,
		Presence = 15
	}
	public enum ERobotControllerStatus
	{
		EStopped,
		EIniting,
		EStopping,
		EDisconnected,
		EConnected,
		EConnecting,
		EDisconnecting
	};


	class DataThread:ThreadJob
	{
		RobotDataCommunicator owner;
		public DataThread(RobotDataCommunicator o)
		{
			owner=o;
		}
		protected override void ThreadFunction() 
		{
			while (!this.IsDone) {
				if(owner._Process()!=0){
					this.IsDone=true;
				}
			}
		}
		
		protected override void OnFinished() 
		{
		}
	}

	UdpClient _client;
	DataThread _thread;


	public delegate void Delg_OnCameraConfig(string cameraProfile);
	public delegate void Delg_OnRobotCalibrateDone();
	public delegate void Delg_OnReportMessage(int code,string msg);
	public delegate void Delg_OnBumpSensor(float[] v);
	public delegate void Delg_OnIRSensor(float[] v);
	public delegate void Delg_OnJointValues(float[] v);
	public delegate void Delg_OnBatteryLevel(int level);
	public delegate void Delg_OnMessage(int message,BinaryReader reader);
	public delegate void Delg_OnRobotInfoDetected(RobotInfo ifo);
	public delegate void Delg_OnRobotStatus(ERobotControllerStatus status);


	public event Delg_OnCameraConfig OnCameraConfig;
	public event Delg_OnRobotCalibrateDone OnRobotCalibrateDone;
	public event Delg_OnReportMessage OnReportMessage;
	public event Delg_OnBumpSensor OnBumpSensor;
	public event Delg_OnIRSensor OnIRSensor;
	public event Delg_OnMessage OnMessage;
	public event Delg_OnBatteryLevel OnBatteryLevel;
	public event Delg_OnRobotInfoDetected OnRobotInfoDetected;
	public event Delg_OnRobotStatus OnRobotStatus;
	public event Delg_OnJointValues OnJointValues;

	public RobotDataCommunicator()
	{
	}

	public void Start(int port)
	{
		try
		{
			_client = new UdpClient (port);
		}catch(Exception e)
		{

		}
		_thread = new DataThread (this);
		_thread.Start ();
	}

	public void Stop()
	{
		if(_client!=null)
			_client.Close ();
		if(_thread!=null)
			_thread.Abort ();

		_thread = null;
		_client = null;
	}

	int _Process()
	{
		if (_client == null)
			return -1;
		IPEndPoint ep=null;
		byte[] data;
		try
		{
			data=_client.Receive (ref ep);
		}catch{
			return 0;
		}
		if (data == null || data.Length == 0)
			return 0;
		var reader = new BinaryReader (new MemoryStream (data));
		int msg = reader.ReadInt32 ();
		switch (msg) {
		case (int)Messages.Presence:
		{
			RobotInfo ifo=new RobotInfo();
			ifo.Read(reader);
			if(OnRobotInfoDetected!=null)
				OnRobotInfoDetected(ifo);
		}break;
		case (int)Messages.DepthData:
			break;
		case (int)Messages.DepthSize:
			break;
		case (int)Messages.IsStereo:
			break;
		case (int)Messages.CameraConfig:
			if(OnCameraConfig!=null)
				OnCameraConfig(reader.ReadStringNative());
			break;
		case (int)Messages.ReportMessage:
			if(OnReportMessage!=null)
			{
				int code=reader.ReadInt32();
				string str=reader.ReadStringNative();
				OnReportMessage(code,str);
			}
			break;
		case (int)Messages.BumpSensorMessage:
			if(OnBumpSensor!=null)
			{
				int count=reader.ReadInt32();
				float[] values=new float[count];
				for(int i=0;i<count;++i)
				{
					values[i]=reader.ReadSingle();
				}
				OnBumpSensor(values);
			}
			break;
		case (int)Messages.IRSensorMessage:
			if(OnIRSensor!=null)
			{
				int count=reader.ReadInt32();
				float[] values=new float[count];
				for(int i=0;i<count;++i)
				{
					values[i]=reader.ReadSingle();
				}
				OnIRSensor(values);
			}
			break;
		case (int)Messages.BatteryLevel:
			if(OnBatteryLevel!=null)
				OnBatteryLevel(reader.ReadInt32());
			break;
		case (int)Messages.ClockSync:
			break;
		case (int)Messages.JointValues:
			if(OnJointValues!=null)
			{
				int count=reader.ReadInt32();
				float[] values=new float[count];
				for(int i=0;i<count;++i)
				{
					values[i]=reader.ReadSingle();
				}
				OnJointValues(values);
			}
			break;
		case (int)Messages.RobotStatus:
			if(OnRobotStatus!=null)
			{
				int val=reader.ReadInt32();
				OnRobotStatus((ERobotControllerStatus)val);
			}
			break;

		default:
			if(OnMessage!=null)
				OnMessage(msg,reader);
			break;
		}
		return 0;
	}
}
