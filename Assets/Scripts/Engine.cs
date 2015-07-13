using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Engine")]
public class Engine : Module{

	public Engine(){
	}
	
	public Engine(Module parent, int spokeId){
		parent.Attach(spokeId, this);
	}
	
}
