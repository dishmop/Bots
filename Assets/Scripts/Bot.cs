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
	


	
	


}
