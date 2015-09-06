using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

 internal static class Utilities  {

	public static void DrawAxis(Matrix4x4 frame)
	{
		Vector3 X = frame.GetColumn (0);
		Vector3 Y = frame.GetColumn (1);
		Vector3 Z = frame.GetColumn (2);
		Vector3 pos = frame.GetColumn (3);
		Debug.DrawLine (pos,pos+X,Color.red);
		Debug.DrawLine (pos,pos+Y,Color.green);
		Debug.DrawLine (pos,pos+Z,Color.blue);
	}


		public static void FromMatrix4x4(this Transform transform, Matrix4x4 matrix)
		{
			transform.localScale = matrix.GetScale();
			transform.localRotation = matrix.GetRotation();
			transform.localPosition = matrix.GetPosition();
		}
		
		public static Quaternion GetRotation(this Matrix4x4 matrix)
		{
			var qw = Mathf.Sqrt(1f + matrix.m00 + matrix.m11 + matrix.m22) / 2;
			var w = 4 * qw;
			var qx = (matrix.m21 - matrix.m12) / w;
			var qy = (matrix.m02 - matrix.m20) / w;
			var qz = (matrix.m10 - matrix.m01) / w;
			
			return new Quaternion(qx, qy, qz, qw);
		}
		
		public static Vector3 GetPosition(this Matrix4x4 matrix)
		{
			var x = matrix.m03;
			var y = matrix.m13;
			var z = matrix.m23;
			
			return new Vector3(x, y, z);
		}
		
		public static Vector3 GetScale(this Matrix4x4 m)
		{
			var x = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
			var y = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
			var z = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);
			
			return new Vector3(x, y, z);
		}
	public static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}
	
	public static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}
	public static string LocalIPAddress()
	{
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (IPAddress ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
				break;
			}
		}
		return localIP;
	}


	public static string ToExportString(this Quaternion q)
	{
		return string.Format ("{0},{1},{2},{3}", q.w, q.x, q.y, q.z);
	}
	
	public static string ToExportString(this Vector2 q)
	{
		return string.Format ("{0},{1}",  q.x, q.y);
	}
	public static string ToExportString(this Vector3 q)
	{
		return string.Format ("{0},{1},{2}",  q.x, q.y, q.z);
	}

	public static Vector2 ParseVector2(string str)
	{
		try
		{
			string[] splits= str.Split (",".ToCharArray (), 2);
			if (splits.Length < 2)
				return Vector2.zero;
			return new Vector2(float.Parse(splits[0]),float.Parse(splits[1]));
		}catch
		{
			return Vector2.zero;
		}
	}
	public static Vector3 ParseVector3(string str)
	{
		try
		{
			string[] splits= str.Split (",".ToCharArray (), 3);
			if (splits.Length < 3)
				return Vector3.zero;
			return new Vector3(float.Parse(splits[0]),float.Parse(splits[1]),float.Parse(splits[2]));
		}catch
		{
			return Vector3.zero;
		}
	}
	public static Vector4 ParseVector4(string str)
	{
		try
		{
			string[] splits= str.Split (",".ToCharArray (), 4);
			if (splits.Length < 4)
				return Vector4.zero;
			return new Vector4(float.Parse(splits[0]),float.Parse(splits[1]),float.Parse(splits[2]),float.Parse(splits[3]));
		}catch
		{
			return Vector4.zero;
		}
	}
	public static Quaternion ParseQuaternion(string str)
	{
		try
		{
			string[] splits= str.Split (",".ToCharArray (), 4);
			if (splits.Length < 4)
				return Quaternion.identity;
			return new Quaternion(float.Parse(splits[1]),float.Parse(splits[2]),float.Parse(splits[3]),float.Parse(splits[0]));
		}catch
		{
				return Quaternion.identity;
		}
	}
}
