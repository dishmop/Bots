using UnityEngine;
using System.Collections;

public class BotRadio : BotModule {
	public Radio radio;
	

	public override void Start ()
	{
		base.Start ();

	}

	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
	
	}
	
	public override void Update(){
		
		for (int i = 0; i < (int)Radio.ButtonNames.NumButtons; ++i){
			radio.hasDownTriggered[i] = radio.hasDownTriggered[i] || Input.GetKeyDown(radio.thisToUnityButtonMappings[i]);	
			radio.hasUpTriggered[i] = radio.hasUpTriggered[i] || Input.GetKeyUp(radio.thisToUnityButtonMappings[i]);	
			radio.isDown[i] = Input.GetKey(radio.thisToUnityButtonMappings[i]);	
		}
		for (int i = 0; i < (int)Radio.AxisNames.NumAxes; ++i){
			radio.axisValues[i] = -Input.GetAxis(radio.thisToUnityAxesMappings[i]);	
		}
		
	}
	
	

}
