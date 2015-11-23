using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {

	public GameObject[] Cameras;
	public int ActiveCamera;
	void Start () {
		foreach (GameObject cam in Cameras) {
			cam.SetActive(false);
		}

		Cameras [ActiveCamera].SetActive (true);
	}

	void Update() {

		if(Input.GetKeyDown(KeyCode.C)) {
			Cameras[ActiveCamera].SetActive(false);
			ActiveCamera=(ActiveCamera+1)%Cameras.Length;
			Debug.Log(ActiveCamera);
			Cameras[ActiveCamera].SetActive(true);

		}

	}
	

}
