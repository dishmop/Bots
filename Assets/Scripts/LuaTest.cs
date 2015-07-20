using UnityEngine;
using System.Collections;
using System.IO;

public class LuaTest : MonoBehaviour {

	static public AluminumLua.LuaObject CallMe1 (AluminumLua.LuaObject [] args){
		Debug.Log("CallMe1 called!");
		return  new AluminumLua.LuaObject();
	}
	
	
	static public AluminumLua.LuaObject CallMe2 (AluminumLua.LuaObject [] args){
		Debug.Log("CallMe2 called!");
		return  new AluminumLua.LuaObject();
	}
	

	// Use this for initialization
	void Start () {
		
		var context = new AluminumLua.LuaContext ();
		context.AddBasicLibrary ();
		context.AddIoLibrary ();
		
		context.SetGlobal ("CallMe1", LuaTest.CallMe1);
		context.SetGlobal ("CallMe2", LuaTest.CallMe2);
		context.SetGlobal ("random_string", "hello");
		// ...
		
		string pathname = Application.persistentDataPath + "/helloworld.lua";
		TextReader luaFile = File.OpenText(pathname);
		
		var parser = new AluminumLua.LuaParser (context, luaFile); // < or leave file_name out to read stdin
		parser.Parse ();
		
	
	}
	

	
	// Update is called once per frame
	void Update () {
	
	}
}
