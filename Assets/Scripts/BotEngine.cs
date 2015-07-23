using UnityEngine;
using System.Collections;

public class BotEngine : MonoBehaviour {
	public Engine engine;
	public float readPower;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		readPower = engine.power * 10;
		Vector3 force = transform.rotation * new Vector3(0, readPower);
		transform.parent.GetComponent<Rigidbody2D>().AddForceAtPosition(force, transform.position);
	
	}
}
