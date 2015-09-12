using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VirtualRuler : ICameraPostRenderer {
	
	public class VirtualRulerImpl : VRGUI {
		
		public VirtualRuler Owner;
		public override void OnVRGUI()
		{
			Owner.OnOVRGUI ();
		}
	}
	public Camera TargetCamera;
	public float Distance;
	public float Radius;
	VirtualRulerImpl _impl;

	public Material mat;
	Vector3[] vertices;
	int[] indicies ;

	Vector2 _distancePos,_sizePos;

	Text _displayText;
	UIRenderer _renderer = new UIRenderer ();
	// Use this for initialization
	void Start () {

		vertices = new Vector3[]{
			new Vector3(-0.5f,-0.5f,0),
			new Vector3( 0.5f,-0.5f,0),
			new Vector3( 0.5f, 0.5f,0),
			new Vector3(-0.5f, 0.5f,0)
		};
		indicies = new int[]{
			0,1,2,3,0};

		_impl = gameObject.AddComponent<VirtualRulerImpl> ();
		_impl.Owner = this;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localRotation = Quaternion.identity;
		//transform.localPosition = new Vector3 (0, 0, Distance);
	}

	void _CreateMaterial()
	{
		
		if (!mat) { 
			/* Credit:  */
			mat = new Material("Shader \"Lines/Colored Blended\" {" +
			                   "SubShader { Pass {" +
			                   "   BindChannels { Bind \"Color\",color }" +
			                   "   Blend SrcAlpha OneMinusSrcAlpha" +
			                   "   ZTest Off ZWrite Off Cull Off Fog { Mode Off }" +
			                   "} } }");
			mat.hideFlags = HideFlags.HideAndDontSave;
			mat.shader.hideFlags = HideFlags.HideAndDontSave;
			
		}

	}
	public override void OnPostRender()
	{
		_CreateMaterial ();
		GL.PushMatrix ();
		GL.LoadOrtho ();
		mat.SetPass (0);
		GL.Color (Color.white);
		if(_renderer.ResultTexture!=null)
			Graphics.DrawTexture (Camera.current.pixelRect, _impl.guiRenderTexture);
		GL.PopMatrix ();
		
	}
	bool showText = true;

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
	}
	
	Rect _textArea = new Rect(0,0,Screen.width, Screen.height);
	Vector2 DrawString(string str,Vector3 pos,Color color,Vector2 Offset)
	{
		Camera c=Camera.current;
		if (c == null)
			return Vector2.zero;
		Vector2 ret= c.WorldToScreenPoint(c.transform.position+c.transform.rotation*pos);
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
	public override void OnOVRGUI()
	{
	//	if (Camera.current != TargetCamera)
	//		return;
	//	_renderer.Begin (Camera.current.pixelRect.size);
		if(showText)
		{
			DrawDistance();
			DrawSize();
		}
		
		DrawCross (_distancePos, 15);
		DrawCross (_sizePos, 15);
	//	_renderer.End ();
	}
}
