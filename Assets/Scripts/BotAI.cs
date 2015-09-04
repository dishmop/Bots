using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BotAI : BotModule {
	public AI ai;
	LuaBinding luaBinding;
	bool error = false;
	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		
		if (error) return;
		
		if (ai.scriptName != null && ai.scriptName != "" && luaBinding == null){
			luaBinding = new LuaBinding();
			foreach (KeyValuePair<string, object> pair in ai.bot.luaObjectLookup){
				luaBinding.lua[pair.Key] = pair.Value;
			}
			string scriptName = ai.scriptName + ".lua";
			
			
			if (!SystemScripts.singleton.DoesScriptExist(scriptName)){
				UI.singleton.LogConsole("Error: Attempting to run non existance script - '" + scriptName + "'", UI.LogLevel.kError);
				error = true;
				return;
			}
			string script = SystemScripts.singleton.FetchScript(scriptName);
			
			try{			
				luaBinding.lua.DoStringASync(script, scriptName, 20);
				
	
			}
			catch (KopiLua.Lua.LuaException ex)
			{
				Debug.Log (ex.StackTrace);
				UI.singleton.LogConsole(ex.StackTrace, UI.LogLevel.kError);
			}
		
			
			//luaBinding.lua.DoFile(Application.streamingAssetsPath + "/" + bot.runtimeScript + ".lua");
		}

		
		
		if (luaBinding != null){
			if (!luaBinding.lua.isFinishedASync){
			
			
				try{			
					luaBinding.lua.ResumeAsync();
					
					
				}
				catch (KopiLua.Lua.LuaException ex)
				{
					Debug.Log (ex.StackTrace);
					UI.singleton.LogConsole(ex.StackTrace, UI.LogLevel.kError);
				}
				
				
				
			}
		}
		
	
	}
	

}
