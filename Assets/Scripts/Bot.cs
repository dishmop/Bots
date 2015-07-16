using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Bot")]
public class Bot{
	public Module rootModule;
	public string name; 
	
	public Bot (){
	}
	
	public void DebugPrint(){
		Debug.Log("Bot - DebugPrint");
		Debug.Log("Name = " + name);
		rootModule.DebugPrint();
	}
	
	public Bot (Module rootModule, string name){
		this.rootModule = rootModule;
		this.name = name;
	}
	
	// MAKE A DICTIONARY OF ALL THEMODULES SO WE CAN LOOK THEM UP QUICKLY

}
