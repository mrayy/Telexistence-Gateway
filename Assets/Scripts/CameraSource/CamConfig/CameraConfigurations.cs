using UnityEngine;
using System.Collections;
using System.Xml;

public class CameraConfigurations  {
	
	public enum ECameraRotation
	{
		None,
		CW,	//90 degrees rotation Clock Wise
		CCW,//90 degrees rotation Counter Clock Wise
		Flipped //180 degrees flipped
	};
	
	//http://docs.opencv.org/doc/tutorials/calib3d/camera_calibration/camera_calibration.html
	public float fov=60;			//horizontal field of view for the camera measured in degrees
	public float cameraOffset=0;	//physical offset from human eye
	public float stereoOffset=0.065f;	//physical distance between both eyes
	
	public ECameraRotation[] cameraRotation=new ECameraRotation[2];
	
	public Vector2 OpticalCenter=new Vector2(0.5f,0.5f);
	public Vector2 FocalCoeff=new Vector2(1,1);
	public Vector4 KPCoeff=Vector4.zero;
	public string Name="";


	public void LoadXML(XmlReader r)
	{
		Name=r.GetAttribute ("Name");
		float.TryParse( r.GetAttribute ("FOV"),out fov);
		float.TryParse( r.GetAttribute ("CameraOffset"),out cameraOffset);
		OpticalCenter=Utilities.ParseVector2( r.GetAttribute ("OpticalCenter"));
		FocalCoeff=Utilities.ParseVector2( r.GetAttribute ("FocalCoeff"));
		KPCoeff=Utilities.ParseVector4( r.GetAttribute ("KPCoeff"));
		string rot;
		string[] names = new string[]{"LeftRotation","RightRotation"};
		for (int i=0; i<2; ++i) {
			rot = r.GetAttribute (names[i]).ToLower ();
			switch (rot) {
			case "none":
				cameraRotation[i]=ECameraRotation.None;
				break;
			case "flipped":
				cameraRotation[i]=ECameraRotation.Flipped;
				break;
			case "cw":
				cameraRotation[i]=ECameraRotation.CW;
				break;
			case "ccw":
				cameraRotation[i]=ECameraRotation.CCW;
				break;
			}
		}
	}
}
