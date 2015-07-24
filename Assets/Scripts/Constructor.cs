using UnityEngine;


public class Constructor : Module{
	public string botDefinition = "missile";
	public bool activated = false;
	public bool enableAutoRepeat = false;

	public Constructor(Bot bot, float size) : base(bot, size){

	}
	
	public Constructor(Module parent, int spokeId, float size) : base(parent, spokeId, size){
		
	}
	
	public void SetBotDefinition(string botDefinition){
		this.botDefinition = botDefinition;
	}
	
	public void EnableAutoRepeat(bool enable){

		enableAutoRepeat = enable;
	}
	
	public void Activate(bool activate){
		activated = activate;
	}
	
	public void OnCompleteConstruction(){
		if (!enableAutoRepeat) activated = false;
	}
	
	public override string GetTypeName(){
		return "Constructor";
	}
	
	public override string GetShortTypeName(){
		return "C";
	}

	public override float GetPowerRequirements(){
		return activated ? size : 0;
	}
	
	// Called if there is not enough power to cope with requirements
	public override void OnPowerShortage(){
		activated = false;
	}
	
	
	
	public override void DebugPrint(){
		Debug.Log("Type = Constructor");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kConstructor;
	}
	
	public override string GenerateRootConstructor(string thisName, string botName){
		return base.GenerateRootConstructor(thisName, botName) + GeneratePropertiesString(thisName);

	}
	
	public override string GenerateAttachConstructor(string thisName, string parentObjName, int spoke){
		return  base.GenerateAttachConstructor(thisName, parentObjName, spoke) + GeneratePropertiesString(thisName);

	}
	
	string GeneratePropertiesString(string thisName){
		string text = "";
		text += "ConstructorSetBotDefinition(" + thisName + ", \"" + botDefinition + "\")\n";
		text += "ConstructorEnableAutoRepeat(" + thisName + ", " + enableAutoRepeat.ToString() + ")\n";
		text += "ConstructorActivate(" + thisName + ", " + activated.ToString() + ")\n";
		return text;
		
	}
	
	
	
}
