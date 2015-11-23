using UnityEngine;
using System.Collections;

public class HydraulictTranslate : MonoBehaviour {
	
	public Transform From;
	public Transform To;
	public float length;

	float initial;
	// Use this for initialization
	void Start () {

		initial = transform.localPosition.z;
		length = (From.position - To.position).magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 dir = (From.position - To.position);

		float len = dir.magnitude;
		dir.Normalize ();

		float diff = Mathf.Max(0, len-length);

		transform.localPosition = new Vector3 (0, 0, diff+initial);

	}
}
