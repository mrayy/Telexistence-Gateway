using UnityEngine;
using System.Collections;

public class CameraPostRenderer : MonoBehaviour {

	public ICameraPostRenderer[] Renderers; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPostRender()
	{
		foreach (ICameraPostRenderer r in Renderers)
			r.OnPostRender ();
	}
}
