using UnityEngine;
using System.Collections;

public class BotModule : MonoBehaviour {

	public Module module;
	public float temperature = 0;
	
	public float requestedPower;
	public float availablePower;
	public float usedPower;
	
	public float availableGiftedFuel;
	public float acceptableGiftedFuel;
	
	public bool toBeDestroyed = false;
	
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
			transform.FindChild("Model").GetComponent<Renderer>().material.EnableKeyword ("_EMISSION");
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
	
	public virtual void HandleHeat(){
		/// Calc temperature
		temperature = Balancing.singleton.heatToTempMul * module.heatEnergy / (module.volume * module.GetVolumetricHeatCapacity());
		
		
		// Set temperature visualisation to be distributed around circumference - because this is the energy that must get transfered
		// into the aether
		//float normalisedTemp = 200f * temperature * Balancing.singleton.ConvertModuleVolumeToRadius(module.volume) / module.GetMaxKelvin();
		
		float normalisedTemp = temperature / module.GetMaxKelvin();
		
		// Multiply temp by circumference (t get total energy givenout) then divide by area (as this is how much energy will get inputed to the field)
		float surfaceRadiation = Balancing.singleton.heatToTempMulSurf * normalisedTemp / Balancing.singleton.ConvertModuleVolumeToRadius(module.volume) ;
		
		Color heatGlow = CalcHeatGlow(normalisedTemp);
		if (GetComponent<Renderer>() != null){
			GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
			Color col = GetComponent<Renderer>().material.GetColor("_Color");
			col.a = Balancing.singleton.SurfToFieldMul * surfaceRadiation;
			GetComponent<Renderer>().material.SetColor("_Color", col);
		}
		else{
			transform.FindChild("Model").GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
			
			Color col = transform.FindChild("Model").GetComponent<Renderer>().material.GetColor("_Color");
			col.a = Balancing.singleton.SurfToFieldMul * surfaceRadiation;
			transform.FindChild("Model").GetComponent<Renderer>().material.SetColor("_Color", col);
		}
		
		float testTemperature = Balancing.singleton.heatToTempMul * module.heatEnergy / (module.volume * module.GetVolumetricHeatCapacity());
		
		if (testTemperature > 0.95 * module.GetMaxKelvin()){
			toBeDestroyed = true;
		}

		
		// Reduce the heater according to the sruface area (line actually) of the modukle
		float heatToRemove =  Balancing.singleton.heatToRemoveProp * surfaceRadiation * Balancing.singleton.ConvertModuleVolumeToRadius(module.volume);
		module.heatEnergy -= heatToRemove;
	}
	
	public void DestroyModule(){
		GameObject explosion = GameObject.Instantiate(BotFactory.singleton.botExplosion);
		explosion.transform.position = transform.position;
		explosion.GetComponent<BotExplosion>().Trigger(module.volume);
		transform.parent.GetComponent<BotBot>().DestroyModule(gameObject);
	}
	
	// In most cases this just ups the temperature
	public virtual void HandleRadiation(){
		Texture2D texture = AetherSimCamera.singleton.resultPicture;
		
		// Which transform to use?
		Transform useTransform = transform.FindChild("Model") != null ? transform.FindChild("Model") : transform;
		
		int numSamplePoints = (int)( Balancing.singleton.ConvertModuleVolumeToRadius(module.volume) * 16);
		float localRad = 0.51f;
		float heat = 0;
		for (int i = 0; i < numSamplePoints; ++i){
			Vector3 samplePosLocal = new Vector3(localRad * Mathf.Sin(i * 2 * Mathf.PI / numSamplePoints), localRad * Mathf.Cos(i * 2 * Mathf.PI / numSamplePoints), 0);
			Vector3 samplePosWorld = useTransform.TransformPoint(samplePosLocal);
			// Convert to texture space
			Vector3 samplePosQuad = AetherSimCamera.singleton.finalQuad.transform.InverseTransformPoint(samplePosWorld) + new Vector3(0.5f, 0.5f, 0 );
			int xPos = (int)(samplePosQuad.x * texture.width);
			int yPos = (int)(samplePosQuad.y * texture.height);
//			texture.SetPixel(xPos, yPos, Color.blue);
			heat += texture.GetPixel(xPos, yPos).r;
			//Debug.Log ("xPos = " + xPos + " + yPos = " + yPos);
			
		}
		
		ApplyRadiation(heat);
		
		
		
	
	}
	
	public virtual void ApplyRadiation(float energy){
		module.heatEnergy += energy * Balancing.singleton.heatFromFieldMul;
	}
	
	public void PreGameUpdate(){
		requestedPower = 0;
		availablePower = 0;
		usedPower = 0;
		availableGiftedFuel = 0;
		acceptableGiftedFuel = 0;
	}
	
	// Update is called once per frame
	public virtual void GameUpdate () {
		
		if (GetComponent<Collider2D>().enabled != transform.parent.GetComponent<BotBot>().isBotActive){
			GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		}
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		

		HandleRadiation();
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
