using UnityEngine;
using System.Collections;

public class BotEngine : MonoBehaviour {
	public Engine engine;
	public float tempPower = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 force = transform.rotation * new Vector3(0, tempPower);
		transform.parent.GetComponent<Rigidbody2D>().AddForceAtPosition(force, transform.position);
	
	}
}
