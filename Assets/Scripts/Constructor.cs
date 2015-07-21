using UnityEngine;


public class Constructor : Module{
	public string botDefinition;

	public Constructor(Bot bot, string botDefinition) : base(bot){
		this.botDefinition = botDefinition;
	}
	
	public Constructor(Module parent, int spokeId, string botDefinition) : base(parent, spokeId){
		this.botDefinition = botDefinition;
	}
	
	public override string GetTypeName(){
		return "Constructor";
	}
	
	public override string GetShortTypeName(){
		return "C";
	}

	
	public override void DebugPrint(){
		Debug.Log("Type = Constructor");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kConstructor;
	}
	
	public override string GenerateRootConstructor(string botName){
		return "Construct" + GetTypeName() + "(" + botName + ", \"" + botDefinition + "\")\n";
	}
	
	public override string GenerateAttachConstructor(string parentObjName, int spoke){
		return "ConstructAttached" + GetTypeName() + "(" + parentObjName + ", " + spoke + ", \"" + botDefinition + "\")\n";
	}
	
	
	
}
