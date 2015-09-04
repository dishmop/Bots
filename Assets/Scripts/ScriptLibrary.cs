using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class ScriptLibrary : MonoBehaviour {
	
	float nextDirCheckTime = 0;
	float dirCheckDuration = 1f;
	
	StringBuilder scriptNameList = new StringBuilder();
	
	// Use this for initialization
	void Start () {
		UpdateLibInfo();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Time.fixedTime > nextDirCheckTime){
			nextDirCheckTime = Time.fixedTime + dirCheckDuration;
			UpdateLibInfo();
		}
		
		
	}
	
	
	void UpdateLibInfo(){
		TextBox textBox = transform.FindChild("TextBox").GetComponent<TextBox>();
		textBox.ClearListBoxItems();
		scriptNameList.Length = 0;
		IList<string> systemScriptNames = SystemScripts.singleton.FetchSystemScriptNames();
		foreach (string scriptName in systemScriptNames){
			textBox.AddItemToListBox(scriptName, new Color(0.25f, 0.25f, 1f));
		}
		
		IList<string> userScriptNames = SystemScripts.singleton.FetchUserScriptNames();
		foreach (string scriptName in userScriptNames){
			textBox.AddItemToListBox(scriptName, new Color(0.75f, 0.75f, 0.75f));
		}
			
	}
}
