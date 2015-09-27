using UnityEngine;
using System.Collections;
using System.IO;

public class RobotConnectionComponent : MonoBehaviour {

	RobotConnector _connector;

	public AppManager.HeadControllerType HeadControllerType;
	public AppManager.BaseControllerType BaseControllerType;

	//public TelubeeOVRCamera VideoStream;
	public DebugInterface Debugger;

	public bool NullValues=false;
	bool _connected=false;
	public int RobotIndex;

	RobotInfo _RobotIP;

	float[] _RobotJointValues;


	public delegate void Delg_OnRobotConnected(RobotConnector.TargetPorts ports);
	public event Delg_OnRobotConnected OnRobotConnected;
	
	public delegate void Delg_OnRobotDisconnected();
	public event Delg_OnRobotDisconnected OnRobotDisconnected;

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

		_connector.StartDataCommunicator (Settings.Instance.TargetPorts.CommPort);

		//Send Detect Message to scan network for the available robots
		_connector.RobotCommunicator.SetData ("detect", Settings.Instance.TargetPorts.CommPort.ToString(), false);
		_connector.RobotCommunicator.BroadcastMessage (Settings.Instance.TargetPorts.CommPort);

		if (Debugger != null) {
			Debugger.AddDebugElement(new DebugRobotStatus(this));
		}

		//VideoStream.SetConnectionComponent (this);


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
	}

	void OnRobotStatus(RobotDataCommunicator.ERobotControllerStatus status)
	{
		_robotStatus = status;
	}
	void OnRobotInfoDetected(RobotInfo ifo)
	{
		Debug.Log ("Robot detected: " + ifo.Name);
		_RobotIP = ifo;
	}
	void OnDestroy()
	{
		_connector.Dispose();
		_connector = null;
	}

	void Connect()
	{
		RobotInfo ifo=_RobotIP;
		if(ifo==null)
			ifo=AppManager.Instance.RobotManager.GetRobotInfo(RobotIndex);
		ConnectRobot(ifo);
	}
	// Update is called once per frame
	void Update () {
		_connector.NullValues = NullValues;
		_connector.Update ();

		if (Input.GetButtonDown ("ConnectRobot")) {
			if(IsConnected)
				DisconnectRobot();
			else
			{
				Connect();
			}
		}
		if (IsConnected && Input.GetButtonDown ("StartRobot")) {
			_connector.Recalibrate();
			if(!_connector.IsRobotConnected)
				StartUpdate();
			else
				EndUpdate();
		}
		if (Input.GetButtonDown ("CalibrateRobot"))  {
			_connector.Recalibrate();
		}

		if (IsConnected) {
			_connector.SendData("query","",false);
			_connector.SendData("jointVals","",false);
		}
	}


	public void ConnectRobot(RobotInfo r)
	{
		if (_connected) {
			if(r.IP==_RobotIP.IP)
				return;
			DisconnectRobot ();
		}
		_RobotIP = r;
		RobotConnector.TargetPorts ports = Settings.Instance.TargetPorts;
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

}
