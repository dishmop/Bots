using UnityEngine;
using System.Collections;

public class HomeBase : MonoBehaviour {
	public string startupBotDefinition;
	public bool build = false;
	LuaBinding binding = new LuaBinding();
	Bot homeBot;
	GameObject homeBotGO;
	
	
	int count = 0;
	
	void Start(){
		transform.FindChild("Placeholder").gameObject.SetActive(false);
	}

	// Use this for initialization
	void FixedUpdate () {
		if (count++ == 10){
			homeBot = binding.ProcessLuaFile(startupBotDefinition + ".lua");			
			homeBotGO = BotFactory.singleton.ConstructBotBot(homeBot);
			homeBotGO.transform.SetParent(transform);
			homeBotGO.transform.localPosition = Vector3.zero;
			homeBotGO.transform.localRotation = Quaternion.identity;
			homeBotGO.GetComponent<BotBot>().SetBotActive(true);
			homeBotGO.AddComponent<Rigidbody2D>();
			homeBotGO.GetComponent<BotBot>().SolidifyColliders();
			
			
		}
		
		if (build){
			foreach (GameObject go in homeBotGO.GetComponent<BotBot>().modulesToModuleGOs.Values){
				if (go.GetComponent<BotConstructor>() != null){
					go.GetComponent<BotConstructor>().constructor.Activate(true);
				}
			}
			build = false;
			
		}

		
	}
	

}
