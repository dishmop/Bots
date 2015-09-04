using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI : MonoBehaviour {
	public static UI singleton = null;
	
	public GameObject libPanel;
	public GameObject listingPanel;
	public GameObject listingNamePanel;
	public GameObject editPanel;
	public GameObject editNamePanel;
	public GameObject consolePanel;
	public GameObject homeBase;
	
	public Color[] levelColors;
	
	public enum LogLevel{
		kMessage,
		kWarning,
		kError
	}
	
	int lastListItem = -1;
	int tempCount  = 0;
	
	public void OnEditClick(){
	
		editPanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit = listingPanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit;
		editNamePanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit = listingNamePanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit;
		
	}
	
	void UpdateViews(){
//		libPanel.transform.FindChild("TextBox").GetComponent<TextBox>().Redraw();
//		listingPanel.transform.FindChild("TextBox").GetComponent<TextBox>().Redraw();
//		listingNamePanel.transform.FindChild("TextBox").GetComponent<TextBox>().Redraw();
//		editPanel.transform.FindChild("TextBox").GetComponent<TextBox>().Redraw();
//		editNamePanel.transform.FindChild("TextBox").GetComponent<TextBox>().Redraw();
//		consolePanel.transform.FindChild("TextBox").GetComponent<TextBox>().Redraw();
		lastListItem = -1;
	}
	
	public void OnBaseBuildClick(){
		homeBase.GetComponent<HomeBase>().build = true;
		
	}
	
	public void OnDeleteClick(){
		int listItem = libPanel.transform.FindChild("TextBox").GetComponent<TextBox>().listBoxSelectedItem;
		if (listItem >= 0 ){
			string scriptToDelete = libPanel.transform.FindChild("TextBox").GetComponent<TextBox>().listBoxSelectedText;
			if (scriptToDelete != ""){
				bool ok = SystemScripts.singleton.DeleteScript(scriptToDelete);
				if (!ok){
					LogConsole("Error: Failed to delete script - '" + scriptToDelete + "'", LogLevel.kError);
				}
				else{
					LogConsole("Script '" + scriptToDelete + "' successfuly deleted", LogLevel.kMessage);
				}
			}
		}
		UpdateViews();
		
	}
	
	public void OnRestart(){
		Application.LoadLevel(0);
	}
		
	
	public void OnSaveClick(){
		// Get the name to save it as
		string saveName = editNamePanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit;
		
		if (saveName == ""){
			LogConsole("Error: Can't save to an empty script name", LogLevel.kError);
			return;
		}
		if (saveName.Length > 4 && saveName.Substring(saveName.Length - 4) != ".lua"){
			editNamePanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit = saveName + ".lua";
		}
		
		
		// Fist check that we don't already have a script called this
		List<string> existingSystemNames =  SystemScripts.singleton.FetchSystemScriptNames();
		
		if (existingSystemNames.Exists(x => x == saveName)){
			LogConsole ("Error: script name '" + saveName + "' is a System script already", LogLevel.kError);
			return;
		}
		
		SystemScripts.singleton.SaveUserScript(saveName, editPanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit);
		LogConsole ("Script '" + saveName + "' Has been succesfully saved", LogLevel.kMessage);
		UpdateViews();
		
	}
	
	public void LogConsole(string text, LogLevel logLevel){
		consolePanel.transform.FindChild("TextBox").GetComponent<TextBox>().AddRichText(text + "\n", levelColors[(int)logLevel]);
	}
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (tempCount++ < 10) return;
		
		// Get the script listing of the selected script
		int listItem = libPanel.transform.FindChild("TextBox").GetComponent<TextBox>().listBoxSelectedItem;
		if (listItem != lastListItem){
			string scriptToList = libPanel.transform.FindChild("TextBox").GetComponent<TextBox>().listBoxSelectedText;
			string scriptContent  = SystemScripts.singleton.FetchScript(scriptToList);
			listingPanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit = scriptContent;
			
			listingNamePanel.transform.FindChild("TextBox").GetComponent<TextBox>().textToEdit = scriptToList;
		}
		lastListItem = listItem;
	
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
