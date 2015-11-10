using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class PLCDriverObject : MonoBehaviour {


	PLCDriver _driver;

	public string IP;
	public int Port;

	public int SleepInterval = 100;
	public Text DebugText;

	class JobHandler:ThreadJob
	{
		PLCDriverObject _o;
		public JobHandler(PLCDriverObject o)
		{
			_o=o;
		}
		protected override void ThreadFunction() 
		{
			while (!IsDone) {
				_o._RunJob();

			}
		}
		
		protected override void OnFinished() 
		{
		}
	}

	JobHandler _jobHandler;

	void _RunJob()
	{
		_driver.ReadData ();
		_driver.WriteData ();

		Thread.Sleep (SleepInterval);
	}

	string GenerateDebugString()
	{
		string str = "";

		str+="Connected: " + _driver.IsConnected+ "\n";
		str+="Camera FPS: " + _driver.GetTorsoUInt(PLCDriver.ETorsoDataField.CameraFPS)+ "\n";
		str+="J1: " + _driver.GetTorsoUInt(PLCDriver.ETorsoDataField.J1RT)+ "\n";
		str+="J2: " + _driver.GetTorsoUInt(PLCDriver.ETorsoDataField.J2RT)+ "\n";
		str+="J3: " + _driver.GetTorsoUInt(PLCDriver.ETorsoDataField.J3RT)+ "\n";
		str+="J4: " + _driver.GetTorsoUInt(PLCDriver.ETorsoDataField.J4RT)+ "\n";
		
		str+="Battery Level: " + _driver.GetXyrisUInt(PLCDriver.EXyrisDataField.battVoltage)+ "\n";
		return str;
	}

	// Use this for initialization
	void Start () {
		_driver = new PLCDriver ();
		
		IP=Settings.Instance.GetValue ("PLC", "IP",IP);
		int.TryParse (Settings.Instance.GetValue ("PLC", "Port",Port.ToString()), out Port);

		_driver.Connect (IP, Port);

		_jobHandler = new JobHandler (this);
		_jobHandler.Start ();


	}
	 void OnDestroy()
	{
		_jobHandler.Abort ();
		_driver.Destroy ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (DebugText != null) {
			DebugText.text=GenerateDebugString();
		}
	}

	public void OnJointValues(float[] values)
	{
		for (int i=0; i<6; ++i) {
			_driver.SetTorsoUInt ((PLCDriver.ETorsoDataField)(((int)PLCDriver.ETorsoDataField.J1IK)+i), (uint)values [2*i+0]);
			_driver.SetTorsoUInt ((PLCDriver.ETorsoDataField)(((int)PLCDriver.ETorsoDataField.J1RT)+i), (uint)values [2*i+1]);
		}
	}

	public float[] GetJointValues()
	{
		float[] ret = new float[6];
		for (int i=0; i<6; ++i) {
			ret[i]=(float)_driver.GetTorsoUInt ((PLCDriver.ETorsoDataField)(((int)PLCDriver.ETorsoDataField.J1RT)+i));
		}
		return ret;
	}
	public void OnRobotStatus(RobotDataCommunicator.ERobotControllerStatus status)
	{
		_driver.SetTorsoUInt(PLCDriver.ETorsoDataField.Status,(uint)status);
	}
	public void OnCameraFPS(int c0,int c1)
	{
		_driver.SetTorsoUShort (PLCDriver.ETorsoDataField.CameraFPS,(ushort) ((c0 + c1) / 2));
	}

	public float GetBatteryLevel()
	{
		return _driver.GetXyrisUInt(PLCDriver.EXyrisDataField.battVoltage);
	}
	public float GetSpeed()
	{
		return _driver.GetXyrisUInt(PLCDriver.EXyrisDataField.traverseSpeed);
	}
	public float GetWifiLevel()
	{
		return 0;
	}
}
