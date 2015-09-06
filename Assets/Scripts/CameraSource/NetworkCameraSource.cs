using UnityEngine;
using System.Collections;

public class NetworkCameraSource : ICameraSource {
	
	private GstNetworkTexture m_Texture = null;
	public GameObject TargetNode;
	public string Host = "127.0.0.1";
	public int port=7000;
	public bool isStereo = false;

	public void Init()
	{
		m_Texture= TargetNode.AddComponent<GstNetworkTexture> ();
		m_Texture.Initialize ();
		
		m_Texture.ConnectToHost (Host, port);
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
		return m_Texture.PlayerTexture[0];
	}
	
	public Rect GetEyeTextureCoords(EyeName e)
	{
		if (isStereo) {
			if (e == EyeName.LeftEye)
				return new Rect (0, 0, 0.5f, 1);
			return new Rect (0.5f, 0, 0.5f, 1);
		}else
			return new Rect (0, 0, 1, 1);
	}
	public Vector2 GetEyeScalingFactor(EyeName e)
	{
		if (isStereo) 
			return new Vector2 (0.5f, 1);
		return Vector2.one;
	}
}
