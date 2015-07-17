using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System;

[XmlRoot("Module")]
[XmlInclude(typeof(Cell))]
[XmlInclude(typeof(Engine))]
public class Module{
	public const int numSpokes = 6;
	public Module[]  modules = new Module[6];
	public float energy;
	public Guid guid;
	
	
	public enum DirtyFlag{
		kEditor,
		kGame,
		kNumFlags
	};
	public bool[] dirtyFlag = new bool[(int)DirtyFlag.kNumFlags];
	public Guid[] repId = new Guid[(int)DirtyFlag.kNumFlags];
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
		for (int i = 0; i < numSpokes; ++i){
			Module module = modules[i];
			if (module != null){
				Debug.Log ("Spoke num: " + i);
				module.DebugPrint();
			}
			else{
				
			//	Debug.Log ("Spoke num: " + i + " = null");
			}
		}
	}
	
	void InitSetup(Bot bot){
		guid = Guid.NewGuid();
		for (int i = 0; i < dirtyFlag.Count(); ++i){
			dirtyFlag[i] = true;
			repId[i] = Guid.Empty;
		}

		this.bot = bot;
		if (bot != null){
			bot.moduleLookup.Add (guid, this);
		}
	}

}
