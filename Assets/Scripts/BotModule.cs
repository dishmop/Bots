using UnityEngine;
using System.Collections;

public class BotModule : MonoBehaviour {

	public Module module;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Rigidbody2D body = transform.parent.GetComponent<Rigidbody2D>();
		
		Vector2 vel = body.GetPointVelocity(transform.position);
		Vector3 movementDir = vel.normalized;
		float speed = vel.magnitude;
		
		// Apply ground friction 
		Vector3 groundFrictionDir = -movementDir;
		float groundFriction = module.groundFriction;
		
		if (speed < Time.deltaTime * groundFriction / (2 * body.mass)){
			groundFriction = 2 * speed * body.mass / Time.deltaTime;
		}
		body.AddForceAtPosition(groundFriction * groundFrictionDir, transform.position);
		
		// Apply air resistance
		body.AddForceAtPosition(module.airResistance * speed * groundFrictionDir, transform.position);
		
	
	}
}
