﻿using UnityEngine;
using System.Collections;
using System;

public class TELUBeeConfiguration : MonoBehaviour {

	[Serializable]
	public class CameraSettings
	{
		public string Name;
		public float FoV;
		public float CameraOffset=0;
		public Vector2 LensCenter;
		public Vector2 FocalLength;
		public float K1,K2,P1,P2;
		public bool Flipped=false;
	}

	public CameraSettings CamSettings;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
