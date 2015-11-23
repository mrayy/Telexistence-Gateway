using UnityEngine;
using System.Collections;

public class OffscreenProcessor  {

	Material ProcessingMaterial;
	public Texture ResultTexture{
		get{
			return _RenderTexture;
		}
	}
	RenderTexture _RenderTexture;

	public string ShaderName
	{
		set{
			ProcessingShader=Shader.Find(value);
		}
	}
	public Shader ProcessingShader
	{
		set{
			ProcessingMaterial.shader=value;
		}
		get{
			return ProcessingMaterial.shader;
		}
	}

	public OffscreenProcessor()
	{
		ProcessingMaterial = new Material (Shader.Find("Diffuse"));
	}
	void _Setup(Texture InputTexture)
	{
		int width = InputTexture.width;
		int height = InputTexture.height;
		if (((Texture2D)InputTexture).format == TextureFormat.Alpha8)
			height =(int)( height / 1.5f);
		if (_RenderTexture == null) {
			_RenderTexture = new RenderTexture (width, height,1, RenderTextureFormat.ARGB32);
		} else if (	_RenderTexture.width != width || 
		           _RenderTexture.height != height) 
		{
			_RenderTexture = new RenderTexture (width, height,1, RenderTextureFormat.ARGB32);
		}
	}
	public Texture ProcessTexture(Texture InputTexture)
	{
		if (InputTexture.width == 0 || InputTexture.height == 0)
			return InputTexture;
		_Setup (InputTexture);
		ProcessingMaterial.mainTexture = InputTexture;
		Graphics.Blit (InputTexture,_RenderTexture, ProcessingMaterial,0);
		return _RenderTexture;

	}

}
