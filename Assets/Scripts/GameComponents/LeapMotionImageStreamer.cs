using UnityEngine;
using System.Collections;

public class LeapMotionImageStreamer : MonoBehaviour {

	GstNetworkImageStreamer _streamer;
	GstUnityImageGrabber _imageGrabber;
	
	public LeapMotionRenderer HandRenderer;
	public RobotConnectionComponent RobotConnector;
	bool _isConnected;
	int _handsPort;
	// Use this for initialization
	void Start () {		
		RobotConnector.OnRobotConnected += OnRobotConnected;
		RobotConnector.OnRobotDisconnected += OnRobotDisconnected;
		_isConnected = false;
		_handsPort = 7000;
	}
	
	// Update is called once per frame
	void Update () {
		if (_isConnected && _streamer.IsStreaming) {
			_imageGrabber.Update();
		}
	}

	void OnRobotDisconnected()
	{
		_isConnected = false;
		_streamer = null;
	}
	void OnRobotConnected(RobotConnector.TargetPorts ports)
	{
		_imageGrabber = new GstUnityImageGrabber ();
		_imageGrabber.SetTexture2D (HandRenderer.LeapRetrival [0].MainTexture,TextureFormat.Alpha8);
		_imageGrabber.Update();//update once
		
		_handsPort=Settings.Instance.GetPortValue("HandsPort");

		_streamer = new GstNetworkImageStreamer ();
		_streamer.SetBitRate (500);
		_streamer.SetResolution (640, 240, 30);
		_streamer.SetGrabber (_imageGrabber);
		_streamer.SetIP (ports.RobotIP, _handsPort, false);
		RobotConnector.Connector.SendData("HandPorts",_handsPort.ToString(),false);
		
		_streamer.CreateStream ();
		_streamer.Stream ();
		_isConnected = true;
	}
}
