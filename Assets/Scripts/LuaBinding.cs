using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;


public class LuaBinding{
	public Bot bot; 
	public Lua lua = new Lua();
	


	public LuaBinding(){
		var ver = System.Reflection.Assembly.GetAssembly(typeof(Lua)).GetName().Version;
		LocalLog ("Lua DLL version number" + ver.ToString());
	

		
		// Do construction bindings
		lua.RegisterFunction("ConstructBot",this,this.GetType().GetMethod("ConstructBot"));
		lua.RegisterFunction("ConstructFuelCell",this,this.GetType().GetMethod("ConstructFuelCell"));
		lua.RegisterFunction("ConstructEngine",this,this.GetType().GetMethod("ConstructEngine"));
		lua.RegisterFunction("ConstructConstructor",this,this.GetType().GetMethod("ConstructConstructor"));
		lua.RegisterFunction("ConstructAI",this,this.GetType().GetMethod("ConstructAI"));
		
		lua.RegisterFunction("ConstructAttachedFuelCell",this,this.GetType().GetMethod("ConstructAttachedFuelCell"));
		lua.RegisterFunction("ConstructAttachedEngine",this,this.GetType().GetMethod("ConstructAttachedEngine"));
		lua.RegisterFunction("ConstructAttachedConstructor",this,this.GetType().GetMethod("ConstructAttachedConstructor"));
		lua.RegisterFunction("ConstructAttachedAI",this,this.GetType().GetMethod("ConstructAttachedAI"));
		
		lua.RegisterFunction("ConstructorSetBotDefinition",this,this.GetType().GetMethod("ConstructorSetBotDefinition"));
		lua.RegisterFunction("AISetRuntimeScript",this,this.GetType().GetMethod("AISetRuntimeScript"));
		
		
		
		lua.RegisterFunction("ConstructorEnableAutoRepeat",this,this.GetType().GetMethod("ConstructorEnableAutoRepeat"));
		lua.RegisterFunction("ConstructorActivate",this,this.GetType().GetMethod("ConstructorActivate"));
		lua.RegisterFunction("ModuleEnableConsumable",this,this.GetType().GetMethod("ModuleEnableConsumable"));

				
//		lua.RegisterFunction("BotLoadScript",this,this.GetType().GetMethod("BotLoadScript"));
		lua.RegisterFunction("BotEnableAnchor",this,this.GetType().GetMethod("BotEnableAnchor"));
		
		
		// Do runtime bindings
		lua.RegisterFunction("EngineSetPower", this, this.GetType().GetMethod("EngineSetPower"));
		
		
		lua.RegisterFunction("Log",this,this.GetType().GetMethod("Log"));
	}
	
	public Bot ProcessLuaFile(string luaFilename){
		
		bot = null;
		try
		{
			lua.DoFileASync(luaFilename, 1);
			
			while (!lua.isFinishedASync){
				lua.ResumeAsync();
			}
			
		}
		catch (KopiLua.Lua.LuaException ex)
		{
			LocalLog(ex.StackTrace);
		}

		return bot;
	}
	
	
	
	public string GenerateBotConstructionScript(Bot bot){
		bot.ClearVisitedFlags();
		Queue<Module> moduleQueue = new Queue<Module>();
		Queue<int> spokeQueue = new Queue<int>();
		Queue<string> objNameQueue = new Queue<string>();
		Queue<string> parentObjNameQueue = new Queue<string>();
		
		int objNameCount = 0;
		
		StringBuilder builder = new StringBuilder();
		string botName = "bot";
		builder.Append(bot.GenerateConstructor(botName));
		
		if (bot.rootModule == null) return builder.ToString();
		
		moduleQueue.Enqueue(bot.rootModule);
		spokeQueue.Enqueue(-1);
		objNameQueue.Enqueue("obj_" + objNameCount++);
		parentObjNameQueue.Enqueue(null);
		
		
		
		while (moduleQueue.Count() != 0){
			Module thisModule = moduleQueue.Dequeue();
			int thisSpoke = spokeQueue.Dequeue();
			string thisObjName = objNameQueue.Dequeue();
			string thisParentObjName = parentObjNameQueue.Dequeue();
			
			// If this is the first module
			if (thisParentObjName == null){
				builder.Append(thisModule.GenerateRootConstructor(thisObjName, botName));
				
			}
			// Otherwise parent it to the parent module
			else{
				builder.Append(thisModule.GenerateAttachConstructor(thisObjName, thisParentObjName, thisSpoke));
			}
			for (int i = 0; i < 6; ++i){
				if (thisModule.modules[i] != null && !thisModule.modules[i].visited){
					moduleQueue.Enqueue(thisModule.modules[i]);
					spokeQueue.Enqueue(i);
					objNameQueue.Enqueue("obj_" + objNameCount++);
					parentObjNameQueue.Enqueue(thisObjName);
				}
				
			}
			thisModule.visited = true;
			
		}
		return builder.ToString();
	
		
	}
	
	
	public Bot ConstructBot(){
		bot  = new Bot();
		LocalLog ("Construct Bot");
		return bot;
		
	}
	
	public FuelCell ConstructFuelCell(Bot bot, float size){
		FuelCell cell  = new FuelCell(bot, size);
		bot.rootModule = cell;
		LocalLog ("Construct FuelCell");
		return cell;
		
	}
	
	public FuelCell ConstructAttachedFuelCell(Module parent, int spoke, float size){
		FuelCell cell  = new FuelCell(parent, spoke, size);
		LocalLog ("Construct attached FuelCell");
		return cell;
		
	}
	
	
	public Engine ConstructEngine(Bot bot, float size){
		Engine engine = new Engine(bot, size);
		bot.rootModule = engine;
		LocalLog ("Construct Engine");
		return engine;
		
	}
	
	public Engine ConstructAttachedEngine(Module parent, int spoke, float size){
		Engine engine = new Engine(parent, spoke, size);
		LocalLog ("Construct attached Engine");
		return engine;
		
	}
	
	public Constructor ConstructConstructor(Bot bot, float size){
		Constructor constructor = new Constructor(bot, size);
		bot.rootModule = constructor;
		LocalLog ("Construct Constructor");
		return constructor;
		
	}
	
	public Constructor ConstructAttachedConstructor(Module parent, int spoke, float size){
		Constructor constructor = new Constructor(parent, spoke, size);
		LocalLog ("Construct attached Constructor");
		return constructor;
		
	}
	
	
	public AI ConstructAI(Bot bot, float size){
		AI ai = new AI(bot, size);
		bot.rootModule = ai;
		LocalLog ("Construct AI");
		return ai;
		
	}
	
	public AI ConstructAttachedAI(Module parent, int spoke, float size){
		AI ai = new AI(parent, spoke, size);
		LocalLog ("Construct attached AI");
		return ai;
		
	}
	
	public void ConstructorSetBotDefinition(Constructor constructor, string botDefinition){
		constructor.SetBotDefinition(botDefinition);
		LocalLog ("ConstructorSetBotDefinition: " + botDefinition);
	}
	
	public void AISetRuntimeScript(AI ai, string scriptName){
		ai.SetScript(scriptName);
		LocalLog ("AISetRuntimeScript: " + scriptName);
	}
	
//	public void BotLoadScript(Bot bot, string runtimeScript){
//		bot.runtimeScript = runtimeScript;
//		LocalLog ("BotLoadScript: " + runtimeScript);
//	}
	
	public void ConstructorEnableAutoRepeat(Constructor constructor, bool enable){
		constructor.EnableAutoRepeat(enable);

	}
	
	public void ConstructorActivate(Constructor constructor, bool activate){
		constructor.Activate(activate);
		
	}
	
	public void ModuleEnableConsumable(Module module, bool enable){
		module.enableConsumable = enable;
		
	}	
	
	public void BotEnableAnchor(Bot bot, bool enable){
		bot.enableAnchor = enable;
		
		
	}
	
	public void Log(string text){
		LocalLog ("Lua: " + text);
	}
	
	// Runtime control
	public void EngineSetPower(Engine engine, float power){
		engine.desAmount = Mathf.Clamp(power, -1, 1);
		LocalLog ("EngineSetPower: " + power);
	}
	
	void LocalLog(string text){
		//Debug.Log (text);
	}
	
	
}
