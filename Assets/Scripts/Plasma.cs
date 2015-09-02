using UnityEngine;


public class Plasma : Module{



	public Plasma(Bot bot, float size) : base(bot, size){
		groundFriction = 0;
		airResistance = 0;
		enableConsumable = true;
	}
	
	
	public Plasma(Module parent, int spokeId, float size) : base(parent, spokeId, size){
		groundFriction = 0;
		airResistance = 0;
		enableConsumable = true;
	}
	
	

	public override string GetTypeName(){
		return "Plasma";
	}
	
	public override string GetShortTypeName(){
		return "P";
	}

	
	public override void DebugPrint(){
		Debug.Log("Type = Plasma");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kPlasma;
	}
	
	
}
