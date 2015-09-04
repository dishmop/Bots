using UnityEngine;
using System.Collections;
using System.Text;

public class ScriptLibraryText : MonoBehaviour {

	float nextDirCheckTime = 0;
	float dirCheckDuration = 2;
	
	StringBuilder dirText = new StringBuilder();

	// Use this for initialization
	void Start () {
		UpdateDirInfo();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Time.fixedTime > nextDirCheckTime){
			nextDirCheckTime = Time.fixedTime + dirCheckDuration;
			UpdateDirInfo();
		}
		

	}
	
	
	void UpdateDirInfo(){
		
		dirText.Length = 0;
		foreach (string scriptName in SystemScripts.singleton.scriptNames){
			dirText.Append(scriptName + "\n");
		}
		
		GetComponent<TextBox>().textToEdit = dirText.ToString();
	}
}
