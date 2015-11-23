using UnityEngine;
using System.Collections;

public class TorsoController : MonoBehaviour {

	public Transform HeadRoll;
	public Transform HeadTilt;
	public Transform HeadPan;

	public Transform BaseX;
	public Transform BaseY;
	public Transform BaseZ;

	Matrix4x4 targetMatrix;
	const float L1 = 0.5f;
	bool m_calibrated=false;
	Vector3 m_headOffset=new Vector3();
	Vector3 m_headPos=new Vector3();
	float[] m_targetAngles=new float[6];


	// Use this for initialization
	void Start () {
	}
	float vecLength3(float dx, float dy, float dz)
	{
		return Mathf.Sqrt((float)(dx*dx + dy*dy + dz*dz));
	}
	enum myAxis
	{
		axisX,
		axisY,
		axisZ
	}
	void MakeMatrix(myAxis axis, float radian, float transX, float transY, float transZ,ref Matrix4x4 matrix)
	{
		float c = Mathf.Cos(radian);
		float s = Mathf.Sin(radian);
		switch (axis){
		case myAxis.axisX:///@bug ƒoƒ‰ƒoƒ‰‚È‡”Ô‚Éƒƒ‚ƒŠƒAƒNƒZƒX‚µ‚Ä“Ç‚Ýž‚ÝŒø—¦‚ªˆ«‚¢‚©‚à‚µ‚ê‚È‚¢
			matrix[0] = 1;	matrix[4] = 0;	matrix[8] = 0;	matrix[12] = transX;
			matrix[1] = 0;	matrix[5] = c;	matrix[9] = -s;	matrix[13] = transY;
			matrix[2] = 0;	matrix[6] = s;	matrix[10] = c;	matrix[14] = transZ;
			matrix[3] = 0;	matrix[7] = 0;	matrix[11] = 0;	matrix[15] = 1;
			break;
		case myAxis.axisY:
			matrix[0] = c;	matrix[4] = 0;	matrix[8] = s;	matrix[12] = transX;
			matrix[1] = 0;	matrix[5] = 1;	matrix[9] = 0;	matrix[13] = transY;
			matrix[2] = -s;	matrix[6] = 0;	matrix[10] = c;	matrix[14] = transZ;
			matrix[3] = 0;	matrix[7] = 0;	matrix[11] = 0;	matrix[15] = 1;
			break;
		case myAxis.axisZ:
			matrix[0] = c;	matrix[4] = -s;	matrix[8] = 0;	matrix[12] = transX;
			matrix[1] = s;	matrix[5] = c;	matrix[9] = 0;	matrix[13] = transY;
			matrix[2] = 0;	matrix[6] = 0;	matrix[10] = 1;	matrix[14] = transZ;
			matrix[3] = 0;	matrix[7] = 0;	matrix[11] = 0;	matrix[15] = 1;
			break;
		}
	}

	
	void MatrixtoXYZ(Matrix4x4 matrix, ref float a,ref  float b,ref  float c)
	{
		
		c = Mathf.Atan2(matrix[4], matrix[0]);
		b = -Mathf.Asin(matrix[8]);
		if (Mathf.Cos(b) != 0){
			a = Mathf.Asin((matrix[9] / Mathf.Cos(b)));
			if (matrix[10]<0)
				a = Mathf.PI - a;

		}
		else{
			a = 0;
			c = Mathf.Atan2(matrix[1], matrix[5]);
		}
		
	}


	void CalculateTargetValues(ref float[] param)
	{
		
		float len=vecLength3(targetMatrix[12],targetMatrix[13],targetMatrix[14]-0.17f);
		param[2]=len-L1;
		
		if(len!=0){
			float tmp=targetMatrix[12]/len;
			tmp=tmp>+1?+1:tmp;
			tmp=tmp<-1?-1:tmp;
			param[1]=Mathf.Asin(tmp);
			
			
			tmp=-targetMatrix[13]/(len*Mathf.Cos(param[1]));
			tmp=tmp>+1?+1:tmp;
			tmp=tmp<-1?-1:tmp;
			param[0]=Mathf.Asin(tmp);
		}else{
			param[0]=0;
			param[1]=0;
		}
		
		
		Matrix4x4 mat,tmpM,tmpM2;
		mat=new Matrix4x4();
		MakeMatrix(myAxis.axisX,0,0,0,-0.17f,ref mat);
		tmpM2=mat*targetMatrix;
		MakeMatrix(myAxis.axisX,-param[0],0,0,0,ref mat);
		tmpM=mat*tmpM2;
		MakeMatrix(myAxis.axisY,-param[1],0,0,-len,ref mat);
		tmpM2=mat*tmpM;
		MatrixtoXYZ(tmpM2,ref param[3],ref param[4],ref param[5]);

		param[3]=-param[3];
		param[4]=-param[4];
		param[5]=-param[5];
	}
	
	void qtomatrix(ref Matrix4x4 m, Quaternion q)
	{

		float X = q.x;
		float Y = q.y;
		float Z = q.z;
		float W = q.w;

		float    x2 = X * X;
		float    y2 = Y * Y;
		float    z2 = Z * Z;
		
		m[0,0] = 1 - 2 * (y2 +  z2);
		m[0,1] = 2 * (X * Y + W * Z);
		m[0,2] = 2 * (X * Z - W * Y);
		m[0,3] = 0.0f;
		
		m[1,0] = 2 * (X * Y - W * Z);
		m[1,1] = 1 - 2 * (x2 + z2);
		m[1,2] = 2 * (Y * Z + W * X);
		m[1,3] = 0.0f;
		
		m[2,0] = 2 * (X * Z + W * Y);
		m[2,1] = 2 * (Y * Z - W * X);
		m[2,2] = 1 - 2 * (x2 + y2);
		m[2,3] = 0.0f;
		
		m[3,3] = 1.0f;
	}
	
	void ConvertToMatrix(Quaternion q, Vector3 pos, ref Matrix4x4 mat)
	{
		Matrix4x4 m=new Matrix4x4();
		qtomatrix(ref m, q);
		
		
		//added by yamen: include head position in the matrix
		m[3,0] = pos[0];
		m[3,1] = pos[1];
		m[3,2] = pos[2];

		mat[0] = m[0,0];
		mat[1] = m[1,0];
		mat[2] = m[2,0];
		mat[3] = 0;
		
		mat[4] = m[0,1];
		mat[5] = m[1,1];
		mat[6] = m[2,1];
		mat[7] = 0;
		
		mat[8] = m[0,2];
		mat[9] = m[1,2];
		mat[10] = m[2,2];
		mat[11] = 0;
		

		
		mat[12] = m[3,0];
		mat[13] = m[3,1];
		mat[14] = m[3,2];
		mat[15] = 1;
		
		
	}
	void CreateTargetRotationMatrix(Vector3 pos,Quaternion ori)
	{
		Quaternion targetQuat=new Quaternion(ori[0],ori[1],ori[2],-ori[3]);
		m_headPos.Set(-pos[2],-pos[0],pos[1]);

		Vector3 p = m_headPos + m_headOffset;

		ConvertToMatrix (targetQuat, p,ref targetMatrix);
	}

	// Update is called once per frame
	void Update () {
        /*
		if (OVRManager.instance == null)
			return;
		OVRPose pos=OVRManager.display.GetEyePose (OVREye.Right);
		Vector3 eulers= pos.orientation.eulerAngles;

		CreateTargetRotationMatrix(pos.position,pos.orientation);

		CalculateTargetValues (ref m_targetAngles);

//		Debug.Log(string.Format("{0},{1},{2} - {3},{4},{5}",m_targetAngles[0],m_targetAngles[1],m_targetAngles[2],
//		                        m_targetAngles[3],m_targetAngles[4],m_targetAngles[5]));


		HeadTilt.localRotation = Quaternion.Euler (m_targetAngles[3]*Mathf.Rad2Deg + m_targetAngles[0]*Mathf.Rad2Deg,0,0);
		HeadPan .localRotation = Quaternion.Euler (0,m_targetAngles[4]*Mathf.Rad2Deg,0);
		HeadRoll.localRotation = Quaternion.Euler (0,0,m_targetAngles[5]*Mathf.Rad2Deg + m_targetAngles[0]*Mathf.Rad2Deg);

		BaseY.localRotation = Quaternion.Euler (-m_targetAngles[1]*Mathf.Rad2Deg,0,0);
		BaseX.localRotation = Quaternion.Euler (0,0,-m_targetAngles[0]*Mathf.Rad2Deg);

		BaseZ.localPosition = new Vector3(0,m_targetAngles [2],0);

		if (Input.GetButtonDown ("CalibrateRobot")) {
			m_calibrated = false;
		}
		if(!m_calibrated)
		{
			m_calibrated=true;
			m_headOffset=-m_headPos;
			m_headOffset.z+=0.7f;
		}
        */
	}
}
