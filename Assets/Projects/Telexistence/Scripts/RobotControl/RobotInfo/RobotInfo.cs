using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

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
		string v=r.GetAttribute ("Longitude");
		if (v != null && v != "")
			lng = double.Parse (v);
		
		v=r.GetAttribute ("Latitude");
		if (v != null && v != "")
			lat = double.Parse (v);
	}

	public void Read(BinaryReader reader)
	{
		ID=reader.ReadInt32();
		Name=reader.ReadStringNative();
		IP = reader.ReadStringNative();
		Location = reader.ReadStringNative();
		lng=reader.ReadSingle();
		lat=reader.ReadSingle();
		Connected=reader.ReadBoolean();
		Available=reader.ReadBoolean();
	}
}
