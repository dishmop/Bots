﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BotBot : MonoBehaviour {
	public Bot bot;
	public Bounds bounds;
	public Dictionary<Guid, GameObject> modulesToModuleGOs = new Dictionary<Guid, GameObject>();
	public Dictionary<Guid, GameObject> modulesToRodGOs = new Dictionary<Guid, GameObject>();
	public float speed = 0;

	LuaBinding luaBinding;
	
	bool isBotVisible = true;
	bool isBotActive = false;
	
	public void SetBotActive(bool active){
		isBotActive = active;
	}
	
	public void RegisterModule(Module module, GameObject go){
		modulesToModuleGOs.Add (module.guid, go);
	}
	
	public void RegisterRod(Module module, GameObject go){
		modulesToRodGOs.Add (module.guid, go);
	}
	
	public void SetBotVisible(bool visible){
		isBotVisible = visible;
		HandleVisibility();
	}
	
	void HandleVisibility(Transform thisTransform){
		if (thisTransform.GetComponent<MeshRenderer>() != null){
			thisTransform.GetComponent<MeshRenderer>().enabled = isBotVisible;
		}
		foreach (Transform child in thisTransform){
			HandleVisibility(child);
		}
		
	}


	void HandleVisibility(){
		foreach (GameObject go in modulesToModuleGOs.Values){
			HandleVisibility(go.transform);
			
		}
		foreach (GameObject go in modulesToRodGOs.Values){
			HandleVisibility(go.transform);
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void FixedUpdate(){
		if (bot.runtimeScript != null && bot.runtimeScript != "" && luaBinding == null){
			luaBinding = new LuaBinding();
			foreach (KeyValuePair<string, object> pair in bot.luaObjectLookup){
				luaBinding.lua[pair.Key] = pair.Value;
			}
			luaBinding.lua.DoFileASync(Application.streamingAssetsPath  +"/" + bot.runtimeScript + ".lua", 1);
			//luaBinding.lua.DoFile(Application.streamingAssetsPath + "/" + bot.runtimeScript + ".lua");
		}
		
		if (luaBinding != null){
			if (!luaBinding.lua.isFinishedASync){
				luaBinding.lua.ResumeAsync();
			}
		}
	}
}
