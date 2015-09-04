using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SystemScripts : MonoBehaviour {

	public static SystemScripts singleton = null;
	public string[] scriptNames;
	
	Dictionary<string, string> scripts = new Dictionary<string, string>();
	
	
	void FixedUpdate(){
	
	}
	
	void Start(){
		foreach (string scriptName in scriptNames){
			StreamReader reader = File.OpenText(Application.streamingAssetsPath + "/" + scriptName + ".lua");
			string program = reader.ReadToEnd();
			reader.Close();
			scripts.Add(scriptName, program);
			
		}
	
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
