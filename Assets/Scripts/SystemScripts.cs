using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public class SystemScripts : MonoBehaviour {

	public static SystemScripts singleton = null;
	
	string userScriptNameListKey = "ScriptNameList";
	
	
	Dictionary<string, string> systemScripts = new Dictionary<string, string>();
	Dictionary<string, string> userScripts = new Dictionary<string, string>();
	


	
	public void SaveUserScripts(){
		// Concatonate script names in a single string
		StringBuilder sb = new StringBuilder();
		foreach (string scriptName in userScripts.Keys){
			sb.Append (scriptName + ";");
			PlayerPrefs.SetString(scriptName, userScripts[scriptName]);
		}
		
		PlayerPrefs.SetString(userScriptNameListKey, sb.ToString());
		
	}
	
	public void LoadUserScripts(){
		string nameListString = PlayerPrefs.GetString(userScriptNameListKey);
		
		userScripts.Clear();
		int startIndex = 0;
		int endIndex = nameListString.IndexOf(';', startIndex);
		
		while (startIndex < nameListString.Length){
			string scriptName = nameListString.Substring(startIndex, endIndex - startIndex);
			string script = PlayerPrefs.GetString(scriptName);
			userScripts.Add (scriptName, script);
			startIndex = endIndex + 1;
			endIndex = nameListString.IndexOf(';', startIndex);
			
		}
		

		
		
	}
	
	
	void Start(){
		Resources.LoadAll("SystemScripts/");
		Object[] scripts = Resources.FindObjectsOfTypeAll(typeof(TextAsset));
		
		List<TextAsset> scriptAssets = new List<TextAsset>();
		
		foreach (Object obj in scripts){
			int nameLen = obj.name.Length;
			if (nameLen > 4 && obj.name.Substring(nameLen - 4) == ".lua"){
				scriptAssets.Add (obj as TextAsset);
			}
			
		}
		
		foreach (TextAsset script in scriptAssets){
			Debug.Log ("Loading asset: :" + script.name);
			string luaProg = script.text.Replace("\t", "    ");
			systemScripts.Add (script.name, luaProg);
			
			Resources.UnloadAsset(script);
		}
		
		LoadUserScripts();

	
	}
	
	
	public void SaveUserScript(string scriptName, string script){
		if (userScripts.ContainsKey(scriptName)){
			userScripts[scriptName] = script;
		}
		else{
			userScripts.Add (scriptName, script);
		}
		
		SaveUserScripts();
		
	}
	
	
	public bool DeleteScript(string scriptName){
		if (!DoesScriptExist(scriptName)){
			return false;
		}
		
		if (systemScripts.ContainsKey(scriptName)){
			return false;
		}
		
		userScripts.Remove(scriptName);
		PlayerPrefs.DeleteKey(scriptName);
		SaveUserScripts();
		return true;
	}

	public List<string> FetchUserScriptNames(){
		return userScripts.Keys.ToList();
	}
	
	public List<string> FetchSystemScriptNames(){
		return systemScripts.Keys.ToList();
	}
	
	public bool DoesScriptExist(string scriptName){
		return systemScripts.ContainsKey(scriptName) || userScripts.ContainsKey(scriptName);
	}
	
	public string FetchScript(string scriptName){
		if (systemScripts.ContainsKey(scriptName)){
			return systemScripts[scriptName];
		}
		else if (userScripts.ContainsKey(scriptName)){
			return userScripts[scriptName];
		}
		else{
			return "";
		}
	}
	
	string BuildResourceName(string scriptName){
		return "/SystemScripts/" + scriptName;
	}
	
	// We saving using the standard file system
	string BuildFullPath(string scriptName){
		return Application.dataPath + "/Resources/Levels/" + scriptName + ".bytes";

		
	}
	
	
	// This isn't used
	public string LoadSystemScript(string scriptName){
		string luaProg = "";
		string path = BuildResourceName(scriptName);
		Debug.Log("LoadScript: " + path);
		TextAsset asset = Resources.Load(path) as TextAsset;
		if (asset != null){
			Debug.Log ("Loading asset");
			Stream s = new MemoryStream(asset.bytes);
			
			
			BinaryReader br = new BinaryReader(s);
			luaProg = br.ReadString();
			Resources.UnloadAsset(asset);
			
		}	
		else{
			Debug.Log ("Failed to load asset");
		}
		return luaProg;
	}
	
	
	
	// Does the actual serialising
	public void SaveSystemScript(string scriptName, string luaProg){
		#if UNITY_EDITOR		
		FileStream file = File.Create(BuildFullPath(scriptName));
		BinaryWriter bw = new BinaryWriter(file);
		bw.Write(luaProg);
		

		
		
		file.Close();
		
		// Ensure the assets are all realoaded and the cache cleared.
		UnityEditor.AssetDatabase.Refresh();
		#endif
	}	
	
	void Awake(){
		// Singleton
		if (singleton != null) Debug.LogError ("Error assigning singleton");
		singleton = this;
		
	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
