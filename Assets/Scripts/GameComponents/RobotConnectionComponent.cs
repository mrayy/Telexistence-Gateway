using UnityEngine;
using System.Collections;
using System.IO;

public class RobotConnectionComponent : MonoBehaviour {

	RobotConnector _connector;

	public AppManager.HeadControllerType HeadControllerType;
	public AppManager.BaseControllerType BaseControllerType;

	public TelubeeOVRCamera VideoStream;
	public DebugInterface Debugger;

	public bool NullValues=false;
	bool _connected=false;
	public int RobotIndex;

	RobotInfo _RobotIP;
	
	public bool IsConnected
	{
		get
		{
			return _connected;
		}
	}
	
	public RobotConnector Connector
	{
		get{
			return _connector;
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
		_connector.StartDataCommunicator (Settings.Instance.TargetPorts.CommPort);
		_connector.RobotCommunicator.SetData ("detect", Settings.Instance.TargetPorts.CommPort.ToString(), false);
		_connector.RobotCommunicator.BroadcastMessage (Settings.Instance.TargetPorts.CommPort);

		if (Debugger != null) {
			Debugger.AddDebugElement(new DebugHeadPose(_connector));
		}

	}
	void OnMessage(int message,BinaryReader reader)
	{
	//	Debug.Log ("Message Arrived: " + message.ToString());
		if (message == (int)RobotDataCommunicator.Messages.RobotStatus) {
			int status= reader.ReadInt32();
			
		//	Debug.Log ("Robot Status: " + ((RobotDataCommunicator.ERobotControllerStatus)status).ToString());
		}
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
	// Update is called once per frame
	void Update () {
		_connector.NullValues = NullValues;
		_connector.Update ();

		if (Input.GetKeyDown (KeyCode.C)) {
			if(IsConnected)
				DisconnectRobot();
			else
			{
				RobotInfo ifo=_RobotIP;
				if(ifo==null)
					ifo=AppManager.Instance.RobotManager.GetRobotInfo(RobotIndex);
				ConnectRobot(ifo);
			}
		}
		if (IsConnected && Input.GetKeyDown (KeyCode.Space)) {
			_connector.Recalibrate();
			if(!_connector.IsRobotConnected)
				StartUpdate();
			else
				EndUpdate();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			_connector.Recalibrate();
		}

		if (IsConnected) {
			_connector.SendData("query","",false);
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

		if (VideoStream != null) {
			VideoStream.SetRemoteHost(_RobotIP.IP,ports.VideoPort);
		}

		LogSystem.Instance.Log ("Connecting to Robot:" + _RobotIP.Name, LogSystem.LogType.Info);
	}

	public void DisconnectRobot()
	{
		if (!_connected)
			return;
		_connector.DisconnectRobot ();
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
