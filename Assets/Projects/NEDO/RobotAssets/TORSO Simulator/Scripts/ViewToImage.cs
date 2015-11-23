using UnityEngine;
using System.Collections;

public class ViewToImage : MonoBehaviour {

	public Camera SourceCamera;
	public UITexture TargetTexture;

	public int Width,Height;
	RenderTexture _RT;

	// Use this for initialization
	void Start () {
		_RT = new RenderTexture (Width,Height,24);
	
		SourceCamera.clearFlags = CameraClearFlags.Color;
		SourceCamera.backgroundColor = new Color (1, 1, 1, 0);
		SourceCamera.targetTexture = _RT;
		TargetTexture.mainTexture = _RT;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
