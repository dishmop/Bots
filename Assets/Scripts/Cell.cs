using UnityEngine;
using System.Xml;
using System.Xml.Serialization;


[XmlRoot("Cell")]
public class Cell : Module{
	public bool testValue = true;

	public Cell(Bot bot) : base(bot){
	}
	
	
	public Cell(Module parent, int spokeId) : base(parent, spokeId){
	}
	
	public override string GetTypeName(){
		return "Cell";
	}
	
	public override string GetShortTypeName(){
		return "C";
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kCell;
	}
	
	
	public override void DebugPrint(){
		Debug.Log("Type = Cell");
		Debug.Log ("testValue = " + testValue);
		base.DebugPrint();
	}
	

}
