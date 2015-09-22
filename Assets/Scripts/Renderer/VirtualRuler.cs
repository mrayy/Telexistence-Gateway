using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirtualRuler : MonoBehaviour {

	public Camera TargetCamera;
	public float Distance;
	public float Radius;
	public Material RenderMaterial;
	bool showText = true;

	float _lastRadius;

	public UILabel DistanceLabel;
	public UILabel SizeLabel;

	bool _Visible=false;

	public Vector2 _distancePos,_sizePos;

	Text _displayText;
	// Use this for initialization
	void Start () {
		MeshRenderer r= gameObject.AddComponent<MeshRenderer> ();
		MeshFilter mf= gameObject.AddComponent<MeshFilter> ();

		r.material = RenderMaterial;
		mf.mesh = MeshGenerator.GenerateTorus (0.05f,40, 0.05f, _lastRadius);
		transform.localRotation = Quaternion.Euler(90,0,0);

		SetVisible (false);
	}

	void SetVisible(bool v)
	{
		_Visible = v;
		gameObject.GetComponent<MeshRenderer> ().enabled = v;
		
		if (DistanceLabel)
			DistanceLabel.enabled = v;
		if (SizeLabel)
			SizeLabel.enabled = v;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos;

		if (_Visible) {
			transform.localPosition = new Vector3 (0, 0, Distance);

			pos = ProjectPoint (Vector3.forward * 10);
			DistanceLabel.text = "Distance:" + ToMetric (Distance);
			DistanceLabel.transform.localPosition = new Vector3 (pos.x, pos.y, 0);
		
			pos = ProjectPoint (new Vector3 (Radius, 0, Distance));
			SizeLabel.text = "Size:" + ToMetric (Radius);
			SizeLabel.transform.localPosition = new Vector3 (pos.x, pos.y, 0);

			if (Radius != _lastRadius) {
				MeshFilter mf = gameObject.GetComponent<MeshFilter> ();
				_lastRadius = Radius;
				MeshGenerator.ScaleTorus (mf.mesh, 0.1f, _lastRadius * 0.1f, _lastRadius, 40);
			}

			Distance += ((Input.GetKey (KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey (KeyCode.DownArrow) ? 1 : 0)) * Time.deltaTime;
			Radius += ((Input.GetKey (KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey (KeyCode.LeftArrow) ? 1 : 0)) * Time.deltaTime;

			Distance = Mathf.Max (0.0f, Distance);
			Radius = Mathf.Max (0.0f, Radius);
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			SetVisible(!_Visible);
		}
	}


	string ToMetric(float v)
	{
		float value = v;
		string unit = " m";
		if (v < 1) {
			value=v*100;
			unit=" cm";
		}else if (v > 1000) {
			value=v/1000.0f;
			unit=" km";
		}
		value = (int)(value * 100) * 0.01f;
		return value.ToString () + unit;
	}
	/*
	void DrawCross(Vector2 pos,float size)
	{
		_CreateMaterial ();
		pos.x /= Screen.width;
		pos.y /= Screen.height;
		float sx=size / Screen.width;
		float sy=size / Screen.height;
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho ();
		GL.Begin (GL.LINES);
		GL.Color (Color.red);
		GL.Vertex3 (pos.x - sx / 2, pos.y- sy / 2, 0);
		GL.Vertex3 (pos.x + sx / 2, pos.y+ sy / 2, 0);
		GL.End ();
		GL.Begin (GL.LINES);
		GL.Color (Color.red);
		GL.Vertex3 (pos.x + sx / 2, pos.y - sy / 2, 0);
		GL.Vertex3 (pos.x - sx / 2, pos.y + sy / 2, 0);
		GL.End ();
		GL.PopMatrix();
	}*/
	
	Rect _textArea = new Rect(0,0,Screen.width, Screen.height);

	Vector2 ProjectPoint(Vector3 pos)
	{
		Vector2 ret = TargetCamera.WorldToScreenPoint (TargetCamera.transform.position + TargetCamera.transform.rotation * pos);
		ret.x -= TargetCamera.pixelWidth / 2;
		ret.y -= TargetCamera.pixelHeight / 2;
		return ret;

	}
	Vector2 DrawString(string str,Vector3 pos,Color color,Vector2 Offset)
	{
		Camera c=TargetCamera;
		if (c == null)
			return Vector2.zero;

		Vector2 ret= ProjectPoint(pos);
		//DrawCross (_distancePos, 15);
		_textArea.position=ret+Offset;
		GUI.skin.GetStyle("Label").fontStyle=FontStyle.Bold;
		GUI.skin.GetStyle("Label").fontSize=20;
		GUI.skin.GetStyle("Label").normal.textColor=color;
		//textArea.position=_distancePos-textArea.size/2;
		
		_textArea.size= GUI.skin.GetStyle("Label").CalcSize(new GUIContent(str));
		GUI.Label(_textArea,str);
		return ret;
	}
	void DrawDistance()
	{
		string str="Distance:"+ToMetric(Distance);
		_distancePos= DrawString(str,Vector3.forward*10,Color.red,Vector2.zero);
	}

	void DrawSize()
	{
		string str="Size:"+ToMetric(Radius);
		_sizePos= DrawString(str,new Vector3(Radius,0,Distance),Color.green,new Vector2(0,15));
	}
	 void OnOVRGUI()
	{
	//	if (Camera.current != TargetCamera)
	//		return;
	//	_renderer.Begin (Camera.current.pixelRect.size);
		if(showText)
		{
			DrawDistance();
			DrawSize();
		}
		
	//	DrawCross (_distancePos, 15);
	//	DrawCross (_sizePos, 15);


	//	_renderer.End ();
	}
}
