using UnityEngine;
using System.Collections;

public class OculusHeadController : IRobotHeadControl {

	Quaternion _initial;

	public bool GetHeadOrientation(out Quaternion q, bool abs)
	{
		if (Ovr.Hmd.Detect () == 0) {
			q=Quaternion.identity;
			return false;
		}

		q = OVRManager.display.GetHeadPose (0).orientation;
		if (!abs) {
		//	q=q*_initial;
		}
		q.x = -q.x;
		q.y = -q.y;
		return true;
	}
	public bool GetHeadPosition(out Vector3 v,bool abs)
	{
		
		if (Ovr.Hmd.Detect () == 0) {
			v=Vector3.zero;
			return false;
		}
		v = OVRManager.display.GetHeadPose (0).position;
		return true;
	}
	
	public void Recalibrate()
	{
		
		if (Ovr.Hmd.Detect () >0) {
			OVRManager.display.RecenterPose();
		}
	}
}
