using UnityEngine;


public class Constructor : Module{
	public string botDefinition = "missile";

	public Constructor(Bot bot, float size) : base(bot, size){

	}
	
	public Constructor(Module parent, int spokeId, float size) : base(parent, spokeId, size){
		
	}
	
	public void SetBotDefinitiion(string botDefinition){
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
	
	public override string GenerateRootConstructor(string thisName, string botName){
		string text = base.GenerateRootConstructor(thisName, botName);
		text += "ConstructorSetBotDefinition(" + thisName + ", \"" + botDefinition + "\")\n";
		return text;
	}
	
	public override string GenerateAttachConstructor(string thisName, string parentObjName, int spoke){
		string text = base.GenerateAttachConstructor(thisName, parentObjName, spoke);
		text += "ConstructorSetBotDefinition(" + thisName + ", \"" + botDefinition + "\")\n";
		
		return text;
	}
	
	
	
}
