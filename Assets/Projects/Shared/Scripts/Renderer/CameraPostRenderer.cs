using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraPostRenderer : MonoBehaviour {

	public ICameraPostRenderer[] Renderers; 

	List<ICameraPostRenderer> _renderers=new List<ICameraPostRenderer>();
	// Use this for initialization
	void Start () {
		if(_renderers==null)
			_renderers=new List<ICameraPostRenderer>();

		if (Renderers != null) {
			foreach (var r in Renderers) {
				_renderers.Add (r);
			}
		}
	}

	public void AddRenderer(ICameraPostRenderer r)
	{
		_renderers.Add (r);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnPreRender()
	{
		foreach (ICameraPostRenderer r in _renderers)
			r.OnPreRender ();
	}
	void OnPostRender()
	{
		foreach (ICameraPostRenderer r in _renderers)
			r.OnPostRender ();
	}
}
