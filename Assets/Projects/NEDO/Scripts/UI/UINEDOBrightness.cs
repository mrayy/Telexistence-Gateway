using UnityEngine;
using System.Collections;

public class UINEDOBrightness : MonoBehaviour {

	public UISlider BrightnessSlider;
	public UILabel  BrigntnessLabel;
	public UI2DSprite BrigntnessIcon;
	public UIWidget Holder;

	public VideoParametersController VideoParams;


	public float Value=0.5f;

	float _alpha;
	float _timeout=0;
	float _value=0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("BrightnessUp"))
			Value += 0.1f;
		if (Input.GetButtonDown ("BrightnessDown"))
			Value -= 0.1f;

		if (Value != _value) {
			SetBrightness(Value);
		}
		if (_timeout > 0) {
			_timeout -= Time.deltaTime;
		} else {
			if(_alpha>0)
			{
				_alpha-=Time.deltaTime;
				if(_alpha<=0)
					_alpha=0;
			}
		}

		Holder.alpha = _alpha;
	}

	public void SetBrightness(float v)
	{
		if (v > 1)
			v = 1;
		if (v < 0)
			v = -1;
		//Value = v;
		_value = v;
		_alpha = 1;

		if (VideoParams != null) {
			VideoParams.ExposureValue=_value*0.6f+0.1f;
		}

		_timeout = 5;

		BrigntnessIcon.alpha = _value * 0.8f + 0.2f;
		if (BrigntnessIcon.alpha < 0)
			BrigntnessIcon.alpha = 1;
		BrigntnessLabel.text = ((int)(_value * 100)).ToString () + "%";
		BrightnessSlider.value = _value;
	}
	public float GetBrigtness(){
		return Value;
	}
}
