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
		GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		readPower = 50 * engine.GetPowerRequirements();
		Vector3 force = transform.rotation * new Vector3(0, readPower);
		transform.parent.GetComponent<Rigidbody2D>().AddForceAtPosition(force, transform.position);
		
		engine.powerMultiplied = Mathf.Lerp (engine.powerMultiplied, 1, 0.001f);
	
	}
}
