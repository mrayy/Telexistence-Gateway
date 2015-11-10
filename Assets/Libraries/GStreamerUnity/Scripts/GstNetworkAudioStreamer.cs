using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class GstNetworkAudioStreamer:IGstStreamer {
	
	internal const string DllName = "GStreamerUnityPlugin";
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private System.IntPtr mray_gst_createAudioNetworkStreamer();
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_audioStreamerSetIP(System.IntPtr p, [MarshalAs(UnmanagedType.LPStr)]string ip, int videoPort, bool rtcp);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool mray_gst_audioStreamerCreateStream(System.IntPtr p);

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void mray_gst_audioStreamerSetChannels(System.IntPtr p,int c);

	GstUnityImageGrabber _grabber;

	public GstNetworkAudioStreamer()
	{
		m_Instance = mray_gst_createAudioNetworkStreamer();	
	}
	
	
	public void SetIP(string ip,int videoPort,bool rtcp)
	{		
		mray_gst_audioStreamerSetIP (m_Instance, ip, videoPort, rtcp);
	}
	public bool CreateStream()
	{		
		return mray_gst_audioStreamerCreateStream (m_Instance);
	}

	public void SetChannels(int c)
	{
		mray_gst_audioStreamerSetChannels (m_Instance, c);
	}

}
