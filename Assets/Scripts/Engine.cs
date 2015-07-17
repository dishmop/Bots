using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Engine")]
public class Engine : Module{

	public Engine(Bot bot) : base(bot){
	}
	
	public override string GetTypeName(){
		return "Engine";
	}
	
	public override string GetShortTypeName(){
		return "E";
	}
	
	public Engine(Module parent, int spokeId) : base(parent, spokeId){
	}
	
	public override void DebugPrint(){
		Debug.Log("Type = Engine");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kEngine;
	}
	
	
	
}
