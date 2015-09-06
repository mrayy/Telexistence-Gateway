using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;	// For DllImport.
using System;

public class GstCustomPlayer:IGstPlayer  {
	
	
	internal const string DllName = "GStreamerUnityPlugin";
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private System.IntPtr mray_gst_createCustomVideoPlayer();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_customPlayerSetPipeline(System.IntPtr p, [MarshalAs(UnmanagedType.LPStr)]string pipeline);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool mray_gst_customPlayerCreateStream(System.IntPtr p);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_customPlayerGetFrameSize(System.IntPtr p, ref int w, ref int h, ref int comp);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool mray_gst_customPlayerGrabFrame(System.IntPtr p, ref int w, ref int h);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_customPlayerBlitImage(System.IntPtr p, System.IntPtr _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight);
	
	
	public Vector2 FrameSize
	{
		get
		{
			int w=0,h=0,comp=0;
			mray_gst_customPlayerGetFrameSize(m_Instance,ref w,ref h,ref comp);
			return new Vector2(w,h);
		}
	}
	
	public GstCustomPlayer()
	{
		m_Instance = mray_gst_createCustomVideoPlayer();	
	}
	
	
	public void SetPipeline(string pipeline)
	{		
		mray_gst_customPlayerSetPipeline (m_Instance, pipeline);
	}
	public bool CreateStream()
	{		
		return mray_gst_customPlayerCreateStream (m_Instance);
	}
	
	public bool GrabFrame(out Vector2 frameSize,out int components)
	{
		int w=0,h=0,c=0;
		if(mray_gst_customPlayerGrabFrame(m_Instance,ref w,ref h))
		{
			mray_gst_customPlayerGetFrameSize(m_Instance,ref w,ref h,ref c);
			components=c;
			frameSize.x=w;
			frameSize.y=h;
			return true;
		}
		components=3;
		frameSize.x = frameSize.y = 0;
		return false;
	}
	
	public void BlitTexture( System.IntPtr _NativeTexturePtr, int _TextureWidth, int _TextureHeight )
	{
		if (_NativeTexturePtr == System.IntPtr.Zero) return;
		
		Vector2 sz = FrameSize;
		if (_TextureWidth != sz.x || _TextureHeight != sz.y) return;	// For now, only works if the texture has the exact same size as the webview.
		
		mray_gst_customPlayerBlitImage(m_Instance, _NativeTexturePtr, _TextureWidth, _TextureHeight);	// We pass Unity's width and height values of the texture
	}
}
