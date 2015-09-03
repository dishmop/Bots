using UnityEngine;


public class SolarCell : Module{



	public SolarCell(Bot bot, float size) : base(bot, size){
	}
	
	
	public SolarCell(Module parent, int spokeId, float size) : base(parent, spokeId, size){
	}
	
	public override float GetVolumetricHeatCapacity(){
		return 20;
	}
	

	public override string GetTypeName(){
		return "SolarCell";
	}
	
	public override string GetShortTypeName(){
		return "S";
	}

	
	public override void DebugPrint(){
		Debug.Log("Type = SolarCell");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kSolarCell;
	}
	
	
}
