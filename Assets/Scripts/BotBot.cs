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
	const float massToEngergyFactor = 10;
	
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
	
	}
	
	public void FixedUpdate(){
		GetComponent<Rigidbody2D>().isKinematic = !isBotActive;
		if (!isBotActive) return;
		
		// Thiswould be beter done on a per module basis for slingshot kindof manourvrees
		GetComponent<Rigidbody2D>().constraints = bot.enableAnchor ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
		
		// Caluclate the mass
		float mass = 0;
		foreach(Module module in bot.guidModuleLookup.Values){
			mass += module.size;
		}
		GetComponent<Rigidbody2D>().mass = mass;
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
		
		// Deal with fuel usage
		// First work out fuel usage
		float powerRequirements = 0;
		foreach(Module module in bot.guidModuleLookup.Values){
			powerRequirements += module.GetPowerRequirements();
		}
		float fuelRequiredThisFrame = powerRequirements * Time.fixedDeltaTime;
		
		// See how mcuh fuel we have in total
		float fuelSize = 0;
		foreach(Module module in bot.guidModuleLookup.Values){
			if (module.enableConsumable){
				fuelSize += module.size * massToEngergyFactor;
			}
		}
		bool shortage = false;
		float sizeMul = (fuelSize - fuelRequiredThisFrame)  / fuelSize;
		if (sizeMul < 0){
			sizeMul = 0;
			shortage = true;
		}
		foreach(Module module in bot.guidModuleLookup.Values){
			if (module.enableConsumable){
				module.size *= sizeMul;
			}
			if (shortage){
				module.OnPowerShortage();
			}
		}
	}
}
