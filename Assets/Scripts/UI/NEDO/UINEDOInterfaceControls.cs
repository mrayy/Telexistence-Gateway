using UnityEngine;
using System.Collections;

public class UINEDOInterfaceControls : MonoBehaviour {


	public PLCDriverObject PLCDriver;

	public UISlider BatteryLevel;

	// Use this for initialization
	void Start () {
		if (PLCDriver == null) {
			PLCDriver=GameObject.FindObjectOfType<PLCDriverObject>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PLCDriver!=null) {
			SetBatteryLevel(PLCDriver.GetBatteryLevel());
			SetSpeed(PLCDriver.GetSpeed());
		}
	}

	public void SetBatteryLevel(float level)
	{
		BatteryLevel.value = level;
	}

	public void SetWifiLevel(float level)
	{
	}

	public void SetSpeed(float speed)
	{
	}

	public void SetCondition(int condition)
	{
	}

	public void SetErrorMessage(string message)
	{
	}
}
