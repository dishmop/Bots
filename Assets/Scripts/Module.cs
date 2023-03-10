using UnityEngine;
using System.Linq;
using System;

public class Module{
	public const int numSpokes = 6;
	public Module[]  modules = new Module[6];
	public float energy;
	public Guid guid;
	public float groundFriction = 10;
	public float airResistance = 1;
	public float volume = 1;
	public bool enableConsumable = false;
	public float heatEnergy = 0;
	public float initialVolume = 0;
	
	public Bot bot;
	
	public bool visited;
	
	public Module (Bot bot, float size){
		this.volume= size;
		initialVolume = size;
		InitSetup(bot);
	}
	
	// Parent and the spoke of the parent we are attaching to
	public Module(Module parent, int spokeId, float size){
		this.volume= size;
		initialVolume = size;
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
	
	public bool CanRecharge(){
		return (volume < initialVolume);
	}
	
	// We shouldn't grow more than our initial volume
	public void SetVolume(float newVolume){
		volume = Mathf.Max (0, Mathf.Min (newVolume, initialVolume));
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

	
	public virtual string GenerateRootConstructor(string thisName, string botName){
		string text = "";
		text += thisName + " = Construct" + GetTypeName() + "(" + botName  +  ", " + volume + ")\n";
		text += GeneratePropertiesString(thisName);
		return text;
	}
	
	public virtual string GenerateAttachConstructor(string thisName, string parentObjName, int spoke){
		string text = "";
		text += thisName + " = ConstructAttached" + GetTypeName() + "(" + parentObjName + ", " + spoke  +  ", " + volume + ")\n";
		text += GeneratePropertiesString(thisName);
		return text;
	}
	
	// kg per unit of volume
	public virtual float GetDensity(){
		return 1;
	}
	
	// Joules per unit of volume
	public virtual float GetEnergyDensity(){
		return 100;
	}
	
	public virtual float GetVolumetricHeatCapacity(){
		return 100;
	}
	
	public virtual float GetMaxKelvin(){
		return 100;
	}
	

	string GeneratePropertiesString(string thisName){
		string text = "";
		// If not the default value, then set it
		if (enableConsumable == true){
			text += "ModuleEnableConsumable(" + thisName + ", " + enableConsumable.ToString() + ")\n";
		}
		return text;
		
	}
	
	void InitSetup(Bot bot){
		guid = Guid.NewGuid();
		
		this.bot = bot;
		if (bot != null){
			bot.guidModuleLookup.Add (guid, this);
		}
	}

}
