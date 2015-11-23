using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class PointCloudDataRenderer : MonoBehaviour {
	public string FileName;
	public string TagName;
	public Color PointsColor;

	// Use this for initialization
	void Start () {


		MeshFilter mf = GetComponent<MeshFilter> ();
		if (mf.mesh == null) {
			mf.mesh=new Mesh();
		}
		Mesh mesh = mf.mesh;

		int vertCount;
		List<Vector3> positions=new List<Vector3>();
		List<Color> colors = new List<Color> ();

		using (StreamReader reader = new StreamReader(FileName))
		{
			while(!reader.EndOfStream){
				string line = reader.ReadLine();

				string[] parts=line.Split("\t".ToCharArray());

				if(parts[0]!=TagName)
					continue;

				positions.Add(new Vector3(float.Parse(parts[1]),float.Parse(parts[2]),float.Parse(parts[3])));

				Vector3 clr=new Vector3(float.Parse(parts[4]),float.Parse(parts[5]),float.Parse(parts[6]));
				colors.Add(new Color((clr.x/180.0f) + 0.5f,(clr.y/180.0f) + 0.5f,(clr.z/180.0f) + 0.5f,1));
			}
		}

		vertCount = positions.Count;
		int[] indecies = new int[vertCount];
		for(int i=0;i<vertCount;++i)
		{
			indecies[i]=i;
			colors[i]=PointsColor;//new Color(1,1,1,1);
		}
		mesh.vertices = positions.ToArray();
		mesh.colors = colors.ToArray();
		mesh.SetIndices(indecies,MeshTopology.Points, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
