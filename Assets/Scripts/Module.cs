using UnityEngine;
using System.Linq;
using System;

public class Module{
	public const int numSpokes = 6;
	public Module[]  modules = new Module[6];
	public float energy;
	public Guid guid;
	public float groundFriction = 1;
	public float airResistance = 1;
	
	public Bot bot;
	
	public bool visited;
	
	public Module (Bot bot){
		InitSetup(bot);
	}
	
	// Parent and the spoke of the parent we are attaching to
	public Module(Module parent, int spokeId){
		InitSetup(parent.bot);
		
		
		DebugUtils.Assert (spokeId < numSpokes, "Invalid spoke Id");
		DebugUtils.Assert (parent.modules[spokeId] == null, "Attempting to attach to occupied spoke");
		
		// Work out which spoke our parent is on relative to us
		int parentSpoke = SpokeDirs.CalcInverseSpoke(spokeId);
		
		parent.modules[spokeId] = this;
		modules[parentSpoke] = parent;
	}
	
	public virtual string GetTypeName(){
		return "Module";
	}
	
	public virtual string GetShortTypeName(){
		return "M";
	}
	
	
	public virtual ModuleType GetModuleType(){
		return ModuleType.kError;
	}
	
	public void CloneProperties(Module module){
		for (int i = 0; i < 6; ++i){
			modules[i] = module.modules[i];
		}
		bot = module.bot;
		
	}
	

	
	public virtual void DebugPrint(){
		Debug.Log("numSpokes = " + numSpokes);
		visited = true;
		for (int i = 0; i < numSpokes; ++i){
			Module module = modules[i];
			if (module != null && !module.visited){
				Debug.Log ("Spoke num: " + i);
				module.DebugPrint();
			}
		}
	}
	
	public virtual string GenerateRootConstructor(string botName){
		return "Construct" + GetTypeName() + "(" + botName + ")\n";
	}
	
	public virtual string GenerateAttachConstructor(string parentObjName, int spoke){
		return "ConstructAttached" + GetTypeName() + "(" + parentObjName + ", " + spoke + ")\n";
	}
	
	void InitSetup(Bot bot){
		guid = Guid.NewGuid();
		
		this.bot = bot;
		if (bot != null){
			bot.moduleLookup.Add (guid, this);
		}
	}

}
