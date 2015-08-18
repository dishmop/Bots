using UnityEngine;
using System.Collections;

public class BotModule : MonoBehaviour {

	public Module module;
	public float temperature = 0;
	
	//public float editSize= -1;

	// Use this for initialization
	void Start () {
		HandleScale();
		if (GetComponent<Renderer>() != null){
			GetComponent<Renderer>().material.EnableKeyword ("_EMISSION");
		}
		else{
			transform.FindChild("BotEngine_Model").GetComponent<Renderer>().material.EnableKeyword ("_EMISSION");
		}
	
	}
	
	void Update(){
		HandleScale();
	}
	
	public void HandleScale(){
		// use 2 x because module is radius 0.5
		transform.localScale = 2 * Balancing.singleton.ConvertModuleSizeToRadius(module.volume) * new Vector3(1, 1, 1);
	}
	
	public void HandleHeat(){
		/// Calc temperature
		temperature = module.heatEnergy / (module.volume * module.GetVolumetricHeatCapacity());
		
		float normalisedTemp = temperature / module.GetMaxKelvin();
		
		Color heatGlow = Color.Lerp(Color.black, Color.red, normalisedTemp);
		if (GetComponent<Renderer>() != null){
			GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
		}
		else{
			transform.FindChild("BotEngine_Model").GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		

		HandleHeat();
		
		
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
