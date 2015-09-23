using UnityEngine;
using System.Collections;
using System.Xml;
using System;

[Serializable]
public class CameraConfigurations  {
	
	public enum ECameraRotation
	{
		None,
		CW,	//90 degrees rotation Clock Wise
		CCW,//90 degrees rotation Counter Clock Wise
		Flipped //180 degrees flipped
	};
	
	public string Name;
	public float FoV;			  //horizontal field of view for the camera measured in degrees
	public float CameraOffset=0; //physical offset from human eye
	public float StereoOffset=0.065f;	//physical distance between both eyes
	public Vector2 LensCenter; 
	public Vector2 FocalLength=new Vector2(1,1);
	public Vector4 KPCoeff=Vector4.zero;
	public Vector2 PixelShiftLeft= Vector2.zero;
	public Vector2 PixelShiftRight= Vector2.zero;
	public float Focal=1;
	public ECameraRotation[] Rotation=new ECameraRotation[2];

	/*
	//http://docs.opencv.org/doc/tutorials/calib3d/camera_calibration/camera_calibration.html
	public float fov=60;			//horizontal field of view for the camera measured in degrees
	public float cameraOffset=0;	//physical offset from human eye
	public float stereoOffset=0.065f;	//physical distance between both eyes
	
	public ECameraRotation[] cameraRotation=new ECameraRotation[2];
	
	public Vector2 OpticalCenter=new Vector2(0.5f,0.5f);
	public Vector2 FocalCoeff=new Vector2(1,1);
	public Vector4 KPCoeff=Vector4.zero;
	public string Name="";*/


	public void LoadXML(XmlReader r)
	{
		Name=r.GetAttribute ("Name");
		float.TryParse( r.GetAttribute ("FOV"),out FoV);
		float.TryParse( r.GetAttribute ("CameraOffset"),out CameraOffset);
		LensCenter=Utilities.ParseVector2( r.GetAttribute ("OpticalCenter"));
		FocalLength=Utilities.ParseVector2( r.GetAttribute ("FocalCoeff"));
		KPCoeff=Utilities.ParseVector4( r.GetAttribute ("KPCoeff"));
		Vector4 PixelShift=Utilities.ParseVector4( r.GetAttribute ("PixelShift"));
		PixelShiftLeft.Set (PixelShift.x, PixelShift.y);
		PixelShiftRight.Set (PixelShift.z, PixelShift.w);
		string rot;
		string[] names = new string[]{"LeftRotation","RightRotation"};
		for (int i=0; i<2; ++i) {
			rot = r.GetAttribute (names[i]).ToLower ();
			switch (rot) {
			case "none":
				Rotation[i]=ECameraRotation.None;
				break;
			case "flipped":
				Rotation[i]=ECameraRotation.Flipped;
				break;
			case "cw":
				Rotation[i]=ECameraRotation.CW;
				break;
			case "ccw":
				Rotation[i]=ECameraRotation.CCW;
				break;
			}
		}
	}
}
