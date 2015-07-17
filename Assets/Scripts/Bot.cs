using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlRoot("Bot")]
public class Bot{
	public Module rootModule;
	public string name; 
	public Dictionary<Guid, Module> moduleLookup = new Dictionary<Guid, Module>();
	

	public Bot (){
	}
	
	public void DebugPrint(){
		Debug.Log("Bot - DebugPrint");
		Debug.Log("Name = " + name);
		rootModule.DebugPrint();
	}
	
	public Bot (string name){
		this.name = name;
	}
	
	public Module FindModule(Guid guid){
//		foreach (KeyValuePair<Guid, Module> entry in moduleLookup){
//			Debug.Log ("key = " + entry.Key.ToString() + ", Value = " + entry.Value.ToString());
//		}
		if (!moduleLookup.ContainsKey(guid)) return null;
		return moduleLookup[guid];
	}
	
	


}
