using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Xml;
using System.Threading;
using System.Text;

public class RemoteRobotCommunicator : IRobotCommunicator,IDisposable {


	class CommunicationThread:ThreadJob
	{
		RemoteRobotCommunicator owner;
		public CommunicationThread(RemoteRobotCommunicator o)
		{
			owner=o;
		}
		protected override void ThreadFunction() 
		{
			while (!this.IsDone) 
			{
				if(owner._connected || owner.Broadcast)
				{
					owner.Update(0);
				}
				if(owner._userInfo.RobotConnected && owner._connected)
				{
					Thread.Sleep(10);
				}else
				{
					Thread.Sleep(100);
				}
			}
		}
		
		protected override void OnFinished() { 
		}
	}


	struct UserInfo
	{
		public string UserName;
		public bool UserConnected;
		public bool RobotConnected;
		public string RobotAddr;
		public string RobotLocation;
	};

	struct DataInfo
	{
		public string value;
		public bool statusData;
	};
	
	bool _connected=false;
	UdpClient _client;
	CommunicationThread _thread;
	IPEndPoint _addr = new IPEndPoint ( IPAddress.Loopback,0);
	UserInfo _userInfo=new UserInfo();

	Dictionary<string,DataInfo> _values = new Dictionary<string, DataInfo> ();
	string _outputValues="";

	bool _broadCast=false;

	public bool Broadcast
	{
		set{
			_broadCast = value;
		}
		get{
			return _broadCast;
		}
	}

	void _SendUpdate()
	{
		_UpdateData ();
		Update (0);
	}

	void _UpdateData()
	{
		lock(_values)
		{
			using (var sw = new StringWriter()) {
				using (var xw = XmlWriter.Create(sw)) {
					// Build Xml with xw.
					xw.WriteStartElement("RobotData");
					//xw.WriteAttributeString("Connected",_userInfo.RobotConnected.ToString());

					foreach(var v in _values)
					{
						xw.WriteStartElement("Data");
						xw.WriteAttributeString("N",v.Key);
						xw.WriteAttributeString("V",v.Value.value);
						xw.WriteEndElement();
					}

					xw.WriteEndElement();
				}
				_outputValues= sw.ToString();
			}
		}
	}
	void _CleanData(bool statusValues)
	{
		lock (_values) {
			if (statusValues)
				_values.Clear ();
			else {
				List<string> keys=new List<string>();
				foreach (var v in _values) {
					if (!v.Value.statusData) {
						keys.Add(v.Key);
					}
				}
				foreach(var v in keys)
				{
					_values.Remove(v);
				}
			}
		}
		_UpdateData ();
	}

	public RemoteRobotCommunicator()
	{
		_thread = new CommunicationThread (this);
		_thread.Start ();
	}

	public  void Dispose()
	{
		if (_client != null) 
		{
			_client.Close();
			_client=null;
		}

		if (_thread != null) {
			_thread.Abort();
		}
	}
	public override void SetBroadcastNext(bool set){
		Broadcast = set;
	}
	public override void BroadcastMessage(int port)
	{
		byte[] d;
		_UpdateData ();
		lock(_values)
		{
			d=Encoding.UTF8.GetBytes(_outputValues);
			ClearData (false);
		}
		if (d.Length == 0)
			return;
		bool removeClient = false;
		if (_client == null) {
			_client = new UdpClient ();
			removeClient=true;
		}
		_client.Send(d,d.Length,new IPEndPoint(IPAddress.Broadcast,port));
		if (removeClient) {
			_client.Close ();
			_client = null;
		}
	}
	public override bool Connect (string ip, int port)
	{
		if (_connected)
			Disconnect ();
		_addr.Address = IPAddress.Parse (ip);
		_addr.Port = port;

		try
		{
			_client = new UdpClient ();
			_client.Connect (_addr);
		}catch(Exception e)
		{
			LogSystem.Instance.Log("RemoteRobotCommunicator::Connect() - "+e.Message,LogSystem.LogType.Error);
		}

		_connected = true;
		return true;
	}
	public override void Disconnect()
	{
		if (!_connected)
			return;
		_connected = false;
		_client.Close ();
		_client = null;
	}
	public override bool IsConnected()
	{
		return _connected;
	}
	
	public override void SetUserID(string userID)
	{
		_userInfo.UserName = userID;
	}
	public override void ConnectUser(bool c)
	{
		_userInfo.UserConnected = c;
		_SendUpdate ();
	}
	public override void ConnectRobot(bool c)
	{
		_userInfo.RobotConnected = c;
		lock (_values) {
			SetData ("RobotConnect", c.ToString (), true);
			_SendUpdate ();
		}
	}

	public override string GetData(string key)
	{
		string v="";
		lock(_values)
		{
			if (_values.ContainsKey (key))
				v=_values [key].value;
		}
		return v;
	}
	
	public override void SetData(string key, string value, bool statusData) 
	{
		DataInfo di = new DataInfo ();
		di.statusData = statusData;
		di.value = value;
		lock(_values)
		{
			if(!_values.ContainsKey(key))
				_values.Add(key,di);
			else _values[key]=di;
			_UpdateData();
		}
	}
	public override void RemoveData(string key) 
	{
		lock(_values)
		{
			_values.Remove (key);
			_UpdateData ();
		}
	}
	public override void ClearData(bool statusValues)
	{
		_CleanData (statusValues);
	}
	
	public override void Update(float dt)
	{
		if (!_connected && !Broadcast)
			return;

		byte[] d;
		lock(_values)
		{
			d=Encoding.UTF8.GetBytes(_outputValues);
			ClearData (false);
		}
		if (d.Length == 0)
			return;
		try
		{
			if(Broadcast)
			{
				Broadcast=false;//only broadcast once
				_client.Send(d,d.Length,new IPEndPoint(IPAddress.Broadcast,_addr.Port));
			}else
			{
				_client.Send (d, d.Length);//, _addr
			}
		}catch(Exception e)
		{
			LogSystem.Instance.Log("RemoteRobotCommunicator::Update() - "+e.Message,LogSystem.LogType.Warning);
		}
	}

}
