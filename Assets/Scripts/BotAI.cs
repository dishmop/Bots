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
			string scriptName = ai.scriptName + ".lua";
			
			
			if (!SystemScripts.singleton.DoesScriptExist(scriptName)){
				UI.singleton.LogConsole("Error: Attempting to run non existance script - '" + scriptName + "'", UI.LogLevel.kError);
				return;
			}
			string script = SystemScripts.singleton.FetchScript(scriptName);
			
			
			luaBinding.lua.DoStringASync(script, scriptName, 20);
			
			
			//luaBinding.lua.DoFile(Application.streamingAssetsPath + "/" + bot.runtimeScript + ".lua");
		}
		
		
		if (luaBinding != null){
			if (!luaBinding.lua.isFinishedASync){
				luaBinding.lua.ResumeAsync();
			}
		}
		
	
	}
	

}
