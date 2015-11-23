using UnityEngine;
using System.Collections;

public class TorsoVehicle : MonoBehaviour {

	public Rigidbody MainChassis;
	public Rigidbody LeftRail;
	public Rigidbody RightRail;

	public Transform LeftFrontRail;
	public Transform LeftBackRail;
	public Transform RightFrontRail;
	public Transform RightBackRail;

	public float MaxSpeed = 10;
	public float Acceleration = 0.5f;
	public float MaxRotationSpeed = 30;

	float CurrentSpeed = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(Input.GetKey(KeyCode.UpArrow))
		{
			CurrentSpeed += Acceleration * Time.fixedDeltaTime;

			if(CurrentSpeed > MaxSpeed)// Check and limit vehicle speed
				CurrentSpeed = MaxSpeed;
		}

		MainChassis.AddForce (0, 0, CurrentSpeed);
		

		//Deccleration 
		CurrentSpeed -= 0.1f * Time.fixedDeltaTime;

		if (CurrentSpeed < 0) 
		{
			CurrentSpeed = 0;
		}
	}
}
