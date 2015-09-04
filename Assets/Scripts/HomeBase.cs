using UnityEngine;
using System.Collections;

public class HomeBase : MonoBehaviour {
	public string startupBotDefinition;
	LuaBinding binding = new LuaBinding();
	Bot homeBot;
	
	
	int count = 0;
	
	void Start(){
		transform.FindChild("Placeholder").gameObject.SetActive(false);
	}

	// Use this for initialization
	void FixedUpdate () {
		if (count++ == 12){
			homeBot = binding.ProcessLuaFile(startupBotDefinition + ".lua");			
			GameObject homeBotGO = BotFactory.singleton.ConstructBotBot(homeBot);
			homeBotGO.transform.SetParent(transform);
			homeBotGO.transform.localPosition = Vector3.zero;
			homeBotGO.transform.localRotation = Quaternion.identity;
			homeBotGO.GetComponent<BotBot>().SetBotActive(true);
			homeBotGO.AddComponent<Rigidbody2D>();
			homeBotGO.GetComponent<BotBot>().SolidifyColliders();
			
			
			
		}

		
	}
	

}
