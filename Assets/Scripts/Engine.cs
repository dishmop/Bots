using UnityEngine;


public class Engine : Module{
//	int orient = 0;

	// Runtime values
	public float desPower = 0;
	public float powerMultiplied = 1;

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
	
	public override float GetPowerRequirements(){
		return powerMultiplied * Mathf.Abs(desPower) * size;
	}
	
	// Called if there is not enough power to cope with requirements
	public override void OnPowerShortage(){
		powerMultiplied = Mathf.Lerp (powerMultiplied, 0, 0.1f);
	}
	
	
	
}
