using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Cell")]
public class Cell : Module{

	public Cell(){
	}
	
	public Cell(Module parent, int spokeId){
		parent.Attach(spokeId, this);
	}
	

}
