using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;	// For DllImport.
using System;

public class GstMultipleNetworkPlayer:IGstPlayer  {
	
	
	internal const string DllName = "GStreamerUnityPlugin";
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private System.IntPtr mray_gst_createNetworkMultiplePlayer();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_multiNetPlayerSetIP(System.IntPtr p, [MarshalAs(UnmanagedType.LPStr)]string ip, int baseVideoPort,int count, bool rtcp);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool mray_gst_multiNetPlayerCreateStream(System.IntPtr p);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_multiNetPlayerGetFrameSize(System.IntPtr p, ref int w, ref int h, ref int components);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool mray_gst_multiNetPlayerGrabFrame(System.IntPtr p, ref int w, ref int h);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_multiNetPlayerBlitImage(System.IntPtr p, System.IntPtr _TextureNativePtr, int _UnityTextureWidth, int _UnityTextureHeight, int index);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private int mray_gst_multiNetPlayerFrameCount(System.IntPtr p);
	
	public Vector2 FrameSize
	{
		get
		{
			int w=0,h=0,comp=0;
			mray_gst_multiNetPlayerGetFrameSize(m_Instance,ref w,ref h,ref comp);
			return new Vector2(w,h);
		}
	}
	
	public GstMultipleNetworkPlayer()
	{
		m_Instance = mray_gst_createNetworkMultiplePlayer();	
	}
	
	
	public void SetIP(string ip,int baseVideoPort,int count,bool rtcp)
	{		
		mray_gst_multiNetPlayerSetIP (m_Instance, ip, baseVideoPort,count, rtcp);
	}
	public bool CreateStream()
	{		
		return mray_gst_multiNetPlayerCreateStream (m_Instance);
	}
	
	public bool GrabFrame(out Vector2 frameSize,out int comp)
	{
		int w=0,h=0,c=0;
		if(mray_gst_multiNetPlayerGrabFrame(m_Instance,ref w,ref h))
		{
			mray_gst_multiNetPlayerGetFrameSize(m_Instance,ref w,ref h,ref c);
			comp=c;
			frameSize.x=w;
			frameSize.y=h;
			return true;
		}
		comp = 3;
		frameSize.x = frameSize.y = 0;
		return false;
	}
	
	public void BlitTexture( System.IntPtr _NativeTexturePtr, int _TextureWidth, int _TextureHeight,int index )
	{
		if (_NativeTexturePtr == System.IntPtr.Zero) return;
		
		Vector2 sz = FrameSize;
		if (_TextureWidth != sz.x || _TextureHeight != sz.y) return;	// For now, only works if the texture has the exact same size as the webview.
		
		mray_gst_multiNetPlayerBlitImage(m_Instance, _NativeTexturePtr, _TextureWidth, _TextureHeight,index);	// We pass Unity's width and height values of the texture
	}
}







