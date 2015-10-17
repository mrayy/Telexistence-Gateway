
using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class TelubeeOVRCamera : MonoBehaviour {

	public OVRCameraRig OculusCamera;
	public Material TargetMaterial;
	public int TargetFrameRate=80;

	public int leftCam=0;
	public int rightCam=1;
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

    ICameraSource _cameraSource;
	GstMultipleNetworkAudioPlayer _audioPlayer;

	TelubeeCameraRenderer[] _camRenderer=new TelubeeCameraRenderer[2];

	DebugCameraCaptureElement _cameraDebugElem;

	string _remoteHostIP;

	string _cameraProfile="";

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

	// Use this for initialization
	void Start () {

		Application.targetFrameRate = TargetFrameRate;
		if(OculusCamera==null)	//Try to find OVRCameraRig component
			OculusCamera = GameObject.FindObjectOfType<OVRCameraRig> ();


		if(TargetMaterial!=null)
            Init();


		RobotConnector.OnRobotConnected += OnRobotConnected;
		RobotConnector.Connector.DataCommunicator.OnCameraConfig += OnCameraConfig;
	}


    void Init()
    {
        if(CameraType==CameraSourceType.Local)
        {
            LocalWebcameraSource c;
			_cameraSource = (c=new LocalWebcameraSource());
            c.LeftInputCamera = leftCam;
            c.RightInputCamera = rightCam;
            c.Init();
		}else
		{
			MultipleNetworkCameraSource c;
			_cameraSource = (c=new MultipleNetworkCameraSource());
			c.StreamsCount=2;
			c.TargetNode=gameObject;
			c.Init();
		}

		if(AudioSupport)
			_audioPlayer = new GstMultipleNetworkAudioPlayer ();

		EyeName[] eyes = new EyeName[]{EyeName.RightEye,EyeName.LeftEye};
        if (OculusCamera != null)
        {
            Camera[] cams = new Camera[] { OculusCamera.rightEyeCamera, OculusCamera.leftEyeCamera };
		//	Vector2[] pixelShift = new Vector2[] { Configuration.CamSettings.PixelShiftRight,Configuration.CamSettings.PixelShiftLeft};
            for (int c = 0; c < cams.Length; ++c)
            {
				
				int i = (int)eyes[c];
				cams[i].backgroundColor=Color.black;

			//	CreateMesh ((EyeName)i);
                TelubeeCameraRenderer r = cams[i].gameObject.AddComponent<TelubeeCameraRenderer>();
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
				r._RenderPlane.transform.parent = cams[i].transform;
				r._RenderPlane.transform.localRotation=Quaternion.identity;
				r._RenderPlane.transform.localPosition=new Vector3(0,0,1);
            }
        }
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
	public void SetRemoteHost(string IP,RobotConnector.TargetPorts ports)
	{
		if (_cameraSource != null) {
			_cameraSource.Close();
			_cameraSource=null;
		}
		_remoteHostIP = IP;
		/*
		MultipleNetworkCameraSource c;
		_cameraSource = (c=new MultipleNetworkCameraSource());
		c.TargetNode=gameObject;
		c.Host = IP;
		c.port = port;
		c.Init();*/
		{
			
			MultipleNetworkCameraSource c;
			_cameraSource = (c = new MultipleNetworkCameraSource ());
			c.StreamsCount = 2;
			c.TargetNode = gameObject;
			c.Host = IP;
			c.port = ports.VideoPort;
			c.Init ();

			if(_audioPlayer!=null)
			{
				_audioPlayer.SetIP(IP,ports.AudioPort,false);
				_audioPlayer.CreateStream();
				_audioPlayer.Play();
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
		}
	}
}
