using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotConstructor : MonoBehaviour {
	public Constructor constructor;

	GameObject childBotBotGO;

	public void OnSpawnDetach(){
		childBotBotGO = null;
		constructor.OnCompleteConstruction();
	}
	
	// Use this for initialization
	void Start () {
	
		// Heck must sort the scale out first
		GetComponent<BotModule>().HandleScale();
		
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		
		if (constructor.activated){
			if (!IsConstructing()){
				ConstructBot();
			}

		}
		else{
			// If not active and constructing, we need to cancel what we have done
			if (IsConstructing()){
				GameObject.Destroy(childBotBotGO);
				
			}
		}
	}
	
	bool IsConstructing(){
		return (childBotBotGO != null);
	}
	

	

	
	GameObject ConstructBot(){
		LuaBinding binding = new LuaBinding();
		Bot newBot = binding.ProcessLuaFile(Application.streamingAssetsPath + "/" + constructor.botDefinition + ".lua");
		
		
		childBotBotGO = BotFactory.singleton.ConstructBotBot(newBot);
		childBotBotGO.name = constructor.botDefinition;
		
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
		float constructionDuration = childBotBotGO.GetComponent<Rigidbody2D>().mass / constructor.size;
		childBotBotGO.transform.SetParent(transform);
		childBotBotGO.GetComponent<GenerateEffect>().InitialiseEffect(constructionDuration);
		return childBotBotGO;
 		
	}
}
