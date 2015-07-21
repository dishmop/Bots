using UnityEngine;
using System.Collections;

public class HomeBase : MonoBehaviour {
	public string startupBotDefinition;
	LuaBinding binding = new LuaBinding();
	Bot homeBot;

	// Use this for initialization
	void Start () {
		homeBot = binding.ProcessLuaFile(Application.streamingAssetsPath+"/" + startupBotDefinition + ".lua");			
		GameObject homeBotGO = BotFactory.singleton.ConstructBotBot(homeBot);
		homeBotGO.transform.SetParent(transform);
		homeBotGO.transform.localPosition = Vector3.zero;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
