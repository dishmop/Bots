using UnityEngine;
using System;
using System.Reflection;
using LuaInterface;


public class LuaBinding{
	public Bot bot; 
	public Lua lua = new Lua();
		
	public LuaBinding(){
		// Do bindings
		lua.RegisterFunction("ConstructBot",this,this.GetType().GetMethod("ConstructBot"));
		lua.RegisterFunction("ConstructCell",this,this.GetType().GetMethod("ConstructCell"));
		lua.RegisterFunction("ConstructEngine",this,this.GetType().GetMethod("ConstructEngine"));
		lua.RegisterFunction("ConstructAttachedCell",this,this.GetType().GetMethod("ConstructAttachedCell"));
		lua.RegisterFunction("ConstructAttachedEngine",this,this.GetType().GetMethod("ConstructAttachedEngine"));
		lua.RegisterFunction("Log",this,this.GetType().GetMethod("Log"));
	}
	
	
	public Bot ConstructBot(){
		bot  = new Bot();
		Debug.Log ("Construct Bot");
		return bot;
		
	}
	
	public Cell ConstructCell(Bot bot){
		Cell cell  = new Cell(bot);
		Debug.Log ("Construct Cell");
		return cell;
		
	}
	
	public Cell ConstructAttachedCell(Module parent, int spoke){
		Cell cell  = new Cell(parent, spoke);
		Debug.Log ("Construct attached Cell");
		return cell;
		
	}
	
	
	public Engine ConstructEngine(Bot bot){
		Engine engine = new Engine(bot);
		Debug.Log ("Construct Engine");
		return engine;
		
	}
	
	public Engine ConstructAttachedEngine(Module parent, int spoke){
		Engine engine = new Engine(parent, spoke);
		Debug.Log ("Construct attached Engine");
		return engine;
		
	}
	public void Log(string text){
		Debug.Log (text);
	}
	
	
}
