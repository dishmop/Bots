using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BotBot : MonoBehaviour {
	public Bot bot;
	public Bounds bounds;
	public Dictionary<Guid, GameObject> modulesToModuleGOs = new Dictionary<Guid, GameObject>();
	public Dictionary<Guid, GameObject> modulesToRodGOs = new Dictionary<Guid, GameObject>();
	public Dictionary<Guid, CircleCollider2D> guidToCollider = new Dictionary<Guid, CircleCollider2D>();
	public bool isBotActive = false;
	public float mass;
	public Vector2 centreOfMass = Vector2.zero;
	public float momentOfInteria;
	public bool isOverlapTriggering = true;
//	bool isOverlapTriggeringThisFrame = true;
	
	float kineticEnergy;

	
	LuaBinding luaBinding;
	
	bool isBotVisible = true;
	
	public void SetBotActive(bool active){
		isBotActive = active;
	}
	
	public void RegisterCollider(Module module, CircleCollider2D circleCollider){
		guidToCollider.Add (module.guid,circleCollider);
	}
	
	public void RegisterModule(Module module, GameObject go){
		modulesToModuleGOs.Add (module.guid, go);
	}
	
	public void RegisterRod(Module module, GameObject go){
		modulesToRodGOs.Add (module.guid, go);
	}
	
	public void SetBotVisible(bool visible){
		isBotVisible = visible;
		HandleVisibility();
	}
	
	public void OnChangeRodSize(){
		Queue<Module> moduleQueue = new Queue<Module>();
		Queue<Module> parentModuleQueue = new Queue<Module>();
		Queue<int> spokeQueue = new Queue<int>();
		Queue<Vector3> posQueue = new Queue<Vector3>();
		
		moduleQueue.Enqueue(bot.rootModule);
		parentModuleQueue.Enqueue(null);
		spokeQueue.Enqueue(-1);
		posQueue.Enqueue(Vector3.zero);
		
		// Create the circles
		bot.ClearVisitedFlags();
		while (moduleQueue.Count() != 0){
			Module thisModule = moduleQueue.Dequeue();
			Module thisParentModule = parentModuleQueue.Dequeue();
			int thisSpoke = spokeQueue.Dequeue();
			Vector3 thisPos = posQueue.Dequeue();
			
			for (int i = 0;i < 6; ++i){
				if (thisModule.modules[i] != null && !thisModule.modules[i].visited){
					moduleQueue.Enqueue(thisModule.modules[i]);
					parentModuleQueue.Enqueue(thisModule);
					spokeQueue.Enqueue(i);
					posQueue.Enqueue(thisPos + bot.rodSize * SpokeDirs.GetDirVector(i));
				}
			}
			// Set the BotModule position
			modulesToModuleGOs[thisModule.guid].transform.localPosition = thisPos;
			thisModule.visited = true;
			
			// DO the rod
			if (thisSpoke != -1){
				Vector3 parentPos = modulesToModuleGOs[thisParentModule.guid].transform.localPosition;
				Vector3 rodPos = 0.5f * (parentPos + thisPos);
				Quaternion rodOrient = SpokeDirs.GetDirRotation(thisSpoke);
				GameObject rodGO = modulesToRodGOs[thisModule.guid];
				rodGO.transform.localPosition = rodPos;
				rodGO.transform.localRotation = rodOrient;
				rodGO.transform.localScale = bot.rodSize * BotFactory.singleton.botRodPrefab.transform.localScale;
			}
		}
	
	}
	
	
	void HandleVisibility(Transform thisTransform){
		if (thisTransform.GetComponent<MeshRenderer>() != null){
			thisTransform.GetComponent<MeshRenderer>().enabled = isBotVisible;
		}
		foreach (Transform child in thisTransform){
			HandleVisibility(child);
		}
		
	}


	void HandleVisibility(){
		foreach (GameObject go in modulesToModuleGOs.Values){
			HandleVisibility(go.transform);
			
		}
		foreach (GameObject go in modulesToRodGOs.Values){
			HandleVisibility(go.transform);
		}
	}
	
	// Use this for initialization
	void Start () {
		Simulation.singleton.RegisterBot(gameObject);
	
	}
	
	public void RecalcMass(){
		mass = 0;
		
		foreach(Module module in bot.guidModuleLookup.Values){
			float moduleMass = module.volume * module.GetDensity();
			mass += moduleMass;
		}
	}
	
	public void RecalcCentreOfMass(){
		centreOfMass = Vector2.zero;
		momentOfInteria = 0;
		foreach (GameObject go in modulesToModuleGOs.Values){
			Module module = go.GetComponent<BotModule>().module;
			
			Vector3 moduleCentre = go.transform.localPosition;
			float moduleMass = module.volume * module.GetDensity();
			centreOfMass += new Vector2(moduleCentre.x, moduleCentre.y) * moduleMass; 
		}
		
		centreOfMass *= 1f/ mass;
	
	}
	
	public void RecalcMomentOfInertia(){
		momentOfInteria = 0;
		foreach (GameObject go in modulesToModuleGOs.Values){
			Module module = go.GetComponent<BotModule>().module;
			Vector3 moduleCentre = go.transform.localPosition;
			float moduleMomentOfInertia = Balancing.singleton.ConvertVolumeToUnitMomentOfInertia(module.volume) * module.GetDensity();
			float moduleMass = module.volume * module.GetDensity();
			float moduleDistSq = (moduleCentre - new Vector3(centreOfMass.x, centreOfMass.y, moduleCentre.z)).sqrMagnitude;
			momentOfInteria += moduleMomentOfInertia + moduleDistSq * moduleMass;
		}
		
	}
	
	public void CreateRigidBody(){
		gameObject.AddComponent<Rigidbody2D>();
		RecalcRigidBodyParams();
		
	}
	
	void RecalcRigidBodyParams(){
		// Ensure the rigid body is set up correctly
		RecalcMass ();
		RecalcCentreOfMass();
		RecalcMomentOfInertia();
		
		GetComponent<Rigidbody2D>().mass = mass;
		GetComponent<Rigidbody2D>().centerOfMass = centreOfMass;
		GetComponent<Rigidbody2D>().inertia = momentOfInteria;
		GetComponent<Rigidbody2D>().gravityScale = 0;
		GetComponent<Rigidbody2D>().drag = 0;
		GetComponent<Rigidbody2D>().angularDrag = 5f;
		GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;	// temp test
	}
	
	// Sets all the colliders to be non-triggers
	public void SolidifyColliders(){
		SolidifyColliders(transform);
		
	}
	
	void SolidifyColliders(Transform useTransform){
		if (useTransform.GetComponent<Collider2D>() != null){
			useTransform.GetComponent<Collider2D>().isTrigger = false;
		}
		foreach (Transform child in useTransform){
			SolidifyColliders(child);
		}
		
	}
	
	
	
	public void GameUpdate(){
		if (GetComponent<Rigidbody2D>()){
			GetComponent<Rigidbody2D>().isKinematic = !isBotActive;
		}
		if (!isBotActive) return;
		
		// Thiswould be beter done on a per module basis for slingshot kindof manourvrees
		GetComponent<Rigidbody2D>().constraints = bot.enableAnchor ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
		
		RecalcRigidBodyParams();
		
		
		bot.rodSize = Mathf.Lerp (bot.rodSize, bot.CalcMinRodSize(), 0.2f);
		OnChangeRodSize();


		
		if (bot.runtimeScript != null && bot.runtimeScript != "" && luaBinding == null){
			luaBinding = new LuaBinding();
			foreach (KeyValuePair<string, object> pair in bot.luaObjectLookup){
				luaBinding.lua[pair.Key] = pair.Value;
			}
			luaBinding.lua.DoFileASync(Application.streamingAssetsPath  +"/" + bot.runtimeScript + ".lua", 1);
			//luaBinding.lua.DoFile(Application.streamingAssetsPath + "/" + bot.runtimeScript + ".lua");
		}
		
		
		if (luaBinding != null){
			if (!luaBinding.lua.isFinishedASync){
				luaBinding.lua.ResumeAsync();
			}
		}
		

		// Preupdate modules
		foreach (GameObject go in modulesToModuleGOs.Values){
			go.GetComponent<BotModule>().PreGameUpdate();
			
		}
		
		// Update modules
		foreach (GameObject go in modulesToModuleGOs.Values){
			go.GetComponent<BotModule>().GameUpdate();
			
		}
		
		// Do the fuel usage calculation

		// First work out how much power has been requested
		float powerRequirements = 0;
		foreach(GameObject go in modulesToModuleGOs.Values){
			BotModule botModule = go.GetComponent<BotModule>();
			
			powerRequirements += botModule.requestedPower;
		}
		float fuelRequestedThisFrame = powerRequirements * Time.fixedDeltaTime;
		
		// See how mcuh fuel we have in total
		float totalFuelAvailable = 0;
		foreach(Module module in bot.guidModuleLookup.Values){
			if (module.enableConsumable){
				totalFuelAvailable += module.volume * module.GetEnergyDensity();
			}
		}
		
		// What proportion of the fuel requested can we supply?
		float availableFuelRequested = Mathf.Min (totalFuelAvailable, fuelRequestedThisFrame);
		float proportionAvailable = fuelRequestedThisFrame == 0 ? 1 : (availableFuelRequested / fuelRequestedThisFrame);
		
		

		
		// Inform each module how much power is available to them
		foreach(GameObject go in modulesToModuleGOs.Values){
			BotModule botModule = go.GetComponent<BotModule>();
			
			botModule.availablePower = botModule.requestedPower * proportionAvailable;
		}
		
		
		// Now update each module according to how much fuel is available
		foreach (GameObject go in modulesToModuleGOs.Values){
			go.GetComponent<BotModule>().GameUpdatePostPowerCalc();
			
		}
		
		// How much fuel has actually been used
		float totalPowerUsed = 0;
		foreach(GameObject go in modulesToModuleGOs.Values){
			BotModule botModule = go.GetComponent<BotModule>();
			
			totalPowerUsed += botModule.usedPower;
		}
		
		float totalFuelUsed = totalPowerUsed * Time.fixedDeltaTime;
		
		if (totalFuelAvailable != 0){
			float fuelProp = (totalFuelAvailable - totalFuelUsed) / totalFuelAvailable;
		
		
			// Reduce the size of the consumable modules accordingly
			foreach(Module module in bot.guidModuleLookup.Values){
				if (module.enableConsumable){
					module.volume *= fuelProp;
				}
			}
		}
		


		
		//Debug.Log(Time.fixedTime + ": FixedUpdate, speed = " + GetComponent<Rigidbody2D>().velocity.magnitude);
	}
	
	void FixedUpdate(){
		if (GetComponent<Rigidbody2D>() != null){
			kineticEnergy = CalcKineticEngergy();
		}
		DebugDrawBounds();
		
		// If we survived since last fixd update without getting any overlap triggers - then we have no overlaps
		isOverlapTriggering = false;
		
		foreach (GameObject go in modulesToModuleGOs.Values){
			BotModule botModule = go.GetComponent<BotModule>();
			isOverlapTriggering = isOverlapTriggering || botModule.isOverlapTriggering;
		}
		
	//	Debug.Log (Time.fixedTime + ": " + gameObject.name + ": isOverlapTriggering = " + isOverlapTriggering);
		
	}
	
	// Debug version
	float CalcKineticEngergy(out float linSpeed, out float angVel){
		linSpeed = GetComponent<Rigidbody2D>().GetRelativePointVelocity(GetComponent<Rigidbody2D>().centerOfMass).magnitude;
		float linearKineticEnergy = 0.5f * GetComponent<Rigidbody2D>().mass * linSpeed * linSpeed;
		angVel = Mathf.Deg2Rad * GetComponent<Rigidbody2D>().angularVelocity;
		float rotationalKinecticEnergy = 0.5f * GetComponent<Rigidbody2D>().inertia * angVel * angVel;
		return linearKineticEnergy + rotationalKinecticEnergy;
	}
	
	float CalcKineticEngergy(){
		float linSpeed;
		float angVel;
		linSpeed = GetComponent<Rigidbody2D>().GetRelativePointVelocity(GetComponent<Rigidbody2D>().centerOfMass).magnitude;
		float linearKineticEnergy = 0.5f * GetComponent<Rigidbody2D>().mass * linSpeed * linSpeed;
		angVel = Mathf.Deg2Rad * GetComponent<Rigidbody2D>().angularVelocity;
		float rotationalKinecticEnergy = 0.5f * GetComponent<Rigidbody2D>().inertia * angVel * angVel;
		return linearKineticEnergy + rotationalKinecticEnergy;
	}
	
	
	// Degbug. draw bounds
	void DebugDrawBounds(){
		float zVal= -1;
		
		Vector3 bottomLeft = bounds.min;
		Vector3 topRight = bounds.max;
		Vector3 topLeft = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
		Vector3 bottomRight = new Vector3(topRight.x, bottomLeft.y, topRight.z);
		
		bottomLeft.z = zVal;
		topRight.z = zVal;
		topLeft.z = zVal;
		bottomRight.z = zVal;
		
		Debug.DrawLine (transform.TransformPoint(bottomLeft), transform.TransformPoint(topLeft), Color.blue);
		Debug.DrawLine (transform.TransformPoint(topLeft), transform.TransformPoint(topRight), Color.blue);
		Debug.DrawLine (transform.TransformPoint(topRight), transform.TransformPoint(bottomRight), Color.blue);
		Debug.DrawLine (transform.TransformPoint(bottomRight), transform.TransformPoint(bottomLeft), Color.blue);
	}
	
//	void OnTriggerEnter2D(Collider2D collider){
//		HandleTrigger(collider);
//	}
//	
//	void OnTriggerStay2D(Collider2D collider){
//		HandleTrigger(collider);
//	}
	
	void OnCollisionEnter2D(Collision2D collision){
		HandleCollision(collision);
	}
	
	void OnCollisionStay2D(Collision2D collision){
		HandleCollision(collision);
	}

	
	
//	
//	void HandleTrigger(Collider2D collider){
//		isOverlapTriggeringThisFrame = true;
//
//		if (gameObject.name == "missile_2"){
//			Debug.Log ("HandleTrigger - overlappingName = " + collider.gameObject.name);
//			
//		}
//	}
	
	
	// NOte - we may want to register colliisons continuous for (if we have friction) sliding along a wall
	// and creating sparks and heat and noise!
	void HandleCollision(Collision2D collision){
		float newLinSpeed;
		float newAngVel;
		float newKineticEnergy = CalcKineticEngergy(out newLinSpeed, out newAngVel);
		float kineticEnergyDelta = kineticEnergy - newKineticEnergy;
		BotModule otherModule = collision.contacts[0].collider.gameObject.GetComponent<BotModule>();
		BotModule thisModule = collision.contacts[0].otherCollider.gameObject.GetComponent<BotModule>();
		
		if (collision.contacts.Count() > 1){
			Debug.Log ("Multiple contact poiints in collision not handled properly");
		}
		
		//Debug.Log(Time.fixedTime + ": Collision with " + collision.collider.gameObject.name + ", Loss in energy = " + kineticEnergyDelta + ", old linSpeed = " + tempLineSpeed + ", old AngVel = " + tempAngVel + ", newLinSpeed = " + newLinSpeed + ", new angVel = " + newAngVel);
		
		// If we collided with another module, spread our heat between us and them
		if (otherModule != null){
			thisModule.module.heatEnergy += 0.5f * kineticEnergyDelta;
			otherModule.module.heatEnergy += 0.5f * kineticEnergyDelta;
		}
		else{
			thisModule.module.heatEnergy += kineticEnergyDelta;
		}
		kineticEnergy = newKineticEnergy;
	}
	

	
	void OnDestroy(){
		if (Simulation.singleton != null) Simulation.singleton.UnregisterBot(gameObject);
	}
}
