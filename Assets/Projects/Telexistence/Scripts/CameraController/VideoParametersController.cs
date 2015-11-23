using UnityEngine;
using System.Collections;

public class VideoParametersController : MonoBehaviour {

	public NetValueObject TargetValueObject;

	public float ExposureValue;
	public float GainValue;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (TargetValueObject == null)
			return;
		NetValueObject.ValueControllerCtl gain= TargetValueObject.GetValue ("Camera.Gain");
		NetValueObject.ValueControllerCtl exposure=TargetValueObject.GetValue ("Camera.Brightness");

		if(gain!=null)
			gain.value = GainValue.ToString ();
		if(exposure!=null)
			exposure.value = ExposureValue.ToString ();

		TargetValueObject.SendData ();
	}
}
