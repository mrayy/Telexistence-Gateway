using UnityEngine;
using System.Collections;
using System.Xml;

public class RobotInfo  {

    public long ID;
    public string Name;
    public string IP;
    public string Location;
    public double lng;
    public double lat;

    public bool Connected;
    public bool Available;

	public void LoadXML(XmlReader r)
	{
		Name=r.GetAttribute ("Name");
		IP=r.GetAttribute ("IP");
		Location=r.GetAttribute ("Location");
		string v=r.GetAttribute ("longitude");
		if (v != null && v != "")
			lng = double.Parse (v);
		
		v=r.GetAttribute ("Latitude");
		if (v != null && v != "")
			lat = double.Parse (v);
	}
}
