using UnityEngine;


public class AI : Module{
//	int orient = 0;

	public string scriptName = "";


	public AI(Bot bot, float size) : base(bot, size){
	}
	
	
	public AI(Module parent, int spokeId, float size) : base(parent, spokeId, size){
	}
	
	
	public void SetScript(string scriptName){
		this.scriptName = scriptName;
	}
	
	public override string GetTypeName(){
		return "AI";
	}
	
	public override string GetShortTypeName(){
		return "A";
	}

	
	public override void DebugPrint(){
		Debug.Log("Type = AI");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kAI;
	}
	
	
}
