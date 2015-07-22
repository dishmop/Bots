using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BotFactory : MonoBehaviour {
	public static BotFactory singleton = null;
	
	public GameObject botEnginePrefab;
	public GameObject botFuelCellPrefab;
	public GameObject botConstructorPrefab;
	public GameObject botBotPrefab;
	public GameObject botRodPrefab;
	public GameObject generateEffectSpherePrefab;
	public GameObject generateEffectRodPrefab;
	
	
	
	public GameObject ConstructBotBot(Bot bot){
	
		GameObject newBotBotGO = GameObject.Instantiate(botBotPrefab);
		BotBot newBotBot = newBotBotGO.GetComponent<BotBot>();
		newBotBot.bot = bot;
		
		Queue<Module> moduleQueue = new Queue<Module>();
		Queue<int> spokeToParent = new Queue<int>();
		Queue<Vector3> modulePositions = new Queue<Vector3>();
		moduleQueue.Enqueue(bot.rootModule);
		spokeToParent.Enqueue(-1);
		modulePositions.Enqueue(Vector3.zero);
		
		bot.ClearVisitedFlags();
		while (moduleQueue.Count() != 0){
			Module thisModule = moduleQueue.Dequeue();
			int thisSpokeToParent = spokeToParent.Dequeue();
			Vector3 thisPos = modulePositions.Dequeue();
			
			for (int i = 0; i < 6; ++i){
				if (thisModule.modules[i] != null && !thisModule.modules[i].visited){
					moduleQueue.Enqueue(thisModule.modules[i]);
					modulePositions.Enqueue(thisPos + SpokeDirs.GetDirVector(i) * bot.rodSize);
					spokeToParent.Enqueue(SpokeDirs.CalcInverseSpoke(i));
				}
			}
			// Make the module Game Object
			GameObject newModuleGO = ConstructBotModule(thisModule);
			newModuleGO.transform.SetParent(newBotBotGO.transform);
			newModuleGO.transform.localPosition = thisPos;
			newModuleGO.transform.localScale = new Vector3(1f, 1f, 1f);
			newBotBot.RegisterModule(thisModule, newModuleGO);
			
			// Make the rod conecting them (if there is one)
			if (thisSpokeToParent != -1){
				Vector3 dirToParent = SpokeDirs.GetDirVector(thisSpokeToParent) * bot.rodSize;
				Vector3 parentPos = thisPos + dirToParent;
				Vector3 rodPos = 0.5f * (thisPos + parentPos);
				rodPos.z = botRodPrefab.transform.position.z;
				Quaternion rodRotation = SpokeDirs.GetDirRotation(thisSpokeToParent);
				GameObject newRod = GameObject.Instantiate(botRodPrefab);
				newRod.transform.SetParent(newBotBotGO.transform);
				newRod.transform.position = rodPos;
				newRod.transform.rotation = rodRotation;
				newBotBot.RegisterRod(thisModule, newRod);
				
			
			}
			
			thisModule.visited = true;
		}

		
		// Work out its bounds
		Bounds bounds = new Bounds();
		foreach (Transform child in newBotBot.transform){
			Renderer renderer = child.GetComponent<Renderer>();
			if (renderer != null){
				bounds.Encapsulate(renderer.bounds);
			}
		}
		newBotBot.bounds = bounds;
		
		return newBotBotGO;
		
	}
	
	public GameObject ConstructBotModule(Module module){
		GameObject newModule = null;
		
		switch (module.GetModuleType()){
			case ModuleType.kFuelCell:{
				newModule = GameObject.Instantiate(botFuelCellPrefab);
				newModule.GetComponent<BotFuelCell>().fuelCell = module as FuelCell;
				break;
			}
			case ModuleType.kEngine:{
				newModule = GameObject.Instantiate(botEnginePrefab);
				newModule.GetComponent<BotEngine>().engine = module as Engine;
				break;
			}
			case ModuleType.kConstructor:{
				newModule = GameObject.Instantiate(botConstructorPrefab);
				newModule.GetComponent<BotConstructor>().constructor = module as Constructor;
				break;
			}
		}
		return newModule;
	}

	
	void Awake(){
		// Singleton
		if (singleton != null) Debug.LogError ("Error assigning singleton");
		singleton = this;
		
		
	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
