using UnityEngine;
using System.Collections;

public class OculusBaseController:IRobotBaseControl {
	
	
	Vector3 m_headPosCalib=Vector3.zero;
	
	Vector2 m_currentSpeed=Vector2.zero;
	float m_currentRotation=0;

	public Vector2 GetSpeed()
	{
		if (Ovr.Hmd.Detect () == 0) {
			return Vector2.zero;
		}

		
		Vector3 p = OVRManager.display.GetEyePose (OVREye.Left).position;
		Vector3 diff = p - m_headPosCalib;
		
		float x = diff.z;
		float y = diff.x;
		
		float minOffset = 0.05f;
		float maxOffset = 0.15f;
		
		x = Mathf.Sign(x)*Mathf.Pow(Mathf.Min(1.0f, Mathf.Max(0, Mathf.Abs(x) - minOffset) / (maxOffset-minOffset)),2);
		y = Mathf.Sign(y)*Mathf.Pow(Mathf.Min(1.0f, Mathf.Max(0, Mathf.Abs(y) - minOffset) / (maxOffset - minOffset)),2);
		
		m_currentSpeed += new Vector2(-x, y)*Time.deltaTime * 2;
		m_currentSpeed -= m_currentSpeed*Time.deltaTime*1;
		m_currentSpeed.x = Mathf.Clamp(m_currentSpeed.x, -1, 1);
		m_currentSpeed.y = Mathf.Clamp(m_currentSpeed.y, -1, 1);
		return m_currentSpeed;
	}
	public float GetRotation()
	{
		if (Ovr.Hmd.Detect () == 0) 
			return 0;
		
		float y = OVRManager.display.GetEyePose (OVREye.Left).orientation.eulerAngles.y;
		
		
		float minAngle = 15;
		float maxAngle = 60;
		
		y = Mathf.Sign(y)*Mathf.Pow(Mathf.Min(1.0f, Mathf.Max(0, Mathf.Abs(y) - minAngle) / (maxAngle - minAngle)),2);
		//m_currentRotation = y;
		m_currentRotation += Mathf.Clamp(2 * y, -1, 1)*Time.deltaTime* 2;
		m_currentRotation -= m_currentRotation*Time.deltaTime*1.5f;
		m_currentRotation = Mathf.Clamp(m_currentRotation, -1, 1);
		
		
		float r = m_currentRotation;
		return -r;
	}
	public void Recalibrate()
	{
		if (Ovr.Hmd.Detect () >0) {
			m_headPosCalib = OVRManager.display.GetEyePose (OVREye.Left).position;
		}
	}
}
