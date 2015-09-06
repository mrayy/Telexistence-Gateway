using UnityEngine;
using System.Collections;
//using UnityEditor;


public class AppManager : Manager<AppManager> 
{
	static AppManager _instance=null;

	public static AppManager Instance
	{
		get{
			if(_instance==null)
			{
				_instance=new AppManager();
			}
			return _instance;
		}
	}

	public enum HeadControllerType
	{
		None,
		Keyboard,
		Oculus,
		OptiTrack,
		SceneNode
	}
	
	public enum BaseControllerType
	{
		None,
		Keyboard,
		Joystick,
		Oculus,
		Wiiboard
	}

    public RobotInfoManager RobotManager;
    public CameraConfigurationsManager CamConfigManager;

    public AppManager()
	{
		if (_instance == null) {
			_instance = this;
			Debug.Log ("Start up!");
			Init ();
		}

	}

	void Init()
	{
		RobotManager = new RobotInfoManager();
		CamConfigManager = new CameraConfigurationsManager ();

		RobotManager.LoadRobots(Application.dataPath + "\\Data\\RobotsMap.xml");
		CamConfigManager.LoadConfigurations(Application.dataPath + "\\Data\\CameraConfigurations.xml");
	}

	public bool IsDebugging;




}
