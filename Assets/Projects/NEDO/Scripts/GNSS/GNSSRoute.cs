using UnityEngine;
using System.Collections;
using System.IO;

public class GNSSRoute : MonoBehaviour
{


	public string RouteListPath;
	private ArrayList locationList;
	private ArrayList locationListScreen;
	private string lineParameter;
	Manager mg;

	// Use this for initialization
	void Start ()
	{
		Zoom.OnChangeZoom += OnChangeZoom;	
		mg = GameObject.Find ("Manager").GetComponent<Manager> ();
		locationList = new ArrayList ();
		
		locationListScreen = new ArrayList ();
		
		using (StreamReader reader = new StreamReader(Application.dataPath + "\\"+RouteListPath)) {
			while (!reader.EndOfStream) {
				string line = reader.ReadLine ();
				
				string[] parts = line.Split (",".ToCharArray ());
				locationList.Add(new double[]{double.Parse(parts[0]),double.Parse(parts[1])});
			}
			reader.Close();
		}

		lineParameter = "&path=color:0xff0030";
		
		for (int i=0; i< locationList.Count; i++) {
			double[] d_pos = (double[])locationList [i];
			lineParameter += "|" + d_pos [0].ToString () + "," + d_pos [1].ToString ();
			double[] point = {(double)d_pos [1],  (double)d_pos [0]};
			Vector3 pos = mg.GIStoPos (point);  
			locationListScreen.Add (pos);
		}
		 #if !(UNITY_IPHONE)
		mg.sy_Map.addParameter = lineParameter;
		 #endif

	}

	void reCalCoordinate ()
	{
		locationListScreen = new ArrayList ();
		for (int i=0; i< locationList.Count; i++) {
			
			double[] d_pos = (double[])locationList [i];
			double[] invers_pos = new double[]{d_pos [1], d_pos [0]};
			Vector3 pos3 = mg.GIStoPos (invers_pos);
			
			locationListScreen.Add (pos3);
			
		}

	}

	void OnChangeZoom ()
	{

		reCalCoordinate ();
	}
	
	void Update ()
	{	
	}


}
