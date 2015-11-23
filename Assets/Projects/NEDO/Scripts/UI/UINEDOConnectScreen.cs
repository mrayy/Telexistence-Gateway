using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UINEDOConnectScreen : MonoBehaviour {

	public UIWidget ScreenHandler;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetConnected(bool connected)
	{
		ScreenHandler.gameObject.SetActive(!connected);
	}

}
