
using UnityEngine;
using System.Collections;

public class TelubeeOVRCamera : MonoBehaviour {

	OVRCameraRig OculusCamera;
	public Material TargetMaterial;
	public int TargetFrameRate=80;
    public enum CameraSourceType
    {
        Local,
        Remote
    }
	public DisplayConfigurations Display;
    public CameraSourceType CameraType;

	public TELUBeeConfiguration Configuration;

	public DebugInterface Debugger;

	GameObject[] _RenderPlane=new GameObject[2];
    ICameraSource _cameraSource;

	TelubeeCameraRenderer[] _camRenderer=new TelubeeCameraRenderer[2];

	DebugCameraCaptureElement _cameraDebugElem;

	string _remoteHostIP;

	public string RemoteHostIP
	{
		get{
			return _remoteHostIP;
		}
	}

	// Use this for initialization
	void Start () {

		Application.targetFrameRate = TargetFrameRate;

        OculusCamera = gameObject.GetComponent<OVRCameraRig>();

		if(TargetMaterial!=null)
            Init();


	}


    void Init()
    {
        if(CameraType==CameraSourceType.Local)
        {
            LocalWebcameraSource c;
			_cameraSource = (c=new LocalWebcameraSource());
            c.LeftInputCamera = 0;
            c.RightInputCamera = 1;
            c.Init();
		}else
		{
			MultipleNetworkCameraSource c;
			_cameraSource = (c=new MultipleNetworkCameraSource());
			c.StreamsCount=2;
			c.TargetNode=gameObject;
			c.Init();
		}


		EyeName[] eyes = new EyeName[]{EyeName.LeftEye,EyeName.RightEye};
        if (OculusCamera != null)
        {
            Camera[] cams = new Camera[] { OculusCamera.rightEyeCamera, OculusCamera.leftEyeCamera };
            for (int i = 0; i < cams.Length; ++i)
            {
				cams[i].backgroundColor=Color.black;
				
				CreateMesh (eyes[i]);
                TelubeeCameraRenderer r = cams[i].gameObject.AddComponent<TelubeeCameraRenderer>();
                r.Mat = TargetMaterial;
                r._RenderPlane = _RenderPlane[i];
				r.DisplayCamera=cams[i];
				r.Eye = eyes[i];
				r.Src = this;

				_camRenderer[i]=r;

				r.CamSource = _cameraSource;
                if (i == 0)
                {
					_RenderPlane[i].layer=LayerMask.NameToLayer("RightEye");
                }
                else
                {
					_RenderPlane[i].layer=LayerMask.NameToLayer("LeftEye");
				}
				_RenderPlane[i].transform.parent = cams[i].transform;
				_RenderPlane[i].transform.localRotation=Quaternion.identity;
				_RenderPlane[i].transform.localPosition=new Vector3(0,0,1);
            }
        }
    }
	void CreateMesh(EyeName eye )
	{
		int i = (int)eye;
		_RenderPlane[i] = new GameObject("EyesRenderPlane_"+eye.ToString());
		MeshFilter mf = _RenderPlane[i].AddComponent<MeshFilter> ();
		MeshRenderer mr = _RenderPlane[i].AddComponent<MeshRenderer> ();

		mr.material = TargetMaterial;
		mf.mesh.vertices = new Vector3[]{
			new Vector3( 1,-1,0),
			new Vector3(-1,-1,0),
			new Vector3(-1, 1,0),
			new Vector3( 1, 1,0)
		};
		Rect r = _cameraSource.GetEyeTextureCoords (eye);
		Vector2[] uv = new Vector2[]{
			new Vector2(r.x,r.y),
			new Vector2(r.x+r.width,r.y),
			new Vector2(r.x+r.width,r.y+r.height),
			new Vector2(r.x,r.y+r.height),
		};
		if (Configuration.CamSettings.Flipped) {
			for(int v=0;v<4;++v)
			{
				uv[v]=Vector2.one-uv[v];
			}
		}
		mf.mesh.uv = uv;
		mf.mesh.triangles = new int[]
		{
			0,2,1,0,3,2
		};
		_RenderPlane[i].transform.localPosition =new Vector3 (0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		GStreamerCore.Time = Time.time;

		//if(_configurationDirty)
		{
			//_configurationDirty=false;
			if(Configuration!=null && TargetMaterial!=null)
			{
//				Debug.Log("Configuration Updated");
				TargetMaterial.SetVector("FocalLength",Configuration.CamSettings.FocalLength);
				TargetMaterial.SetVector("LensCenter",Configuration.CamSettings.LensCenter);

				Vector4 WrapParams=new Vector4(Configuration.CamSettings.K1,Configuration.CamSettings.K2,
				                               Configuration.CamSettings.P1,Configuration.CamSettings.P2);
				TargetMaterial.SetVector("WrapParams",WrapParams);
			}
		}
	}

	public void SetRemoteHost(string IP,int port)
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
			c.port = port;
			c.Init ();
			for(int i=0;i<2;++i)
				_camRenderer[i].CamSource=_cameraSource;
			if (Debugger) {
				Debugger.RemoveDebugElement(_cameraDebugElem);;
				_cameraDebugElem=new DebugCameraCaptureElement(_cameraSource.GetBaseTexture());
				Debugger.AddDebugElement(_cameraDebugElem);;
			}
		}

	}
}
