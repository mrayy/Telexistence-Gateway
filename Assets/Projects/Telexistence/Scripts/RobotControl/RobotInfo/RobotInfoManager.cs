using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class IRobotInfoManagerListener
{

    public virtual void OnRobotAdded(RobotInfoManager mngr, RobotInfo ifo) { }
	public virtual void OnRobotStatusUpdated(RobotInfoManager mngr,RobotInfo ifo){}
};
public class RobotInfoManager  {

    protected List<RobotInfo> _robots=new List<RobotInfo>();

    public delegate void RobotDelegate(RobotInfoManager m, RobotInfo r);
    public event RobotDelegate OnRobotAdded;
    public event RobotDelegate OnRobotUpdated;

    public void AddRobotInfo(RobotInfo ifo)
    {
        _robots.Add(ifo);

        if (OnRobotAdded != null)
            OnRobotAdded(this, ifo);
    }
    public RobotInfo GetRobotInfo(int index)
    {
        return _robots[index];
    }
    public RobotInfo GetRobotInfoByID(int id)
    {
        foreach (RobotInfo i in _robots)
        {
            if (i.ID == id)
                return i;
        }
        return null;
    }
    public List<RobotInfo> GetRobots()
    {
        return _robots;
    }
    public void ClearRobots()
    {
        _robots.Clear();
    }

	public void LoadRobots(string path)
    {
		XmlReader reader=XmlReader.Create(path); 
		if (reader == null) {
			LogSystem.Instance.Log("RobotInfoManager::LoadRobots()- Failed to load Robots File:"+path,LogSystem.LogType.Error);
			return;
		}
		long ID = 0;
		while (reader.Read()) {
			if(reader.Name=="Robot")
			{
				RobotInfo r=new RobotInfo();
				r.LoadXML(reader);
				r.ID=ID++;
				_robots.Add(r);
			}
		}
    }
}
