using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

[XmlRoot("Module")]
[XmlInclude(typeof(Cell))]
[XmlInclude(typeof(Engine))]
public class Module{
	public const int numSpokes = 6;
	public Module[]  modules = new Module[6];
	public float energy;
	
	public enum DirtyFlag{
		kEditor,
		kGame,
		kNumFlags
	};
	public bool[] dirtyFlag = new bool[(int)DirtyFlag.kNumFlags];
	public int[] repId = new int[(int)DirtyFlag.kNumFlags];
	
	
	
	public Module (){
		for (int i = 0; i < dirtyFlag.Count(); ++i){
			dirtyFlag[i] = true;
			repId[i] = -1;
		}
	}
	
	public virtual string GetTypeName(){
		return "Module";
	}
	
	public virtual ModuleType GetModeulType(){
		return ModuleType.kError;
	}
	
	public void Attach(int spokeId, Module otherModule){
		DebugUtils.Assert (spokeId < numSpokes, "Invalid spoke Id");
		DebugUtils.Assert (modules[spokeId] == null, "Attempting to attach to occupied spoke");
		modules[spokeId] = otherModule;
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

}
