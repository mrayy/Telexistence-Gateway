using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;	// For DllImport.
using System;

public class GstMultipleNetworkAudioPlayer:IGstPlayer  {
	
	
	internal const string DllName = "GStreamerUnityPlugin";
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private System.IntPtr mray_gst_createNetworkAudioPlayer();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_netAudioPlayerSetIP(System.IntPtr p, [MarshalAs(UnmanagedType.LPStr)]string ip, int audioPort, bool rtcp);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool mray_gst_netAudioPlayerCreateStream(System.IntPtr p);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_netAudioPlayerSetVolume(System.IntPtr p, float v);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private uint mray_gst_netAudioPlayerGetAudioPort(System.IntPtr p);

	
	public GstMultipleNetworkAudioPlayer()
	{
		m_Instance = mray_gst_createNetworkAudioPlayer();	
	}
	
	public uint GetAudioPort()
	{
		return mray_gst_netAudioPlayerGetAudioPort (m_Instance);
	}
	public override int GetCaptureRate (int index)
	{
		return 0;
	}
	
	public void SetIP(string ip,int audioPort,bool rtcp)
	{		
		mray_gst_netAudioPlayerSetIP (m_Instance, ip, audioPort, rtcp);
	}
	public bool CreateStream()
	{		
		return mray_gst_netAudioPlayerCreateStream (m_Instance);
	}

}







