using UnityEngine;
using System.Collections;

public class GstNetworkMultipleTexture : GstBaseTexture {
	
	public string TargetIP="127.0.0.1";
	public int TargetPort=7000;
	public int StreamsCount=1;
	
	private GstMultipleNetworkPlayer _player;

	
	public GstMultipleNetworkPlayer Player
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
		return StreamsCount;
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
		_player = new GstMultipleNetworkPlayer ();
	}
	
	
	protected override void _destroy ()
	{
		_player = null;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ConnectToHost(string ip,int port,int count)
	{
		TargetIP = ip;
		TargetPort = port;
		this.StreamsCount = count;
		if(_player.IsLoaded || _player.IsPlaying)
			_player.Close ();
		_player.SetIP (TargetIP, TargetPort,StreamsCount,false);
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
			for(int i=0;i<GetTextureCount();++i)
			{
				if (_player.GrabFrame (out sz,out components,i)) {
					Resize ((int)sz.x,(int) sz.y,components,i);
					OnFrameCaptured(i);
					if (m_Texture[i] == null)
						Debug.LogError ("The GstTexture does not have a texture assigned and will not paint.");
					else
						_player.BlitTexture (m_Texture[i].GetNativeTexturePtr (), m_Texture[i].width, m_Texture[i].height,i);
				}
			
			}
			break;	
		}
		}
	}
}
