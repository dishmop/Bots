using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Module")]
[XmlInclude(typeof(Cell))]
[XmlInclude(typeof(Engine))]
public class Module{
	public const int numSpokes = 6;
	public Module[]  modules = new Module[6];
	
	public float energy;
	
	public Module (){
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
