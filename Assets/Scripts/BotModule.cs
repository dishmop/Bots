using UnityEngine;
using System.Collections;

public class BotModule : MonoBehaviour {

	public Module module;
	public float temperature = 0;
	
	public float requestedPower;
	public float availablePower;
	public float usedPower;
	
	public bool isOverlapTriggering = true;
	bool isOverlapTriggeringThisFrame = true;
	
		
	//public float editSize= -1;

	// Use this for initialization
	virtual public void Start () {
		HandleScale();
		if (GetComponent<Renderer>() != null){
			GetComponent<Renderer>().material.EnableKeyword ("_EMISSION");
		}
		else{
			transform.FindChild("BotEngine_Model").GetComponent<Renderer>().material.EnableKeyword ("_EMISSION");
		}
	
	}
	
	virtual public void Update(){
		HandleScale();
	}
	
	public void HandleScale(){
		// use 2 x because module is radius 0.5
		if (float.IsNaN(module.volume)){
			Debug.Log ("error");
		}
		transform.localScale = 2 * Balancing.singleton.ConvertModuleVolumeToRadius(module.volume) * new Vector3(1, 1, 1);
	}
	
	public Color CalcHeatGlow(float normalisedTemp){
		float redMul = Mathf.Pow (normalisedTemp, 1);
		float greenMul = Mathf.Pow (normalisedTemp, 2);
		float blueMul = Mathf.Pow (normalisedTemp, 3);
		return new Color(redMul, greenMul, blueMul);
	}
	
	public void HandleHeat(){
		/// Calc temperature
		temperature = module.heatEnergy / (module.volume * module.GetVolumetricHeatCapacity());
		
		float normalisedTemp = temperature / module.GetMaxKelvin();
		
		Color heatGlow = CalcHeatGlow(normalisedTemp);
		if (GetComponent<Renderer>() != null){
			GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
		}
		else{
			transform.FindChild("BotEngine_Model").GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
		}
		
		// Reduce the heater according to the sruface area (line actually) of the modukle
		float heatToRemove =  0.02f * module.heatEnergy * Balancing.singleton.ConvertModuleVolumeToRadius(module.volume);
		module.heatEnergy -= heatToRemove;
	}
	
	public void PreGameUpdate(){
		requestedPower = 0;
		availablePower = 0;
		usedPower = 0;
	}
	
	// Update is called once per frame
	public virtual void GameUpdate () {
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
	
	public virtual void GameUpdatePostPowerCalc(){
	}
	
	public virtual void FixedUpdate(){
//		if (transform.parent.name == "missile_2" || transform.parent.name == "missile_1"){
//			Debug.Log("<color=blue>" + Time.fixedTime + ": " + transform.parent.name + "." + gameObject.name + ": FixedUpdate() </color>");
//		}
		isOverlapTriggering = isOverlapTriggeringThisFrame;
		isOverlapTriggeringThisFrame = false;
		
		
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		HandleTrigger(collider);
	}
	
	void OnTriggerStay2D(Collider2D collider){
		HandleTrigger(collider);
	}
	
	
	void HandleTrigger(Collider2D collider){
//		if (transform.parent.name == "missile_2" || transform.parent.name == "missile_1"){
//			Debug.Log("<color=green>" + Time.fixedTime + ": " + transform.parent.name + "." + gameObject.name + ": HandleTrigger() </color>");
//		}
		
		isOverlapTriggeringThisFrame = true;
		

	}
	
}
