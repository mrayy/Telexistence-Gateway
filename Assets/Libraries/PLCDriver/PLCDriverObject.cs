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

	public class XyrisJointValues
	{
		public float FrontRightRail;
		public float BackRightRail;
		public float FrontLefttRail;
		public float BackLeftRail;
		public float LeftRail;
		public float RightRail;
		public float YBMJoint;
		public float YBMRod;
		public float YBMBaseRoll;


		public float LeftCrawlerRPM;
		public float RightCrawlerRPM;

		public int XyrisOperation;
		public int XyrisTraverse;
		
		public int YBMRodStarted;
		public int YBMEmgStop;
	}

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
		//_driver.WriteData ();

		Thread.Sleep (SleepInterval);
	}

	string GenerateDebugString()
	{
		string str = "";

		str+="Connected: " + _driver.IsConnected+ "\n";
		str+="J1: " + _driver.GetTorsoInt(PLCDriver.ETorsoDataField.J1_rt_angle)+ "\n";
		str+="J2: " + _driver.GetTorsoInt(PLCDriver.ETorsoDataField.J2_rt_angle)+ "\n";
		str+="J3: " + _driver.GetTorsoInt(PLCDriver.ETorsoDataField.J3_rt_disp)+ "\n";
		str+="J4: " + _driver.GetTorsoInt(PLCDriver.ETorsoDataField.J4_rt_angle)+ "\n";
		str+="J5: " + _driver.GetTorsoInt(PLCDriver.ETorsoDataField.J5_rt_angle)+ "\n";
		str+="J6: " + _driver.GetTorsoInt(PLCDriver.ETorsoDataField.J6_rt_angle)+ "\n";
		
		str+="Battery Level: " + _driver.GetXyrisUInt(PLCDriver.EXyrisDataField.battVoltage)+ "\n";
		return str;
	}

	// Use this for initialization
	void Start () {
		_driver = new PLCDriver ();
		
		IP=Settings.Instance.GetValue ("PLC", "IP",IP);
		//int.TryParse (Settings.Instance.GetValue ("PLC", "Port",Port.ToString()), out Port);

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

	public void OnTorsoJointValues(float[] values)
	{
		for (int i=0; i<6; ++i) {
			_driver.SetTorsoUInt ((PLCDriver.ETorsoDataField)(((int)PLCDriver.ETorsoDataField.J1_ik_angle)+i), (uint)(values [2*i+0]*100));
			_driver.SetTorsoUInt ((PLCDriver.ETorsoDataField)(((int)PLCDriver.ETorsoDataField.J1_rt_angle)+i), (uint)(values [2*i+1]*100));
		}
	}

	public float[] GetTorsoJointValues()
	{
		float[] ret = new float[6];
		for (int i=0; i<6; ++i) {
			ret[i]=(float)_driver.GetTorsoInt ((PLCDriver.ETorsoDataField)(((int)PLCDriver.ETorsoDataField.J1_rt_angle)+i));
			ret[i]*=0.01f;
		}
		return ret;
	}

	public XyrisJointValues GetXyrisJointValues()
	{
		XyrisJointValues v = new XyrisJointValues ();
		v.LeftCrawlerRPM = (float)(_driver.GetXyrisUInt (PLCDriver.EXyrisDataField.mainCrwlLeftRPM));
		v.RightCrawlerRPM = (float)(_driver.GetXyrisUInt (PLCDriver.EXyrisDataField.mainCrwlRightRPM));
		v.FrontLefttRail=(float)(_driver.GetXyrisShort (PLCDriver.EXyrisDataField.subCrwlFrontLeft));
		v.BackLeftRail=(float)(_driver.GetXyrisShort (PLCDriver.EXyrisDataField.subCrwlBackLeft));

		v.FrontRightRail=(float)(_driver.GetXyrisShort (PLCDriver.EXyrisDataField.subCrwlFrontRight));
		v.BackRightRail=(float)(_driver.GetXyrisShort (PLCDriver.EXyrisDataField.subCrwlBackRight));


		v.LeftRail=(float)(_driver.GetXyrisShort (PLCDriver.EXyrisDataField.traverseLeft));
		v.RightRail=(float)(_driver.GetXyrisShort (PLCDriver.EXyrisDataField.traverseRight));

		v.YBMJoint=(float)(_driver.GetYBMUShort (PLCDriver.EYbmDataField.rodPitch))/10.0f;
		v.YBMRod = 0;//(float)(_driver.GetYBMUShort (PLCDriver.EYbmDataField.rodRaised));

		v.YBMBaseRoll=(float)(_driver.GetYBMUShort (PLCDriver.EYbmDataField.baseRoll))/10.0f;
		return v;
	}

	public Vector2 GetGPSLocation()
	{
		return new Vector2(
	}

	public void OnRobotStatus(RobotDataCommunicator.ERobotControllerStatus status)
	{
	}
	public void OnCameraFPS(int c0,int c1)
	{
	}

	public float GetCrawlerBatteryLevel()
	{
		return _driver.GetXyrisUInt (PLCDriver.EXyrisDataField.battVoltage)/100.0f;
	}

	
	public float GetOthersBatteryLevel()
	{
		return _driver.GetCommonUShort(PLCDriver.ECommonDataField.battertVoltage)/10.0f;
	}
	public int GetUserRSSI()
	{
		return _driver.GetCommonShort(PLCDriver.ECommonDataField.rssi_base);
	}
	public int GetRobotRSSI()
	{
		return _driver.GetCommonShort(PLCDriver.ECommonDataField.rssi_robot);
	}

}
