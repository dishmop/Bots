using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using LuaInterface;

public class LuaTest : MonoBehaviour {
	public string LuaFileToLoad = "";
	
	Lua lua = new Lua();

	// Use this for initialization
	void Start () {
		// Do bindings
		lua.RegisterFunction("ConstructBot",this,this.GetType().GetMethod("ConstructBot"));
		lua.RegisterFunction("ConstructCell",this,this.GetType().GetMethod("ConstructCell",  new Type[] { typeof(Bot) }));
		lua.RegisterFunction("ConstructEngine",this,this.GetType().GetMethod("ConstructEngine",  new Type[] { typeof(Bot) }));
		lua.RegisterFunction("ConstructCellP",this,this.GetType().GetMethod("ConstructCell",  new Type[] { typeof(Module), typeof(int) }));
		lua.RegisterFunction("ConstructEngineP",this,this.GetType().GetMethod("ConstructEngine",  new Type[] { typeof(Module), typeof(int) }));
		
		lua.DoFile(Application.streamingAssetsPath+"/"+LuaFileToLoad);		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Bot ConstructBot(){
		Bot bot  = new Bot();
		Debug.Log ("Construct Bot");
		return bot;
		
	}
	
	public Cell ConstructCell(Bot bot){
		Cell cell  = new Cell(bot);
		Debug.Log ("Construct Cell");
		return cell;
		
	}
	
	public Cell ConstructCell(Module parent, int spoke){
		Cell cell  = new Cell(parent, spoke);
		Debug.Log ("Construct Cell from parent");
		return cell;
		
	}
	
	
	public Engine ConstructEngine(Bot bot){
		Engine engine = new Engine(bot);
		Debug.Log ("Construct Engine");
		return engine;
		
	}
	
	public Engine ConstructEngine(Module parent, int spoke){
		Engine engine = new Engine(parent, spoke);
		Debug.Log ("Construct Engine from parent");
		return engine;
		
	}

}
