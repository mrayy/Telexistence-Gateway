using UnityEngine;
using System.Collections;

public class MultipleNetworkCameraSource : ICameraSource {
	
	private GstNetworkMultipleTexture m_Texture = null;
	public GameObject TargetNode;
	public string Host = "127.0.0.1";
	public int port=7000;
	public int StreamsCount=1;
	public GstBaseTexture GetBaseTexture()
	{
		return m_Texture;
	}
	public void Init()
	{
		m_Texture= TargetNode.AddComponent<GstNetworkMultipleTexture> ();
		m_Texture.StreamsCount = StreamsCount;
		m_Texture.Initialize ();
		
		m_Texture.ConnectToHost (Host, port,StreamsCount);
		m_Texture.Play ();
	}
	public void Close()
	{
		if (m_Texture != null) {
			m_Texture.Close();
		}
	}
	public Texture GetEyeTexture(EyeName e)
	{
		if (m_Texture != null && m_Texture.PlayerTexture!=null) {
			return m_Texture.PlayerTexture [(int)e];
		}
		return null;
	}
	
	public Rect GetEyeTextureCoords(EyeName e)
	{
		return new Rect (0, 0, 1, 1);
	}
	public Vector2 GetEyeScalingFactor(EyeName e)
	{
		return Vector2.one;
	}
}
