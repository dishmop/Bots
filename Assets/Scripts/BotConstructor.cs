using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotConstructor : BotModule {
	public Constructor constructor;
	public static int botCount = 0;
	
	bool isDettachComplete = true;
	bool hasDettachStarted = false;


	GameObject childBotBotGO;

	public void OnConstructionReady(){
		hasDettachStarted = true;
		constructor.OnNewConstructionReady();
		if (constructor.allowRelease){
			DoDetachment();
		}
	}
	
	void DoDetachment(){
		BotBot childBot = childBotBotGO.GetComponent<BotBot>();
		childBot.GameUpdate();
		constructor.OnCompleteConstruction();
		
		
		childBotBotGO.GetComponent<BotBot>().CreateRigidBody();
		if (childBotBotGO.transform.parent.parent.GetComponent<Rigidbody2D>().constraints != RigidbodyConstraints2D.FreezeAll){
			childBotBotGO.GetComponent<Rigidbody2D>().velocity = transform.parent.GetComponent<Rigidbody2D>().GetPointVelocity(transform.position);
			childBotBotGO.GetComponent<Rigidbody2D>().angularVelocity = transform.parent.GetComponent<Rigidbody2D>().angularVelocity;
		}
		
		childBotBotGO.transform.SetParent(null);
		childBotBotGO.GetComponent<BotBot>().SetBotActive(true);
		
		isDettachComplete = true;
		constructor.allowRelease = false;
		hasDettachStarted = false;
		
		childBotBotGO = null;
		Debug.Log (Time.fixedTime + ": DoDetachment");
	}
	
	
//	public bool IsDetachComplete(){
//		return isDettachComplete;
//	}
//	
	// Use this for initialization
	public override void Start () {
		base.Start();
	
		// Heck must sort the scale out first
		GetComponent<BotModule>().HandleScale();
		
	
	}
	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		

		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		
		if (constructor.activated){
			requestedPower = constructor.CalcPowerRequirements();
		}
		
		if (hasDettachStarted && !isDettachComplete && constructor.allowRelease){
			DoDetachment();
		}
		else{
			constructor.allowRelease = false;
		}



		
	}
	
	public override void GameUpdatePostPowerCalc ()
	{
		if (constructor.activated){
			// if we have enough power to construct
			if (MathUtils.FP.Feq(requestedPower, availablePower)){
				if (!IsConstructing()){
					ConstructBot();
				}
				usedPower = availablePower;
			}
			else{
				constructor.activated = false;
			}
		}
		if (!constructor.activated){
			// If not active and constructing, we need to cancel what we have done
			if (IsConstructing()){
				GameObject.Destroy(childBotBotGO);
				
			}
			usedPower = 0;
		}

		
		// Also, if the effect is in waiting mode - then don't use any power
		// HMM this woul be much better if it did use power but all the power went to heating up the thing that is in the way
//		if (childBotBotGO != null && childBotBotGO.GetComponent<GenerateEffect>() != null && !childBotBotGO.GetComponent<GenerateEffect>().IsConstructing()){
//			usedPower = 0;
//		}

	}
	
	bool IsConstructing(){
		return (childBotBotGO != null);
	}
	

	

	
	GameObject ConstructBot(){
		LuaBinding binding = new LuaBinding();
		Bot newBot = binding.ProcessLuaFile(Application.streamingAssetsPath + "/" + constructor.botDefinition + ".lua");
		
		
		childBotBotGO = BotFactory.singleton.ConstructBotBot(newBot);
		childBotBotGO.name = constructor.botDefinition + "_" + botCount++;
		
		// Position it appropriately
		BotBot botBot = childBotBotGO.GetComponent<BotBot>();
		Vector3 fwDir = transform.rotation * new Vector3(0, 1, 0);
		float dist =  botBot.bounds.extents.y  + GetComponent<Renderer>().bounds.extents.y;
		childBotBotGO.transform.position = transform.position + dist * fwDir;
		childBotBotGO.transform.rotation = transform.rotation;
		
		
		// Get a copy of the object names and their userdata so we can pass it to the runtime script
		LuaInterface.LuaTable globals = binding.lua["_G"] as LuaInterface.LuaTable;
		
					
		System.Collections.Specialized.ListDictionary dict = binding.lua.GetTableDict(globals);
		
		foreach (DictionaryEntry pair in dict)
		{
			newBot.RegisterLuaName(pair.Key.ToString(), pair.Value);
			
		}
		float constructionDuration = botBot.mass / constructor.volume;
		childBotBotGO.transform.SetParent(transform);
		childBotBotGO.GetComponent<GenerateEffect>().InitialiseEffect(constructionDuration);
		
		isDettachComplete = false;
		
		return childBotBotGO;
 		
	}
}
