using UnityEngine;
using System.Collections;

public class GUIRendererPlane : MonoBehaviour {

	public Camera TargetCamera;
	public Material TargetMaterial;
	public Transform OculusCenter;
	public KeyCode EnableCode;
	RenderTexture _RT;
	GameObject _renderPlane;
	// Use this for initialization
	void Start () {

		if (TargetMaterial == null) {
			TargetMaterial = new Material ("");
			TargetMaterial.shader=Shader.Find("Unlit/Transparent Coloured RTT");
		}

		_RT = new RenderTexture (2048,2048,24);
		TargetMaterial.mainTexture = _RT;
		TargetCamera.targetTexture = _RT;
		TargetCamera.clearFlags = CameraClearFlags.Color;
		TargetCamera.backgroundColor = new Color (0, 0, 0, 0);

		GameObject renderPlane = new GameObject ("UIRenderer");
		_renderPlane = renderPlane;
		renderPlane.AddComponent<MeshRenderer> ();
		MeshFilter f = renderPlane.AddComponent<MeshFilter> ();
		
		{
			f.mesh.vertices = new Vector3[]{
				new Vector3 (1, -1, 0),
				new Vector3 (-1, -1, 0),
				new Vector3 (-1, 1, 0),
				new Vector3 (1, 1, 0)
			};
			
			Rect r = new Rect (-1, -1, 1, 1);
			Vector2[] uv = new Vector2[]{
				new Vector2 (r.x, r.y),
				new Vector2 (r.x + r.width, r.y),
				new Vector2 (r.x + r.width, r.y + r.height),
				new Vector2 (r.x, r.y + r.height),
			};
			f.mesh.uv = uv;
			f.mesh.triangles = new int[]
			{
				0,2,1,0,3,2
			};
		}
		renderPlane.GetComponent<MeshRenderer> ().material = TargetMaterial;

		renderPlane.transform.parent= OculusCenter;

		renderPlane.transform.localPosition = new Vector3 (0, 0, 1);
		renderPlane.transform.localRotation = Quaternion.identity;

	}
	
	void _Resize(int w,int h)
	{
		if (_RT == null)
			_RT = new RenderTexture (w,h, 24,RenderTextureFormat.Default);
		
		if (_RT.width != w || _RT.height != h) {
			_RT = new RenderTexture (w,h, 24,RenderTextureFormat.Default);
		}
		//guiRenderPlane.GetComponent<MeshRenderer>().material.mainTexture = guiRenderTexture;
	}
	// Update is called once per frame
	void Update () {
	//	_Resize ((int)TargetCamera.pixelWidth,(int)TargetCamera.pixelHeight);
		if (Input.GetKeyDown (EnableCode)) {
			_renderPlane.GetComponent<MeshRenderer>().enabled=!_renderPlane.GetComponent<MeshRenderer>().enabled;
		}
	}
}
