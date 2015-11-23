using UnityEngine;
using System.Collections;
using System;

public class UINEDOWifi : MonoBehaviour {

	[Serializable]
	public class SignalLevel
	{
		public Sprite img;
		public int level;
	}
	public SignalLevel[] levels;

	public UI2DSprite image;

	public UILabel label;

	public int level;

	int m_level=-1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SetLevel (level);
	}

	public void SetLevel(int level)
	{
		if (m_level == level)
			return;
		label.text = level.ToString () + "db";
		bool found = false;
		for(int i=0;i<levels.Length;++i)
		{
			if(level<levels[i].level)
			{
				image.sprite2D=levels[i].img;
				found=true;
				break;
			}
		}
		if (!found)
			image.sprite2D = levels [levels.Length - 1].img;
	}
}
