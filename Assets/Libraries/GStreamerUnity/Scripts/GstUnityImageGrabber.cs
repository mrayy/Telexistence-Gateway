using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class GstUnityImageGrabber {
	
	enum EPixelFormat
	{
		EPixel_Unkown,
		
		EPixel_LUMINANCE8,
		EPixel_LUMINANCE16,
		
		EPixel_Alpha8,
		EPixel_Alpha4Luminance4,
		EPixel_Alpha8Luminance8,
		
		EPixel_R5G6B5,
		EPixel_R8G8B8,
		EPixel_R8G8B8A8,
		EPixel_X8R8G8B8,
		
		EPixel_B8G8R8,
		EPixel_B8G8R8A8,
		EPixel_X8B8G8R8,
		
		EPixel_Float16_R,
		EPixel_Float16_RGB,
		EPixel_Float16_RGBA,
		EPixel_Float16_GR,
		
		EPixel_Float32_R,
		EPixel_Float32_RGB,
		EPixel_Float32_RGBA,
		EPixel_Float32_GR,
		
		EPixel_Depth,
		EPixel_Stecil,
		
		
		EPixel_Short_RGBA,
		EPixel_Short_RGB,
		EPixel_Short_GR,
		
		EPixel_DXT1,
		EPixel_DXT2,
		EPixel_DXT3,
		EPixel_DXT4,
		EPixel_DXT5,
		
		EPixel_I420,
		
		EPixelFormat_Count
	};

	internal const string DllName = "GStreamerUnityPlugin";
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private System.IntPtr mray_gst_createUnityImageGrabber();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_UnityImageGrabberSetData(System.IntPtr p, System.IntPtr _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight,int Format);

	Texture2D m_Texture;
	protected Color32[] m_Pixels;
	protected GCHandle m_PixelsHandle;
	EPixelFormat m_format;

	protected System.IntPtr m_Instance;

	public System.IntPtr Instance
	{
		get{
			return m_Instance;
		}
	}

	public GstUnityImageGrabber()
	{
		m_Instance = mray_gst_createUnityImageGrabber();	
	}
	public void SetTexture2D(Texture2D texture,TextureFormat format)
	{
		m_Texture = texture;
		if (m_Texture == null) {
			return;
		}
		switch (format) {
		case TextureFormat.ARGB32:
		case TextureFormat.RGBA32:
			m_format=EPixelFormat.EPixel_R8G8B8A8;
			break;
		case TextureFormat.Alpha8:
			m_format=EPixelFormat.EPixel_Alpha8;
			break;
		case TextureFormat.RGB24:
			m_format=EPixelFormat.EPixel_R8G8B8;
			break;
		}
	}
	public void SetTexture2D(Texture2D texture)
	{
		SetTexture2D (m_Texture, m_Texture.format);
	}

	public void Update()
	{
		if (m_Texture == null)
			return;
		m_Pixels = m_Texture.GetPixels32 (0);
		m_PixelsHandle = GCHandle.Alloc(m_Pixels, GCHandleType.Pinned);
		mray_gst_UnityImageGrabberSetData (m_Instance, m_PixelsHandle.AddrOfPinnedObject (), m_Texture.width, m_Texture.height, (int)m_format);
	}
}
