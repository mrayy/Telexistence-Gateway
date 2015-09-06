using UnityEngine;
using System.Collections;

public abstract class GstBaseTexture : MonoBehaviour {
	
	public int m_Width = 64;
	public int m_Height = 64;
	public bool m_FlipX = false;
	public bool m_FlipY = false;

	
	[SerializeField]	// Only for testing purposes.
	protected Texture2D[] m_Texture = null;
	
	
	[SerializeField]
	protected bool m_InitializeOnStart = true;
	protected bool m_HasBeenInitialized = false;

	public abstract int GetTextureCount ();

	protected abstract void _initialize ();
	public abstract IGstPlayer GetPlayer();
	
	public Texture2D[] PlayerTexture
	{
		get
		{
			return m_Texture;	
		}
	}

	public bool IsLoaded {
		get {
			return GetPlayer().IsLoaded;
		}
	}
	
	public bool IsPlaying {
		get {
			return GetPlayer().IsPlaying;
		}
	}


	public void Initialize()
	{
		m_HasBeenInitialized = true;
		
		GStreamerCore.Ref();
		_initialize ();
		
		// Call resize which will create a texture and a webview for us if they do not exist yet at this point.
		Resize(m_Width, m_Height,3);
		
		if (GetComponent<GUITexture>())
		{
			GetComponent<GUITexture>().texture = m_Texture[0];
		}
		else if (GetComponent<Renderer>() && GetComponent<Renderer>().material)
		{		
			GetComponent<Renderer>().material.mainTexture = m_Texture[0];
			GetComponent<Renderer>().material.mainTextureScale = new Vector2(	Mathf.Abs(GetComponent<Renderer>().material.mainTextureScale.x) * (m_FlipX ? -1.0f : 1.0f),
			                                                                 Mathf.Abs(GetComponent<Renderer>().material.mainTextureScale.y) * (m_FlipY ? -1.0f : 1.0f));
		}
		else
		{
			Debug.LogWarning("There is no Renderer or guiTexture attached to this GameObject! GstTexture will render to a texture but it will not be visible.");
		}
		
	}

	protected virtual void _destroy() 
	{
	}
	public void Destroy()
	{
		if (GetPlayer() != null)
		{
			GetPlayer().Destroy ();
			GStreamerCore.Unref();
		}
		_destroy ();
	}
	
	void OnApplicationQuit()
	{
		Destroy ();
	}
	
	public void Play()
	{
		GetPlayer().Play ();
	}
	
	public void Pause()
	{
		GetPlayer().Pause ();
	}
	
	public void Stop()
	{
		GetPlayer().Stop ();
	}
	
	public void Close()
	{
		GetPlayer().Close ();
	}
	void OnDestroy()
	{
		Destroy ();
	}
	TextureFormat GetFormat(int components)
	{
		if (components == 1)
			return TextureFormat.Alpha8;
		if (components == 3)
			return TextureFormat.RGB24;
		return TextureFormat.RGB24;
	}
	public void Resize( int _Width, int _Height,int components )
	{
		m_Width = _Width;
		m_Height = _Height;

		if (GetTextureCount () == 0)
			return;
		if (m_Texture == null)
		{
			m_Texture=new Texture2D[GetTextureCount()];
			for(int i=0;i<GetTextureCount();++i)
			{
				m_Texture[i] = new Texture2D(m_Width, m_Height, GetFormat(components), false);
				m_Texture[i].filterMode = FilterMode.Point;
			}
		}
		else
		{	
			for(int i=0;i<GetTextureCount();++i)
			{
				m_Texture[i].Resize(m_Width, m_Height, GetFormat(components), false);
				m_Texture[i].Apply(false, false);
			}
		}
		
	}

	// Use this for initialization
	void Start () {

		if (m_InitializeOnStart && !m_HasBeenInitialized) 
		{
			Initialize ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
