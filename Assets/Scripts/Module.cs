using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Module")]
public class Module{
	public const int numSpokes = 6;
	public Module[]  modules = new Module[6];
	
	public float energy;
	
	
	public void Attach(int spokeId, Module otherModule){
		DebugUtils.Assert (spokeId < numSpokes, "Invalid spoke Id");
		DebugUtils.Assert (modules[spokeId] == null, "Attempting to attach to occupied spoke");
		modules[spokeId] = otherModule;
	}

}
