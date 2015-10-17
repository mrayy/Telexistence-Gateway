using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	private GameObject myGate;
	private GameObject gateFrame;

	// Use this for initialization
	void Start () {
		myGate = (GameObject)Resources.Load ("Door/MyGate");

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {

			ApperDoor ();

		}

	
	}

	void initGate(){

		gateFrame = GameObject.Find ("GateFrame");
//		gateFrame.transform.localScale = new Vector3(0.2f, 0.2f, 0.1f);
//		gateFrame.transform.position.x += 1f;
	}


	void ApperDoor(){

		initGate ();
		Vector3 position = this.transform.position;
		position.z += 1.6f;
		position.x += 0.2f;
		Instantiate (myGate, position, Quaternion.identity);
	}



}
