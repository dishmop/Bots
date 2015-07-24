using UnityEngine;


public class FuelCell : Module{

	public FuelCell(Bot bot, float size) : base(bot, size){
		enableConsumable = true;
		
	}
	
	
	public FuelCell(Module parent, int spokeId, float size) : base(parent, spokeId, size){
		enableConsumable = true;
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
