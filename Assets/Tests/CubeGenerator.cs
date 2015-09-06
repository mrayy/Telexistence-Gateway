using UnityEngine;
using System.Collections;

public class CubeGenerator : MonoBehaviour {

	public PrimitiveType Primitive;
	public int CountPerEdge;
	public float Spacing;
	// Use this for initialization
	void Start () {
		GameObject baseCube=GameObject.CreatePrimitive(Primitive);

		for (int i=-CountPerEdge/2; i<=CountPerEdge/2; ++i)
			for (int j=-CountPerEdge/2; j<=CountPerEdge/2; ++j)
				for (int k=-CountPerEdge/2; k<=CountPerEdge/2; ++k) {
					Vector3 pos=new Vector3(Spacing*i,Spacing*j,Spacing*k);

					GameObject c=(GameObject)Instantiate(baseCube,pos,Quaternion.Euler(new Vector3(Random.Range(0.0f,180.0f),Random.Range(0.0f,180.0f),Random.Range(0.0f,180.0f))));
					c.transform.parent=this.transform;
				}

		GameObject.Destroy (baseCube);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
