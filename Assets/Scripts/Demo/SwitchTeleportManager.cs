using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchTeleportManager : MonoBehaviour {

	public TelubeeOVRCamera Robot1;
	public TelubeeOVRCamera Robot2;

	public Material ActiveMaterial;
	public Material RemoteMaterial;

	public ITeleportBehaviour TeleporterDoor;

	public RobotConnectionComponent ActiveRobot
	{
		get
		{
			if(TeleporterDoor.IsActive())
				return Robot2.RobotConnector;
			return Robot1.RobotConnector;
		}
	}

	// Use this for initialization
	void Start () {
		Robot1.ApplyMaterial (ActiveMaterial);
		Robot2.ApplyMaterial (RemoteMaterial);

		TeleporterDoor.OnEntered += OnEntered;
		TeleporterDoor.OnExitted += OnExitted;
	}


	void Connect(RobotConnectionComponent r)
	{
		if(!r.IsConnected)
			r.ConnectRobot();
		else r.DisconnectRobot();
	}

	void StartRobot(RobotConnectionComponent r)
	{
		if(!r.IsRobotStarted())
			r.StartUpdate();
		else
			r.EndUpdate();
	}

	void SwitchTeleport(bool on)
	{
		if (on) {
			Robot2.RobotConnector.StartUpdate ();
			TeleporterDoor.OnEnter (Robot2);
		} else {
			Robot1.RobotConnector.StartUpdate();
			TeleporterDoor.OnExit();
		}
	}
	
	void OnEntered(ITeleportBehaviour t)
	{
		Robot1.RobotConnector.EndUpdate ();
	}
	void OnExitted(ITeleportBehaviour t)
	{
		Robot2.RobotConnector.EndUpdate();
	}
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("ConnectRobot")) {
			Connect (Robot1.RobotConnector);
			Connect (Robot2.RobotConnector);
		}
		if ( Input.GetButtonDown ("StartRobot")) {
			SwitchTeleport(false);
			//StartRobot (Robot1.RobotConnector);
			//StartRobot (Robot2.RobotConnector);

		}
		if (Input.GetButtonDown ("CalibrateRobot"))  {
			Robot1.RobotConnector.Recalibrate();
			Robot2.RobotConnector.Recalibrate();
		}


		if (Input.GetKeyDown (KeyCode.T)) {
			SwitchTeleport(!TeleporterDoor.IsActive());
		}
	}

}
