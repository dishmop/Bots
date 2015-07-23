using UnityEngine;
using System.Collections;

public class BotModule : MonoBehaviour {

	public Module module;
	public float editSize= -1;

	// Use this for initialization
	void Start () {
		HandleScale();
	
	}
	
	void Update(){
		HandleScale();
	}
	
	public void HandleScale(){
		// use 2 x because moduleis radisu 0.5
		transform.localScale = 2 * Mathf.Sqrt (module.size) * new Vector3(1, 1, 1);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		
		if (editSize < 0){
			editSize = module.size;
		}
		else{
			module.size = editSize;
		
		}
		    
		
		Rigidbody2D body = transform.parent.GetComponent<Rigidbody2D>();
		
		if (body.constraints == RigidbodyConstraints2D.FreezeAll) return;
		
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
