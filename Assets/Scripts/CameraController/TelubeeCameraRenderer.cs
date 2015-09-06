using UnityEngine;
using System.Collections;

public class TelubeeCameraRenderer : MonoBehaviour {
	
	public EyeName Eye;
	public Texture CamTexture;
	public Material Mat;
	public GameObject _RenderPlane;
	public TelubeeOVRCamera Src;
	public Camera DisplayCamera;

	public float fovScaler;

	OffscreenProcessor _Processor=new OffscreenProcessor();
	// Use this for initialization
	void Start () {
		_Processor.ShaderName = "Image/RedProcessor";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPreRender()
	{
		if(CamTexture!=null && Mat!=null)
		{
			Mat.mainTexture=_Processor.ProcessTexture(CamTexture);
		
		//	float fovScaler = 1;
			if(Src.Configuration!=null)
			{
				float fov=Src.Configuration.CamSettings.FoV;
				
				float focal = 1;//in meter
				float w1 = 2 * focal*Mathf.Tan(Mathf.Deg2Rad*(DisplayCamera.fieldOfView*0.5f));
				float w2 = 2 * (focal - Src.Configuration.CamSettings.CameraOffset)*Mathf.Tan(Mathf.Deg2Rad*fov*0.5f);

				if(w1==0)
					w1=1;
				float ratio = w2 / w1;
				
				fovScaler=ratio;
			}
			
			float aspect = (float)CamTexture.width / (float)CamTexture.height;
			if(aspect==0 || float.IsNaN(aspect))
				aspect=1;
			_RenderPlane.transform.localScale = new Vector3 (fovScaler, fovScaler/aspect, 1);

		}
	}
}
