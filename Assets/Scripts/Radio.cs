using UnityEngine;


public class Radio : Module{
//	int orient = 0;



	public Radio(Bot bot, float size) : base(bot, size){
	}
	
	
	public Radio(Module parent, int spokeId, float size) : base(parent, spokeId, size){
	}
	
	

	public override string GetTypeName(){
		return "Radio";
	}
	
	public override string GetShortTypeName(){
		return "R";
	}

	
	public override void DebugPrint(){
		Debug.Log("Type = Radio");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kRadio;
	}
	
	
}
