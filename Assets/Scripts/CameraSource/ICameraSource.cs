using UnityEngine;
using System.Collections;

public enum EyeName
{
	LeftEye=0,
	RightEye=1
}

public interface ICameraSource  {
	void Init();

    Texture GetEyeTexture(EyeName e);

    Rect GetEyeTextureCoords(EyeName e);
	Vector2 GetEyeScalingFactor(EyeName e);

	void Close(); 
}
