
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GNSSObject : MonoBehaviour
{
	private int iIndex = 0;
	private bool zoomChange;
	private int  zoomLevel;
	private float scaleDelta;
	private string lineParameter;
	public float  characterScale = 20;
	Manager mg;

	PLCDriverObject SourceObject;

	public double[] location;

	public Text debugLabel;
	
	// Use this for initialization
	void Start ()
	{
		Zoom.OnChangeZoom += OnChangeZoom;	
		mg = GameObject.Find ("Manager").GetComponent<Manager> ();
		scaleDelta = transform.localScale.x * characterScale;
		zoomLevel = mg.sy_Map.zoom;
		float scalefactor = Mathf.Pow (2f, mg.sy_Map.zoom - 18);
		
		transform.localScale = new Vector3 (scalefactor * scaleDelta, scalefactor * scaleDelta, scalefactor * scaleDelta);
		
		SourceObject = GameObject.FindObjectOfType<PLCDriverObject> ();
	}
	
	double[] oldPos;
	
	void reCalCoordinate ()
	{
		
		double[] invers_pos = new double[]{location [1], location [0]};
		transform.localPosition = mg.GIStoPos (invers_pos);

		zoomChange = false;
		
		float scalefactor = Mathf.Pow (2f, mg.sy_Map.zoom - 18);
		transform.localScale = new Vector3 (scalefactor * scaleDelta, scalefactor * scaleDelta, scalefactor * scaleDelta);

	}
	
	void OnChangeZoom ()
	{
		
		reCalCoordinate ();
	}
	
	void Update ()
	{	

		Vector2 _loc = SourceObject.GetGPSLocation ();
		
		location [0] = _loc.x;
		location [1] = _loc.y;

		double[] invers_pos = new double[]{location [1], location [0]};
		transform.localPosition = mg.GIStoPos (invers_pos);

		if (debugLabel != null)
			debugLabel.text = "Vehicle Location: " + location [0].ToString ("0.00000") + "," + location [1].ToString ("0.00000");
	}
	
}
