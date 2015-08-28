using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BotBot : MonoBehaviour {
	public Bot bot;
	public Bounds bounds;
	public Dictionary<Guid, GameObject> modulesToModuleGOs = new Dictionary<Guid, GameObject>();
	public Dictionary<Guid, GameObject[]> modulesToRodGOs = new Dictionary<Guid, GameObject[]>();
	public Dictionary<Guid, CircleCollider2D> guidToCollider = new Dictionary<Guid, CircleCollider2D>();
	public bool isBotActive = false;
	public float mass;
	public Vector2 centreOfMass = Vector2.zero;
	public float momentOfInteria;
	public bool isOverlapTriggering = true;
//	bool isOverlapTriggeringThisFrame = true;
	
	float kineticEnergy;
	public bool toBeDestroyed = false;
	

	
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
	
	public void RemoveRod(Module module, int spokeDir){
		Module otherModule = module.modules[spokeDir];
		int otherSpokeDir = SpokeDirs.CalcInverseSpoke(spokeDir);

		GameObject[] goArray = modulesToRodGOs[module.guid];
		GameObject[] otherGoArray = modulesToRodGOs[otherModule.guid];
		
		goArray[spokeDir] = null;
		otherGoArray[otherSpokeDir] = null;
		
		// If we've still got rods associated with this module, then keep the array hanging around - otherwise, remove it
		bool isEmpty = true;
		for (int i = 0; i < 6; ++i){
			if (goArray[i] != null){
				isEmpty = false;
				break;
			}
		}

		bool isOtherEmpty = true;
		for (int i = 0; i < 6; ++i){
			if (otherGoArray[i] != null){
				isOtherEmpty = false;
				break;
			}
		}
		
		if (isEmpty) modulesToRodGOs.Remove (module.guid);
		if (isOtherEmpty) modulesToRodGOs.Remove (otherModule.guid);
		
	}
	
	public void RegisterRod(Module module, GameObject go, int spokeDir){
		// Need to find the other module and register us there too
		Module otherModule = module.modules[spokeDir];
		int otherSpokeDir = SpokeDirs.CalcInverseSpoke(spokeDir);
		
		
		if (!modulesToRodGOs.ContainsKey(module.guid)){
			modulesToRodGOs.Add (module.guid, new GameObject[6]);	
		}
		if (!modulesToRodGOs.ContainsKey(otherModule.guid)){
			modulesToRodGOs.Add (otherModule.guid, new GameObject[6]);	
		}
		
		modulesToRodGOs[module.guid][spokeDir] = go;
		modulesToRodGOs[otherModule.guid][otherSpokeDir] = go;
		
		
		
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
			if (!modulesToModuleGOs.ContainsKey(thisModule.guid)){
				Debug.Log("Error!");
			}
			modulesToModuleGOs[thisModule.guid].transform.localPosition = thisPos;
			thisModule.visited = true;
			
			// DO the rod
			if (thisSpoke != -1){
				Vector3 parentPos = modulesToModuleGOs[thisParentModule.guid].transform.localPosition;
				Vector3 rodPos = 0.5f * (parentPos + thisPos);
				Quaternion rodOrient = SpokeDirs.GetDirRotation(thisSpoke);
				
				// THIS IS ALL A BIT DODGY - NOT SURE WHICH MODULE I SHOULD BE USING
				GameObject rodGO = modulesToRodGOs[thisParentModule.guid][thisSpoke];
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
		foreach (GameObject[] goArray in modulesToRodGOs.Values){
			for (int i = 0; i < 3; ++i){
				if (goArray[i] != null){
					HandleVisibility(goArray[i].transform);
				}
			}
			
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
		GetComponent<Rigidbody2D>().angularDrag = 5;//1f;
		GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.StartAwake;	// Perhaps need to never sleep?
	}
	
	// Sets all the colliders to be non-triggers
	public void SolidifyColliders(){
		SolidifyColliders(transform);
		
	}
	
	public void DestroyModule(GameObject botModuleGO){
		Module module = botModuleGO.GetComponent<BotModule>().module;
		Bot oldBot = module.bot;
		
		// 
		
		// We are going to need to traverse all the modules here
		oldBot.ClearVisitedFlags();
		
		
		// For each neighbour of the module we are destroying, we need to construct a new bot
		int fragCount = 0;
		for (int i = 0; i < module.modules.Count (); ++i){
			Module otherModule = module.modules[i];
			if (otherModule != null){
			
				// Remove the rod associated with this other module as it should not longer exist
				GameObject otherRodGO = modulesToRodGOs[module.guid][i];
				RemoveRod(module, i);
				GameObject.Destroy (otherRodGO);
				
				// Remove the connection
				module.modules[i] = null;
				otherModule.modules[SpokeDirs.CalcInverseSpoke(i)] = null;
				
				
				
				// Construct a new bot with this other module at its root
				Bot newBot = new Bot(gameObject.name + "_" + fragCount++);
				newBot.rootModule = otherModule;
				newBot.rodSize = oldBot.rodSize;
				newBot.enableAnchor = oldBot.enableAnchor;
				newBot.guidModuleLookup.Add (otherModule.guid, otherModule);
				

				
				// Construct the GameObject to house the new bot
				GameObject newBotBotGO = GameObject.Instantiate(BotFactory.singleton.botBotPrefab);
				BotBot newBotBot = newBotBotGO.GetComponent<BotBot>();
				newBotBot.bot = newBot;
				newBotBot.bounds = bounds;
				newBotBotGO.name = newBot.name;
				
				// Get the gameobject associated with this "other" module and parent it to the new bot GO
				GameObject otherBotModuleGO = modulesToModuleGOs[otherModule.guid];
				modulesToModuleGOs.Remove(otherModule.guid);
				newBotBot.modulesToModuleGOs.Add(otherModule.guid, otherBotModuleGO);
				newBotBotGO.transform.rotation = otherBotModuleGO.transform.rotation;
				newBotBotGO.transform.position = otherBotModuleGO.transform.position;
				otherBotModuleGO.transform.SetParent(newBotBotGO.transform);

								
				
				otherModule.bot = newBot;
				otherModule.visited = true;
				
				// Now follow the tree from this other module adding all the 
				// modules, BotModules and Rods that we find into our new bot
				Queue<Module> moduleQueue = new Queue<Module>();
				Queue<int> spokeQueue = new Queue<int>();
				
				for (int j = 0; j < 6; ++j){
					Module otherOtherModule = otherModule.modules[j];
					if (otherOtherModule != null && !otherOtherModule.visited){
						moduleQueue.Enqueue(otherOtherModule);
						spokeQueue.Enqueue(SpokeDirs.CalcInverseSpoke(j));
					}
				}
				
				while (moduleQueue.Count () != 0){
					Module otherModule2 = moduleQueue.Dequeue();
					int otherSpoke2 = spokeQueue.Dequeue();
					GameObject otherBotModuleGO2 = modulesToModuleGOs[otherModule2.guid];
					GameObject otherRodGO2 = modulesToRodGOs[otherModule2.guid][otherSpoke2];
					
					// Add this one's children to the queue
					for (int j = 0; j < 6; ++j){
						Module otherOtherModule = otherModule2.modules[j];
						if (otherOtherModule != null && !otherOtherModule.visited){
							moduleQueue.Enqueue(otherOtherModule);
							spokeQueue.Enqueue(SpokeDirs.CalcInverseSpoke(j));
						}
					}
					
					// Deal with our module
					otherModule2.bot = newBot;
					newBot.guidModuleLookup.Add (otherModule2.guid, otherModule2);
					oldBot.guidModuleLookup.Remove (otherModule2.guid);
					
					// Deal with the BotModuleGO
					otherBotModuleGO2.transform.SetParent(newBotBotGO.transform);
					otherRodGO2.transform.SetParent(newBotBotGO.transform);
					
					newBotBot.modulesToModuleGOs.Add (otherModule2.guid, otherBotModuleGO2);
					newBotBot.RegisterRod (otherModule2, otherRodGO2, otherSpoke2);
					
					// Remove from this bot
					modulesToModuleGOs.Remove (otherModule2.guid);
					modulesToRodGOs.Remove (otherModule2.guid);
					
					otherModule.visited = true;
					
					
				}
				
				newBotBot.RecalcMass();
				
				newBotBot.CreateRigidBody();
				if (GetComponent<Rigidbody2D>().constraints != RigidbodyConstraints2D.FreezeAll){
					newBotBotGO.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().GetPointVelocity(newBotBotGO.transform.position);
					newBotBotGO.GetComponent<Rigidbody2D>().angularVelocity = GetComponent<Rigidbody2D>().angularVelocity;
				}
				
				newBotBot.SetBotActive(true);
			}
		}
		// Remove the module game object
		modulesToModuleGOs.Remove(module.guid);
		GameObject.Destroy (botModuleGO);
		
		// Finally deestor this bot - as it is no longer needed - first check if all our lists and maps are empty
		DebugUtils.Assert(oldBot.guidModuleLookup.Count () == 0, "Bot guidModuleLookup lookup not empty!");
		
		DebugUtils.Assert(modulesToModuleGOs.Count () == 0, "BotBot modulesToModuleGOs lookup not empty!");
		DebugUtils.Assert(modulesToRodGOs.Count () == 0, "BotBot modulesToRodGOs lookup not empty!");
		
		toBeDestroyed = true;
			
		
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
				totalFuelAvailable += Mathf.Max (module.volume - 0.1f, 0f) * module.GetEnergyDensity();
			}
		}
		
		float totalGiftedFuel = 0;
		foreach(GameObject go in modulesToModuleGOs.Values){
			BotModule botModule = go.GetComponent<BotModule>();
			
			totalGiftedFuel += botModule.availableGiftedFuel;
		}

		
		// What proportion of the fuel requested can we supply?
		float availableFuelRequested = Mathf.Min (totalFuelAvailable + totalGiftedFuel, fuelRequestedThisFrame);
		
		
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
		
		// First deduct what we have used from the gifted fuel
		float giftedFuelUsed = Mathf.Min(totalGiftedFuel, totalFuelUsed);
		
		float giftedFuelRemaining = totalGiftedFuel - giftedFuelUsed;
		
		// If we have some batteries we can attempt to recharge them
		int numFuelCells = 0;
		foreach (GameObject go in modulesToModuleGOs.Values){
			BotFuelCell botFuelCell = go.GetComponent<BotFuelCell>();
			if (botFuelCell != null){
				numFuelCells++;
			}
		}
		
		// Probably need somethiung in here to ask if the cell can recharge this amount
		float fuelToAddPerCell = giftedFuelRemaining / numFuelCells;
		float giftedFuelAccepted = 0;
		foreach (GameObject go in modulesToModuleGOs.Values){
			BotFuelCell botFuelCell = go.GetComponent<BotFuelCell>();
			if (botFuelCell != null){
				float currentFuel = botFuelCell.module.volume * botFuelCell.module.GetEnergyDensity();
				float newFuel = currentFuel + fuelToAddPerCell;
				float prop = newFuel / currentFuel;
				botFuelCell.module.volume *= prop;
				
				float newFuelTest = botFuelCell.module.volume * botFuelCell.module.GetEnergyDensity();
				giftedFuelAccepted += fuelToAddPerCell;
			}
		}
		
		// INform each moduel that attempted to gift some fuel how much we could accept
		float propGiftedFuelAccepted = (giftedFuelAccepted == 0) ? 0 : (giftedFuelAccepted / totalGiftedFuel);
		
		foreach(GameObject go in modulesToModuleGOs.Values){
			BotModule botModule = go.GetComponent<BotModule>();
			
			botModule.acceptableGiftedFuel = botModule.availableGiftedFuel * propGiftedFuelAccepted;
		}
		
		
		if (totalFuelAvailable != 0){
		
			float fuelProp = (totalFuelAvailable - totalFuelUsed) / totalFuelAvailable;
		
		
			// Reduce the size of the consumable modules accordingly
			foreach(Module module in bot.guidModuleLookup.Values){
				if (module.enableConsumable){
					module.volume *= fuelProp;
				}
			}
		}
		
		// Chec for any modules that need to be destoryed
		List<BotModule> modulesToDestroy = new List<BotModule>();
		
		foreach (GameObject go in modulesToModuleGOs.Values){
			
			if (go.GetComponent<BotModule>().toBeDestroyed){
				modulesToDestroy.Add (go.GetComponent<BotModule>());
			}
		}
		if (modulesToDestroy.Count() > 1){
			Debug.LogError ("Can't handle multiple modules being destoryed just yet");
		}
		
		if (modulesToDestroy.Count() == 1){
			modulesToDestroy[0].DestroyModule();
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
			thisModule.module.heatEnergy += 4 * 0.5f * kineticEnergyDelta;
			otherModule.module.heatEnergy += 4 * 0.5f * kineticEnergyDelta;
		}
		else{
			thisModule.module.heatEnergy += 4 * kineticEnergyDelta;
		}
		kineticEnergy = newKineticEnergy;
	}
	

	
	void OnDestroy(){
		if (Simulation.singleton != null) Simulation.singleton.UnregisterBot(gameObject);
	}
}
