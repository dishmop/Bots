using UnityEngine;


public class Engine : Module{
	int orient = 0;

	public Engine(Bot bot) : base(bot){
	}
	
	
	public Engine(Module parent, int spokeId) : base(parent, spokeId){
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
	
	
	
}
