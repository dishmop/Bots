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
		// Do bindings
		lua.RegisterFunction("ConstructBot",this,this.GetType().GetMethod("ConstructBot"));
		lua.RegisterFunction("ConstructFuelCell",this,this.GetType().GetMethod("ConstructFuelCell"));
		lua.RegisterFunction("ConstructEngine",this,this.GetType().GetMethod("ConstructEngine"));
		lua.RegisterFunction("ConstructConstructor",this,this.GetType().GetMethod("ConstructConstructor"));
		lua.RegisterFunction("ConstructAttachedFuelCell",this,this.GetType().GetMethod("ConstructAttachedFuelCell"));
		lua.RegisterFunction("ConstructAttachedEngine",this,this.GetType().GetMethod("ConstructAttachedEngine"));
		lua.RegisterFunction("ConstructAttachedConstructor",this,this.GetType().GetMethod("ConstructAttachedConstructor"));
		
		lua.RegisterFunction("Log",this,this.GetType().GetMethod("Log"));
	}
	
	
	public string GenerateBotConstructionScript(Bot bot){
		bot.ClearVisitedFlags();
		Queue<Module> moduleQueue = new Queue<Module>();
		Queue<int> spokeQueue = new Queue<int>();
		Queue<string> objNameQueue = new Queue<string>();
		Queue<string> parentObjNameQueue = new Queue<string>();
		
		int objNameCount = 0;
		
		StringBuilder builder = new StringBuilder();
		builder.Append("bot = ConstructBot()\n");
		
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
				builder.Append(thisObjName + " = " + thisModule.GenerateRootConstructor("bot"));
				
			}
			// Otherwise parent it to the parent module
			else{
				builder.Append(thisObjName + " = " + thisModule.GenerateAttachConstructor(thisParentObjName, thisSpoke));
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
		Debug.Log ("Construct Bot");
		return bot;
		
	}
	
	public FuelCell ConstructFuelCell(Bot bot){
		FuelCell cell  = new FuelCell(bot);
		bot.rootModule = cell;
		Debug.Log ("Construct FuelCell");
		return cell;
		
	}
	
	public FuelCell ConstructAttachedFuelCell(Module parent, int spoke){
		FuelCell cell  = new FuelCell(parent, spoke);
		Debug.Log ("Construct attached FuelCell");
		return cell;
		
	}
	
	
	public Engine ConstructEngine(Bot bot){
		Engine engine = new Engine(bot);
		bot.rootModule = engine;
		Debug.Log ("Construct Engine");
		return engine;
		
	}
	
	public Engine ConstructAttachedEngine(Module parent, int spoke){
		Engine engine = new Engine(parent, spoke);
		Debug.Log ("Construct attached Engine");
		return engine;
		
	}
	
	public Constructor ConstructConstructor(Bot bot, string botDefinition){
		Constructor constructor = new Constructor(bot, botDefinition);
		bot.rootModule = constructor;
		Debug.Log ("Construct Constructor");
		return constructor;
		
	}
	
	public Constructor ConstructAttachedConstructor(Module parent, int spoke, string botDefinition){
		Constructor constructor = new Constructor(parent, spoke, botDefinition);
		Debug.Log ("Construct attached Constructor");
		return constructor;
		
	}
	
	public void Log(string text){
		Debug.Log ("Lua: " + text);
	}
	
	
}