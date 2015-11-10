using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class PLCDriver  {
	
	public enum ETorsoDataField
	{
		J1RT,
		J2RT,
		J3RT,
		J4RT,
		J5RT,
		J6RT,
		J1IK,
		J2IK,
		J3IK,
		J4IK,
		J5IK,
		J6IK,
		J1TQ,
		J2TQ,
		J3TQ,
		J4TQ,
		J5TQ,
		J6TQ,
		IMURoll,
		IMUPitch,
		IMUYaw,
		Status,
		CameraFPS,
		Collision
	};
	
	
	public enum EXyrisDataField
	{
		terminal,
		offline,
		inOperation,
		traverse,
		subcraler,
		offroad,
		mainCrwlMtrRight,
		mainCrwlMtrLeft,
		mainCrwlRightRot,
		mainCrwlLeftRot,
		subCrwlPrevAngRight,
		subCrwlPrevAngLeft,
		subCrwlCurrAngRight,
		subCrwlCurrAngLeft,
		frontAngle,
		sideAngle,
		rightTraverse,
		leftTraverse,
		battVoltage,
		battCurrent,
		inForwardDir,
		inTraverseControl,
		maxSpeed,
		traverseSpeed,
		maxTempLeft,
		maxTempRight,
		unused0,
		HealthCheckCount,
	};

	
	internal const string DllName = "PLCUnityPlugin";
	
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private IntPtr CreatePLCDriver(  );
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool PLCDriverConnect(IntPtr driver, [MarshalAs(UnmanagedType.LPStr)]string ip, int port );
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool PLCDriverIsConnected(IntPtr driver );
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private bool PLCDriverDisconnect(IntPtr driver);
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void PLCDriverDestroy(IntPtr driver);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void PLCDriverRead(IntPtr driver);
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void PLCDriverWrite(IntPtr driver);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void PLCSetTorsoUInt(IntPtr driver,int data,uint v);
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void PLCSetTorsoUShort(IntPtr driver,int data,ushort v);

	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private uint PLCGetTorsoUInt(IntPtr driver,int data);
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private ushort PLCGetTorsoUShort(IntPtr driver,int data);
	
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void PLCSetXyrisUInt(IntPtr driver,int data,uint v);
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private void PLCSetXyrisUShort(IntPtr driver,int data,ushort v);
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private uint PLCGetXyrisUInt(IntPtr driver,int data);
	[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
	extern static private ushort PLCGetXyrisUShort(IntPtr driver,int data);


	IntPtr _instance;

	public bool IsConnected {
		get {
			return PLCDriverIsConnected (_instance);
		}
	}

	public System.IntPtr NativePtr
	{
		get
		{
			return _instance;
		}
	}


	public PLCDriver()
	{
		_instance = CreatePLCDriver ();
	}
	
	public void Destroy()
	{
		PLCDriverDestroy (_instance);
	}
	public bool Connect(string ip,int port)
	{
		return PLCDriverConnect (_instance,ip, port);
	}
	public void Disconnect()
	{
		PLCDriverDisconnect (_instance);
	}
	
	public void SetTorsoUInt(ETorsoDataField data,uint v)
	{
		PLCSetTorsoUInt (_instance, (int)data,v);
	}
	public void SetTorsoUShort(ETorsoDataField data,ushort v)
	{
		PLCSetTorsoUShort (_instance, (int)data,v);
	}
	public uint GetTorsoUInt(ETorsoDataField data)
	{
		return PLCGetTorsoUInt (_instance, (int)data);
	}
	public uint GetTorsoUShort(ETorsoDataField data)
	{
		return PLCGetTorsoUShort (_instance, (int)data);
	}


	public uint GetXyrisUInt(EXyrisDataField data)
	{
		return PLCGetXyrisUInt (_instance, (int)data);
	}
	public ushort GetXyrisUShort(EXyrisDataField data)
	{
		return PLCGetXyrisUShort (_instance, (int)data);
	}

	public void ReadData()
	{
		PLCDriverRead (_instance);
	}
	public void WriteData()
	{
		PLCDriverWrite (_instance);
	}
}
