using UnityEngine;
using System.Collections;

public class OculusHeadController : IRobotHeadControl {

	Quaternion _initial;
	Vector3 _neckOffset=Vector3.zero;

	public OculusHeadController()
	{
		if (Ovr.Hmd.Detect () == 0) {
			return;
		}
		float[] neckOffset = new float[] {
			Ovr.Hmd.OVR_DEFAULT_NECK_TO_EYE_HORIZONTAL,
			Ovr.Hmd.OVR_DEFAULT_NECK_TO_EYE_VERTICAL
		};
		neckOffset= OVRManager.capiHmd.GetFloatArray (Ovr.Hmd.OVR_KEY_NECK_TO_EYE_DISTANCE, neckOffset);
		this._neckOffset = new Vector3 (0, neckOffset [1], neckOffset [0]);
		_neckOffset.y = 0;
		//_neckOffset.z = 0;
	}

	public bool GetHeadOrientation(out Quaternion q, bool abs)
	{
		if (Ovr.Hmd.Detect () == 0) {
			q=Quaternion.identity;
			return false;
		}

		Ovr.Posef ts= OVRManager.capiHmd.GetTrackingState (0).HeadPose.ThePose;
	//	q = ts.Orientation.ToQuaternion(false);
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
//		v = OVRManager.display.GetHeadPose (0).position;
		Ovr.Posef ts= OVRManager.capiHmd.GetTrackingState (0).HeadPose.ThePose;

		Quaternion q = ts.Orientation.ToQuaternion (false);
		
		q.x = -q.x;
		q.y = -q.y;
		v = ts.Position.ToVector3(true)- q*_neckOffset;
		return true;
	}
	
	public void Recalibrate()
	{
		
		if (Ovr.Hmd.Detect () >0) {
			OVRManager.display.RecenterPose();
		}
	}
}
