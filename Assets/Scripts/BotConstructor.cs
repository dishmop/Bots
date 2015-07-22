using UnityEngine;
using System.Collections;

public class BotConstructor : MonoBehaviour {
	public Constructor constructor;
	
	float spawnDuraction = 1;
	float spawnTime = 0;
	bool spawnDone = false;
	
	
	
	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!spawnDone && Time.time > spawnTime + spawnDuraction){
			spawnDone = true;
			LuaBinding binding = new LuaBinding();
			Bot newBot = binding.ProcessLuaFile(Application.streamingAssetsPath+"/" + constructor.botDefinition + ".lua");
			GameObject botBotGO = BotFactory.singleton.ConstructBotBot(newBot);
			botBotGO.name = constructor.botDefinition;
			
			// Position it appropriately
			BotBot botBot = botBotGO.GetComponent<BotBot>();
			Vector3 fwDir = transform.rotation * new Vector3(0, 1, 0);
			float dist =  botBot.bounds.extents.y  + GetComponent<Renderer>().bounds.extents.y;
			botBotGO.transform.position = transform.position + dist * fwDir;
			botBotGO.transform.rotation = transform.rotation;
			
			botBotGO.GetComponent<GenerateEffect>().InitialiseEffect();
	 		
	 	}
	
	}
}
