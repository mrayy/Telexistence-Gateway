using UnityEngine;
using System.Collections;

public class NEDORobotConnectionStarter : MonoBehaviour {
	
	public RobotConnectionComponent Robot;
	public UINEDOConnectScreen ConnectScreen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown ("ConnectRobot")) {
			if(!Robot.IsConnected)
			{
				Robot.ConnectRobot();
				ConnectScreen.SetConnected(true);
			}
			else{
				Robot.DisconnectRobot();
				ConnectScreen.SetConnected(false);
			}

		}
		if ( Input.GetButtonDown ("StartRobot")) {
			if(!Robot.IsRobotStarted())
				Robot.StartUpdate();
			else
				Robot.EndUpdate();
		}
		if (Input.GetButtonDown ("CalibrateRobot"))  {
			Robot.Recalibrate();
		}

	}
}
