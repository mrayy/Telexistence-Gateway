
using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class TelubeeOVRCamera : MonoBehaviour,IDependencyNode  {

	public OVRCameraRig OculusCamera;
	public Material TargetMaterial;
	public int TargetFrameRate=80;
    public enum CameraSourceType
    {
        Local,
        Remote
    }
	public DisplayConfigurations Display;
    public CameraSourceType CameraType;

	public RobotConnectionComponent RobotConnector;

	public TELUBeeConfiguration Configuration;
	public bool AudioSupport=false;

//	RobotConnectionComponent _Connection;

	public DebugInterface Debugger;

	public TelubeeCameraRenderer TargetEyeLeft;
	public TelubeeCameraRenderer TargetEyeRight;

	NetValueObject _videoValues;

	public VideoParametersController ParameterController;

    ICameraSource _cameraSource;
	GstMultipleNetworkAudioPlayer _audioPlayer;
	GstNetworkAudioStreamer _audioStreamer;

	TelubeeCameraRenderer[] _camRenderer=new TelubeeCameraRenderer[2];

	DebugCameraCaptureElement _cameraDebugElem;

	uint[] _videoPorts;
	uint _audioPort=0;

	string _remoteHostIP;

	string _cameraProfile="";
	int m_grabbedFrames=0;

	public int GrabbedFrames
	{
		get{
			return m_grabbedFrames;
		}
	}

	public string RemoteHostIP
	{
		get{
			return _remoteHostIP;
		}
	}

	public TelubeeCameraRenderer[] CamRenderer {
		get {
			return _camRenderer;
		}
	}
	public  void OnDependencyStart(DependencyRoot root)
	{
		if (root == RobotConnector) {
			RobotConnector.OnRobotConnected += OnRobotConnected;
			RobotConnector.OnRobotDisconnected+=OnRobotDisconnected;
			RobotConnector.Connector.DataCommunicator.OnCameraConfig += OnCameraConfig;
			RobotConnector.OnServiceNetValue+=OnServiceNetValue;
		}
	}
	// Use this for initialization
	void Start () {

		Application.targetFrameRate = TargetFrameRate;
		if(OculusCamera==null)	//Try to find OVRCameraRig component
			OculusCamera = GameObject.FindObjectOfType<OVRCameraRig> ();

        if (Configuration == null)
            Configuration = gameObject.AddComponent<TELUBeeConfiguration>();

		if(TargetMaterial!=null)
            Init();

		RobotConnector.AddDependencyNode (this);

		GStreamerCore.Ref ();
	}

	void OnDestroy()
	{
		//GStreamerCore.Unref ();
		if(_videoValues!=null)
			_videoValues.Dispose ();
	}

	public void ApplyMaterial(Material m)
	{
		TargetMaterial = m;
		if(_camRenderer [0]!=null)
			_camRenderer [0].ApplyMaterial (m);
		
		if(_camRenderer [1]!=null)
			_camRenderer [1].ApplyMaterial (m);
	}
	
	public void OnServiceNetValue(string serviceName,int port)
	{
		if (serviceName == "AVStreamServiceModule") {
			_videoValues.Connect(RobotConnector.RobotIP.IP,port);
		}
	}
	void OnFrameGrabbed(Texture2D texture,int index)
	{
	//	Debug.Log ("Frame Grabbed: "+index);
		m_grabbedFrames++;
		if (m_grabbedFrames > 10) {
			_camRenderer[0].Enable();
			_camRenderer[1].Enable();
		}

		if (RobotConnector != null) {
			RobotConnector.OnCameraFPS(_cameraSource.GetBaseTexture().GetCaptureRate(0),_cameraSource.GetBaseTexture().GetCaptureRate(1));
		}
	}

    void Init()
    {
		CameraType = Settings.Instance.RobotSettings.ReadValue ("Camera", "Source", CameraType.ToString ())=="Local"?CameraSourceType.Local:CameraSourceType.Remote;
        if(CameraType==CameraSourceType.Local)
        {
            LocalWebcameraSource c;
			_cameraSource = (c=new LocalWebcameraSource());
			c.LeftInputCamera = Settings.Instance.RobotSettings.ReadValue ("Camera", "Left", 0);
			c.RightInputCamera = Settings.Instance.RobotSettings.ReadValue ("Camera", "Right", 1);
            c.Init();
		}else
		{
			MultipleNetworkCameraSource c;
			_cameraSource = (c=new MultipleNetworkCameraSource());
			c.StreamsCount=2;
			c.TargetNode=gameObject;
			c.Init();
		}

		if (AudioSupport) {
			_audioPlayer = new GstMultipleNetworkAudioPlayer ();
			_audioStreamer = gameObject.AddComponent<GstNetworkAudioStreamer> ();
			_audioStreamer.SetChannels(1);
			_audioStreamer.CreateStream();
		}

		EyeName[] eyes = new EyeName[]{EyeName.RightEye,EyeName.LeftEye};
		TelubeeCameraRenderer[] Targets = new TelubeeCameraRenderer[]{TargetEyeRight,TargetEyeLeft};
        if (OculusCamera != null)
        {
            Camera[] cams = new Camera[] { OculusCamera.rightEyeCamera, OculusCamera.leftEyeCamera };
		//	Vector2[] pixelShift = new Vector2[] { Configuration.CamSettings.PixelShiftRight,Configuration.CamSettings.PixelShiftLeft};
            for (int c = 0; c < cams.Length; ++c)
            {
				int i = (int)eyes[c];
				cams[i].backgroundColor=Color.black;

			//	CreateMesh ((EyeName)i);
				TelubeeCameraRenderer r = Targets[i];
				if(r==null)
					r=cams[i].gameObject.AddComponent<TelubeeCameraRenderer>();
				r.Mat = Object.Instantiate(TargetMaterial);
				r.DisplayCamera=cams[i];
				r.Src = this;
				r.CamSource = _cameraSource;

				r.CreateMesh(eyes[c]);

				_camRenderer[i]=r;

                if (i == 0)
                {
					r._RenderPlane.layer=LayerMask.NameToLayer("RightEye");
                }
                else
                {
					r._RenderPlane.layer=LayerMask.NameToLayer("LeftEye");
				}
				if(Targets[i]==null)
				{
					r._RenderPlane.transform.parent = cams[i].transform;
					r._RenderPlane.transform.localRotation=Quaternion.identity;
					r._RenderPlane.transform.localPosition=new Vector3(0,0,1);
				}
            }
        }

		
		_videoValues=new NetValueObject();
		if (ParameterController != null)
			ParameterController.TargetValueObject = _videoValues;
    }
	
	// Update is called once per frame
	void Update () {
		GStreamerCore.Time = Time.time;


		if (_cameraProfile != "") {
			
			XmlReader reader = XmlReader.Create (new StringReader (_cameraProfile));
			while (reader.Read()) {
				if(reader.NodeType==XmlNodeType.Element)
				{
					Configuration.CamSettings.LoadXML (reader);
					_camRenderer[0].CreateMesh(EyeName.LeftEye);
					_camRenderer[1].CreateMesh(EyeName.RightEye);
					break;
				}
			}
			_cameraProfile="";
		}
	}
	/*
	public void SetConnectionComponent(RobotConnectionComponent connection)
	{
		_Connection = connection;
		_Connection.Connector.DataCommunicator.OnCameraConfig += OnCameraConfig;
	}*/
	void OnCameraConfig(string cameraProfile)
	{
		_cameraProfile = cameraProfile;

		//Debug.Log (cameraProfile);
	}

	void OnRobotConnected(RobotConnector.TargetPorts ports)
	{
		SetRemoteHost (ports.RobotIP, ports);
	}
	void OnRobotDisconnected()
	{
		_camRenderer [0].Disable ();
		_camRenderer [1].Disable ();

		if (_audioStreamer != null) {
			_audioStreamer.Stop();
		}
	}
	public void SetRemoteHost(string IP,RobotConnector.TargetPorts ports)
	{
		_remoteHostIP = IP;
		/*
		MultipleNetworkCameraSource c;
		_cameraSource = (c=new MultipleNetworkCameraSource());
		c.TargetNode=gameObject;
		c.Host = IP;
		c.port = port;
		c.Init();*/
		if(this.CameraType==CameraSourceType.Remote)
		{
			
			if (_cameraSource != null) {
				_cameraSource.Close();
				_cameraSource=null;
			}
			MultipleNetworkCameraSource c;
			_cameraSource = (c = new MultipleNetworkCameraSource ());
			c.StreamsCount = 2;
			c.TargetNode = gameObject;
			c.Host = IP;
			c.port = Settings.Instance.GetPortValue("VideoPort",0);
			c.Init ();
			
			_cameraSource.GetBaseTexture ().OnFrameGrabbed += OnFrameGrabbed;
			m_grabbedFrames=0;

			//disable the renderes until we video stream starts
			//_camRenderer[0].Disable();
			//_camRenderer[1].Disable();

			_videoPorts=new uint[2]{0,0};
			_videoPorts[0]=c.Texture.Player.GetVideoPort(0);
			_videoPorts[1]=c.Texture.Player.GetVideoPort(1);
			RobotConnector.Connector.SendData("VideoPorts",_videoPorts[0].ToString()+","+_videoPorts[1].ToString(),true);

			if(_audioPlayer!=null)
			{
				int port= Settings.Instance.GetPortValue("AudioPort",0);
				_audioPlayer.SetIP(IP,0,false);
				_audioPlayer.CreateStream();
				_audioPlayer.Play();
				_audioPort=_audioPlayer.GetAudioPort();
				RobotConnector.Connector.SendData("AudioPort",_audioPort.ToString(),true);
			}

			if(_audioStreamer!=null)
			{
				_audioStreamer.SetIP(IP,7010,false);//port should be dynamically assigned from remote side
				_audioStreamer.Stream();
			}
			for(int i=0;i<2;++i)
				_camRenderer[i].CamSource=_cameraSource;
			if (Debugger) {
				Debugger.RemoveDebugElement(_cameraDebugElem);;
				_cameraDebugElem=new DebugCameraCaptureElement(_cameraSource.GetBaseTexture());
				Debugger.AddDebugElement(_cameraDebugElem);;
			}
		}

		{
			//request camera settings
			RobotConnector.Connector.SendData("CameraParameters","",false);
			
			//request netvalue port
			RobotConnector.Connector.RobotCommunicator.SetData ("NetValuePort", "AVStreamServiceModule,"+RobotConnector.Connector.DataCommunicator.Port.ToString(), false,false);
		}
	}
}
