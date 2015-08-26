using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BotAI : BotModule {
	public AI ai;
	LuaBinding luaBinding;
	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		
		if (ai.scriptName != null && ai.scriptName != "" && luaBinding == null){
			luaBinding = new LuaBinding();
			foreach (KeyValuePair<string, object> pair in ai.bot.luaObjectLookup){
				luaBinding.lua[pair.Key] = pair.Value;
			}
			luaBinding.lua.DoFileASync(Application.streamingAssetsPath  + "/" + ai.scriptName + ".lua", 10);
			//luaBinding.lua.DoFile(Application.streamingAssetsPath + "/" + bot.runtimeScript + ".lua");
		}
		
		
		if (luaBinding != null){
			if (!luaBinding.lua.isFinishedASync){
				luaBinding.lua.ResumeAsync();
			}
		}
		
	
	}
	

}
