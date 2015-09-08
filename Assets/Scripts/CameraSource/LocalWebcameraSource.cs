using UnityEngine;
using System.Collections;

public class LocalWebcameraSource : ICameraSource {

    public int LeftInputCamera;
    public int RightInputCamera;

	public int ResolutionX=640;
	public int ResolutionY=480;

    WebCamTexture _LeftCam;
    WebCamTexture _RightCam;
	
	public GstBaseTexture GetBaseTexture()
	{
		return null;
	}
    public void Init()
    {
        LeftInputCamera = Mathf.Min(LeftInputCamera, WebCamTexture.devices.Length - 1);
        RightInputCamera = Mathf.Min(RightInputCamera, WebCamTexture.devices.Length - 1);
        for (int i = 0; i < WebCamTexture.devices.Length; ++i)
            Debug.Log("Camera [" + i.ToString() + "]: " + WebCamTexture.devices[i].name);

        if (LeftInputCamera != -1)
        {
			_LeftCam = new WebCamTexture(WebCamTexture.devices[LeftInputCamera].name, ResolutionX, ResolutionY);
            _LeftCam.Play();
        }
        if (RightInputCamera != -1)
        {
			_RightCam = new WebCamTexture(WebCamTexture.devices[RightInputCamera].name, ResolutionX, ResolutionY);
            _RightCam.Play();
        }
    }
	public void Close()
	{
		if (_LeftCam != null) {
			_LeftCam.Stop ();
		}
		if (_RightCam != null) {
			_RightCam.Stop ();
		}
	}
    public Texture GetEyeTexture(EyeName e)
    {
        if (e == EyeName.LeftEye)
            return _LeftCam;
        return _RightCam;
    }

    public Rect GetEyeTextureCoords(EyeName e)
    {
        return new Rect(0, 0, 1, 1);
	}
	public Vector2 GetEyeScalingFactor(EyeName e)
	{
		return Vector2.one;
	}
}
