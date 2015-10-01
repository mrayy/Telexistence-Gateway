using UnityEngine;
using System.Collections;

public class TelubeeCameraRenderer : MonoBehaviour {
	
	public EyeName Eye;
	public ICameraSource CamSource;
	public Material Mat;
	public GameObject _RenderPlane;
	public TelubeeOVRCamera Src;
	public Camera DisplayCamera;
	//public Vector2 PixelShift;
	public float fovScaler;
	Texture _RenderedTexture;

	OffscreenProcessor _Processor=new OffscreenProcessor();
	// Use this for initialization
	void Start () {
		_Processor.ShaderName = "Image/I420ToRGB";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CreateMesh(EyeName eye )
	{
		Eye = eye;
		int i = (int)eye;
		if(_RenderPlane==null)
			_RenderPlane = new GameObject("EyesRenderPlane_"+eye.ToString());
		MeshFilter mf = _RenderPlane.GetComponent<MeshFilter> ();
		if (mf == null) {
			mf = _RenderPlane.AddComponent<MeshFilter> ();
		}
		MeshRenderer mr = _RenderPlane.GetComponent<MeshRenderer> ();
		if (mr == null) {
			mr = _RenderPlane.AddComponent<MeshRenderer> ();
		}
		
		mr.material = Mat;
		mf.mesh.vertices = new Vector3[]{
			new Vector3( 1,-1,0),
			new Vector3(-1,-1,0),
			new Vector3(-1, 1,0),
			new Vector3( 1, 1,0)
		};
		Rect r = CamSource.GetEyeTextureCoords (eye);
		Vector2[] uv = new Vector2[]{
			new Vector2(r.x,r.y),
			new Vector2(r.x+r.width,r.y),
			new Vector2(r.x+r.width,r.y+r.height),
			new Vector2(r.x,r.y+r.height),
		};
		Matrix4x4 rotMat = Matrix4x4.identity;
		if (Src.Configuration.CamSettings.Rotation [i] == CameraConfigurations.ECameraRotation.Flipped) {
			rotMat = Matrix4x4.TRS (Vector3.zero, Quaternion.Euler (0, 0, 180), Vector3.one);
		} else if (Src.Configuration.CamSettings.Rotation [i] == CameraConfigurations.ECameraRotation.CW) {
			rotMat = Matrix4x4.TRS (Vector3.zero, Quaternion.Euler (0, 0, 90), Vector3.one);
		} else if (Src.Configuration.CamSettings.Rotation [i] == CameraConfigurations.ECameraRotation.CCW) {
			rotMat = Matrix4x4.TRS (Vector3.zero, Quaternion.Euler (0, 0, -90), Vector3.one);
		}
		for(int v=0;v<4;++v)
		{
			Vector3 res=rotMat*(2*uv[v]-Vector2.one);
			uv[v]=(new Vector2(res.x,res.y)+Vector2.one)*0.5f;//Vector2.one-uv[v];
		}
		mf.mesh.uv = uv;
		mf.mesh.triangles = new int[]
		{
			0,2,1,0,3,2
		};
		transform.localPosition =new Vector3 (0, 0, 1);
		transform.localRotation =Quaternion.identity;
	}
	void OnPreRender()
	{
		_RenderedTexture = CamSource.GetEyeTexture (Eye);
		if(_RenderedTexture!=null && Mat!=null)
		{
			if(_RenderedTexture!=null && (_RenderedTexture as Texture2D)!=null && (_RenderedTexture as Texture2D).format==TextureFormat.Alpha8)
				_RenderedTexture=_Processor.ProcessTexture(_RenderedTexture);//CamTexture;//
			Mat.mainTexture=_RenderedTexture;

				
			Mat.SetVector("TextureSize",new Vector2(_RenderedTexture.width,_RenderedTexture.height));
			

		//	float fovScaler = 1;
			if(Src.Configuration!=null)
			{
				if(Eye==EyeName.LeftEye)
					Mat.SetVector("PixelShift",Src.Configuration.CamSettings.PixelShiftLeft);
				else 
					Mat.SetVector("PixelShift",Src.Configuration.CamSettings.PixelShiftRight);

				float fov=Src.Configuration.CamSettings.FoV;
				
				float focal = Src.Configuration.CamSettings.Focal;//1;//in meter
				float w1 = 2 * focal*Mathf.Tan(Mathf.Deg2Rad*(Camera.current.fieldOfView*0.5f));
				float w2 = 2 * (focal - Src.Configuration.CamSettings.CameraOffset)*Mathf.Tan(Mathf.Deg2Rad*fov*0.5f);

				if(w1==0)
					w1=1;
				float ratio = w2 / w1;
				
				fovScaler=ratio;

				
				//				Debug.Log("Configuration Updated");
				Mat.SetVector("FocalLength",Src.Configuration.CamSettings.FocalLength);
				Mat.SetVector("LensCenter",Src.Configuration.CamSettings.LensCenter);
				
				//	Vector4 WrapParams=new Vector4(Configuration.CamSettings.KPCoeff.x,Configuration.CamSettings.KPCoeff.y,
				//	                               Configuration.CamSettings.KPCoeff.z,Configuration.CamSettings.KPCoeff.w);
				Mat.SetVector("WrapParams",Src.Configuration.CamSettings.KPCoeff);
			}else
				Mat.SetVector("PixelShift",Vector2.zero);
				
			
			float aspect = (float)_RenderedTexture.width / (float)_RenderedTexture.height;
			if(aspect==0 || float.IsNaN(aspect))
				aspect=1;
			_RenderPlane.transform.localScale = new Vector3 (fovScaler, fovScaler/aspect, 1);

		}
	}
	void OnPostRender()
	{
		/*
		if (_RenderedTexture != null) {
			GL.PushMatrix();
			GL.LoadOrtho();
			Graphics.DrawTexture(new Rect(0,0,0.5f,0.5f),_RenderedTexture);
			GL.PopMatrix();
		}*/
	}
}
