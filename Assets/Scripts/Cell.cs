using UnityEngine;
using System.Xml;
using System.Xml.Serialization;


[XmlRoot("Cell")]
public class Cell : Module{
	public bool testValue = true;

	public Cell(){
	}
	
	
	public Cell(Module parent, int spokeId){
		parent.Attach(spokeId, this);
	}
	
	public override string GetTypeName(){
		return "Cell";
	}
	
	public override ModuleType GetModeulType(){
		return ModuleType.kCell;
	}
	
	
	public override void DebugPrint(){
		Debug.Log("Type = Cell");
		Debug.Log ("testValue = " + testValue);
		base.DebugPrint();
	}
	

}
