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
