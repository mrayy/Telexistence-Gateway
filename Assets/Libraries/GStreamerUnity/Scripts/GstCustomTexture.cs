using UnityEngine;
using System.Collections;

public class GstCustomTexture : GstBaseTexture {
	public string Pipeline="";
	
	private GstCustomPlayer _player;

	
	public GstCustomPlayer Player
	{
		get	
		{
			return _player;
		}
		set
		{
			if (value != null)
				_player = value;
		}
	}
	
	public override int GetTextureCount ()
	{
		return 1;
	}
	/*public override int GetCaptureRate (int index)
	{
		return _player.GetCaptureRate (index);
	}*/
	
	public override IGstPlayer GetPlayer(){
		return _player;
	}
	
	protected override void _initialize()
	{
		_player = new GstCustomPlayer ();
	}
	protected override void _destroy ()
	{
		_player = null;
	}

	public void SetPipeline(string p)
	{
		Pipeline = p;
		if(_player.IsLoaded || _player.IsPlaying)
			_player.Close ();
		_player.SetPipeline (Pipeline);
		_player.CreateStream();
	}
	

	
	void OnGUI()
	{
		// This function should do input injection (if enabled), and drawing.
		if (_player == null)
			return;
		
		Event e = Event.current;
		
		switch (e.type) {
		case EventType.Repaint:	
		{
			Vector2 sz;
			int components;
			if (_player.GrabFrame (out sz,out components)) {
				Resize ((int)sz.x,(int) sz.y,components,0);
				OnFrameCaptured(0);
				if (m_Texture == null)
					Debug.LogError ("The GstTexture does not have a texture assigned and will not paint.");
				else
					_player.BlitTexture (m_Texture[0].GetNativeTexturePtr (), m_Texture[0].width, m_Texture[0].height);
			}
			break;	
		}
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
