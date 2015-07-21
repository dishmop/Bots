using UnityEngine;


public class FuelCell : Module{

	public FuelCell(Bot bot) : base(bot){
	}
	
	
	public FuelCell(Module parent, int spokeId) : base(parent, spokeId){
	}
	
	public override string GetTypeName(){
		return "FuelCell";
	}
	
	public override string GetShortTypeName(){
		return "F";
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kFuelCell;
	}
	
	
	public override void DebugPrint(){
		Debug.Log("Type = FuelCell");
		base.DebugPrint();
	}
	

}
