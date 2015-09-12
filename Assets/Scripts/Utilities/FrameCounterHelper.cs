using UnityEngine;
using System.Collections;

public class FrameCounterHelper  {
	
	int m_frameCounter = 0;
	float m_timeCounter = 0.0f;
	int m_lastFramerate = 0;
	public float m_refreshTime = 1;
	float m_lastTime=0;

	public int FPS
	{
		get{
			return m_lastFramerate;
		}
	}
	public FrameCounterHelper()
	{
		m_lastTime = UnityEngine.Time.time;
	}

	public void AddFrame()
	{
		float t = UnityEngine.Time.time;
		float dt=t-m_lastTime;
		m_timeCounter += dt;
		m_frameCounter++;
		if( m_timeCounter >= m_refreshTime )
		{
			//This code will break if you set your m_refreshTime to 0, which makes no sense.
			m_lastFramerate = (int)(m_frameCounter/m_timeCounter);
			m_frameCounter = 0;
			m_timeCounter = 0.0f;
		}
		m_lastTime = t;
	}
}
