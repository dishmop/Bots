using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotConstructor : MonoBehaviour {
	public Constructor constructor;
	
	float spawnDuration = 2;
	float spawnTime = 0;
	bool spawnDone = false;
	
	
	
	
	// Use this for initialization
	void Start () {
		spawnTime = Time.fixedTime;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		
		if (!spawnDone && Time.fixedTime > spawnTime + spawnDuration){
			//spawnDone = true;
			spawnTime = Time.fixedTime;
			LuaBinding binding = new LuaBinding();
			Bot newBot = binding.ProcessLuaFile(Application.streamingAssetsPath + "/" + constructor.botDefinition + ".lua");
			
			
			GameObject botBotGO = BotFactory.singleton.ConstructBotBot(newBot);
			botBotGO.name = constructor.botDefinition;
			
			// Position it appropriately
			BotBot botBot = botBotGO.GetComponent<BotBot>();
			Vector3 fwDir = transform.rotation * new Vector3(0, 1, 0);
			float dist =  botBot.bounds.extents.y  + GetComponent<Renderer>().bounds.extents.y;
			botBotGO.transform.position = transform.position + dist * fwDir;
			botBotGO.transform.rotation = transform.rotation;
			
			// Get a copy of the object names and their userdata so we can pass it to the runtime script
			LuaInterface.LuaTable globals = binding.lua["_G"] as LuaInterface.LuaTable;
			
						
			System.Collections.Specialized.ListDictionary dict = binding.lua.GetTableDict(globals);
			
			foreach (DictionaryEntry pair in dict)
			{
				newBot.RegisterLuaName(pair.Key.ToString(), pair.Value);
				
			}

			botBotGO.GetComponent<GenerateEffect>().InitialiseEffect();
	 		
	 	}
	
	}
}
