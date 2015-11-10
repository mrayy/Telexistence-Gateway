using UnityEngine;
using System.Collections;

public class SphereTeleportBehaviour : ITeleportBehaviour {


	enum EMode
	{
		Entering,
		Entered,
		Exiting,
		Exitted
	}

	TelubeeOVRCamera _camera;
	EMode _mode=EMode.Exitted;

	float _scale=0;

	// Use this for initialization
	void Start () {
		_switchMode (EMode.Exitted);
	}
	
	// Update is called once per frame
	void Update () {
		switch (_mode) {
		case EMode.Entering:
			_scale+=0.1f*Time.deltaTime;
			if(_scale>0.5f)
			{
				_scale=1;
				_switchMode(EMode.Entered);
			}
			this.transform.localScale=new Vector3(_scale,_scale,_scale);

			break;
		case EMode.Entered:
			break;
		case EMode.Exiting:
			_scale-=0.1f*Time.deltaTime;
			if(_scale<0.5f)
			{
				_scale=0;
				_switchMode(EMode.Exitted);
			}
			this.transform.localScale=new Vector3(_scale,_scale,_scale);
			break;
		case EMode.Exitted:
			break;
		}
	}


	void _switchMode(EMode mode)
	{
		_mode = mode;
		switch (_mode) {
		case EMode.Entering:
			transform.localScale=Vector3.zero;
			GetComponent<Renderer>().enabled=true;

			break;
		case EMode.Entered:
			if(OnEntered!=null)
				OnEntered(this);
			break;
		case EMode.Exiting:
			break;
		case EMode.Exitted:
			transform.localScale=Vector3.zero;
			GetComponent<Renderer>().enabled=false;
			if(OnExitted!=null)
				OnExitted(this);
			break;
		}
	}
	
	public override void OnEnter(TelubeeOVRCamera camera)
	{
		_camera = camera;
		_switchMode(EMode.Entering);
	}
	public override void OnExit()
	{
		_switchMode(EMode.Exiting);
	}

	public override bool IsActive()
	{
		return _mode != EMode.Exitted;
	}
}
