using UnityEngine;
using System.Collections;

public class FrameCounterHelper  {
	
	int m_frameCounter = 0;
	float m_timeCounter = 0.0f;
	int m_lastFramerate = 0;
	public float m_refreshTime = 1;

	public int FPS
	{
		get{
			return m_lastFramerate;
		}
	}
	public FrameCounterHelper()
	{
	}

	public void AddFrame()
	{
		if( m_timeCounter < m_refreshTime )
		{
			m_timeCounter += UnityEngine.Time.deltaTime;
			m_frameCounter++;
		}
		else
		{
			//This code will break if you set your m_refreshTime to 0, which makes no sense.
			m_lastFramerate = (int)(m_frameCounter/m_timeCounter);
			m_frameCounter = 0;
			m_timeCounter = 0.0f;
		}
	}
}
