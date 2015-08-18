using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


public class Bot{
	public Module rootModule;
	public string name; 
	public Dictionary<Guid, Module> guidModuleLookup = new Dictionary<Guid, Module>();
	public Dictionary<string, object> luaObjectLookup = new Dictionary<string, object>();
	public float rodSize = 1.25f;
	public bool enableAnchor = false;
	
	public string runtimeScript;

	
	

	public Bot (){
	}
	
	public void DebugPrint(){
		ClearVisitedFlags();
		Debug.Log("Bot - DebugPrint");
		Debug.Log("Name = " + name);
		rootModule.DebugPrint();
	}
	
	public Bot (string name){
		this.name = name;
	}
	
	// if oure object is part of this bot, then store it in a list
	public bool RegisterLuaName(string key, object obj){
		if (obj == this){
			luaObjectLookup.Add (key, obj);
			return true;
		}
			
		
		foreach (Module module in guidModuleLookup.Values){
			if (obj == module){
				luaObjectLookup.Add (key, obj);
				return true;
			}
		}
		return false;
	}
	
	
	
	
	public Module FindModule(Guid guid){
//		foreach (KeyValuePair<Guid, Module> entry in moduleLookup){
//			Debug.Log ("key = " + entry.Key.ToString() + ", Value = " + entry.Value.ToString());
//		}
		if (!guidModuleLookup.ContainsKey(guid)) return null;
		return guidModuleLookup[guid];
	}
	
	public void ClearVisitedFlags(){
		foreach (Module module in guidModuleLookup.Values){
			module.visited = false;
		}
	}
	
	// Give than we wish to add a new spoke fromModule to Module, what spoke would need to be removed to accomodate this?
	// there can be only 1 - it is the one betweem outMod0 and outMo1
	public Module TrialNewSpoke(Module fromModule, Module toModule){
		// Start at out current module and the new spoke and traverse everyhing we can (but do not go down spokes
		// from module which are not this one).
		ClearVisitedFlags();
		fromModule.visited = true;
		
		Queue<Module> moduleQueue = new Queue<Module>();
		moduleQueue.Enqueue(toModule);
		while (moduleQueue.Count() != 0){
			Module thisModule = moduleQueue.Dequeue();
			thisModule.visited = true;
			
			for (int i = 0; i < 6; ++i){
				Module otherModule = thisModule.modules[i];
				if (otherModule != null && !otherModule.visited){
					moduleQueue.Enqueue(otherModule);
				}
			}
		}
		
		// Now the spoke we remove is the one going to the module off from this which has been visited
		for (int i = 0; i < 6; ++i){
			Module otherModule = fromModule.modules[i];
			if (otherModule != null && otherModule.visited){
				return otherModule;
			}
		}
		DebugUtils.Assert (false, "Failed to find other Module we can disconnect");
		return null;
	}
	
	
	public void ReplaceModule(Module discardedModule, Module newModule){
		newModule.CloneProperties(discardedModule);
		for (int i = 0; i < 6; ++i){
			if (newModule.modules[i] != null){
				newModule.modules[i].modules[SpokeDirs.CalcInverseSpoke(i)] = newModule;
			}
		}
		guidModuleLookup.Remove(discardedModule.guid);
		if (rootModule == discardedModule){
			rootModule = newModule;
		}
		
		
	}
	
	public void ReplaceSpoke(Module fromModule, Module toModule, int toSpoke, Module disconnectedModule){
		for (int i = 0; i < 6; ++i){
			// Remove the one we want to
			if (fromModule.modules[i] == disconnectedModule){
				fromModule.modules[i] = null;
				disconnectedModule.modules[SpokeDirs.CalcInverseSpoke(i)] = null;
				
			}

		}
		fromModule.modules[toSpoke] = toModule;
		toModule.modules[SpokeDirs.CalcInverseSpoke(toSpoke)] = fromModule;
	}
	
	public string GenerateConstructor(string botName){
		string text = botName + " = ConstructBot()" + "\n";
		text += "";
		return text;
	}
	
	public float CalcMinRodSize(){
		float maxSize = 0;
		foreach (Module module in guidModuleLookup.Values){
			maxSize = Mathf.Max (maxSize, module.volume);
		}
		float maxRodLen = 2.1f * maxSize;
		float minRodLen = 0;
		while (maxRodLen - minRodLen > 0.01f){
			float midRodLen = 0.5f * (maxRodLen + minRodLen);
			if (TestIsOverlap(midRodLen)){
				minRodLen = midRodLen;
			}
			else{
				maxRodLen = midRodLen;
			}
		}
		return maxRodLen;
	}
	

	bool TestIsOverlap(float rodLen){
		int numCircles = guidModuleLookup.Count ();
		Vector2[] circleCentres = new Vector2[numCircles];
		float[] circleRadii = new float[numCircles];
		
		Queue<Module> moduleQueue = new Queue<Module>();
		Queue<Vector2> posQueue = new Queue<Vector2>();
		
		moduleQueue.Enqueue(rootModule);
		posQueue.Enqueue(Vector2.zero);
		
		// Create the circles
		ClearVisitedFlags();
		int count= 0;
		while (moduleQueue.Count() != 0){
			Module thisModule = moduleQueue.Dequeue();

			Vector2 thisPos = posQueue.Dequeue();
			
			for (int i = 0;i < 6; ++i){
				if (thisModule.modules[i] != null && !thisModule.modules[i].visited){
					moduleQueue.Enqueue(thisModule.modules[i]);

					posQueue.Enqueue(thisPos + rodLen * SpokeDirs.GetDirVector2D(i));
				}
			}
			circleCentres[count] = thisPos;
			circleRadii[count] =  Balancing.singleton.ConvertModuleSizeToRadius(thisModule.volume);
			count++;
			
			thisModule.visited = true;
		}
		
		for (int i = 0; i < numCircles - 1; ++i){
			for (int j = i + 1; j < numCircles; ++j){
				float sumRadii = circleRadii[i] + circleRadii[j];
				float distSq = (circleCentres[i] - circleCentres[j]).sqrMagnitude;
				if (distSq < sumRadii * sumRadii){
					return true;
				}
			}
		}
		return false;
		
		
	}
			

	
	


}
