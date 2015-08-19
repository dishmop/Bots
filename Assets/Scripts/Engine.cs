using UnityEngine;


public class Engine : Module{
//	int orient = 0;

	// Runtime values
	public float desAmount = 0;


	public Engine(Bot bot, float size) : base(bot, size){
	}
	
	
	public Engine(Module parent, int spokeId, float size) : base(parent, spokeId, size){
	}
	
	public override string GetTypeName(){
		return "Engine";
	}
	
	public override string GetShortTypeName(){
		return "E";
	}

	
	public override void DebugPrint(){
		Debug.Log("Type = Engine");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kEngine;
	}
	
	public float CalcPowerRequirements(float amount){
		return Mathf.Abs(amount) * volume;
	}
	

	
	
	
}
