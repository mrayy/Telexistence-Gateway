using UnityEngine;
using System.Collections;

public abstract class ITeleportBehaviour : MonoBehaviour {


	public delegate void Deleg_OnEntered(ITeleportBehaviour t);
	public delegate void Deleg_OnExitted(ITeleportBehaviour t);

	public  Deleg_OnEntered OnEntered;
	public  Deleg_OnExitted OnExitted;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void OnEnter (TelubeeOVRCamera camera);
	public abstract void OnExit ();
	public abstract bool IsActive ();
}
