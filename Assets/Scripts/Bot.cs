using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Bot")]
public class Bot{
	public Module rootModule;
	public string name; 
	
	public Bot (){
	}
	
	public Bot (Module rootModule, string name){
		this.rootModule = rootModule;
		this.name = name;
	}

}
