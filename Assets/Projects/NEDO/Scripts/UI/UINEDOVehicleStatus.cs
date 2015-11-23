using UnityEngine;
using System.Collections;

public class UINEDOVehicleStatus : MonoBehaviour {
	
	public UILabel AngleLabel;
	public UI2DSprite angleBackground;
	public UI2DSprite TorsoStatus;
	public UI2DSprite XyrisStatus;
	public UI2DSprite YBMStatus;
	
	public Sprite InfoBG;
	public Sprite WarningBG;
	public Sprite ErrorBG;


	enum EWarningLevel
	{
		Normal,
		Warning,
		High
	}
	EWarningLevel _warningLevel=EWarningLevel.Normal;

	float _angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetTorsoStatus(int s,bool overCurrent)
	{
		if (overCurrent)
			TorsoStatus.sprite2D = ErrorBG;
		else {
			if (s == 1)//TORSO: Parked
				TorsoStatus.sprite2D = WarningBG;
			else if (s == 3)//TORSO: Operation
				TorsoStatus.sprite2D = InfoBG;
		}
	}
	public void SetAngle(float angle)
	{
		_angle = angle;

		AngleLabel.text = angle.ToString ("0.0") + "°";

		if (_angle >= 41) {
			_warningLevel = EWarningLevel.High;

			angleBackground.sprite2D=ErrorBG;
		} else if (_angle >= 36) {
			_warningLevel = EWarningLevel.Warning;
			angleBackground.sprite2D=WarningBG;
		} else if (_angle >= -36) {
			_warningLevel = EWarningLevel.Normal;
			angleBackground.sprite2D=InfoBG;
		} else if (_angle >= -42) {
			_warningLevel = EWarningLevel.Warning;
			angleBackground.sprite2D=WarningBG;
		} else {
			_warningLevel = EWarningLevel.High;
			angleBackground.sprite2D=ErrorBG;
		}
	}
}
