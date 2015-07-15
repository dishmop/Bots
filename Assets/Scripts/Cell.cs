﻿using UnityEngine;
using System.Xml;
using System.Xml.Serialization;


[XmlRoot("Cell")]
public class Cell : Module{
	public bool testValue = true;

	public Cell(){
	}
	
	public Cell(Module parent, int spokeId){
		parent.Attach(spokeId, this);
	}
	
	
	public override void DebugPrint(){
		Debug.Log("Type = Cell");
		Debug.Log ("testValue = " + testValue);
		base.DebugPrint();
	}
	

}