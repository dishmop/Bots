using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


public class Radio : Module{
	public Dictionary<string, ButtonNames> stringToButtonNamesMap = new Dictionary<string, ButtonNames>();
	public Dictionary<string, AxisNames> stringToAxisNamesMap = new Dictionary<string, AxisNames>();
	
	public enum ButtonNames{
		JoystickButton_A,
		JoystickButton_B,
		JoystickButton_X,
		JoystickButton_Y,
		
		// NOt handle consistently between windows and mac
		//		kJoystickButton_Up,
		//		kJoystickButton_Down,
		//		kJoystickButton_Left,
		//		kJoystickButton_Right,
		NumButtons,
	}
	
	public enum AxisNames{
		Joystick_LeftStick_X,
		Joystick_LeftStick_Y,
		Joystick_RightStick_X,
		Joystick_RightStick_Y,
		Joystick_LeftTrigger,
		Joystick_RightTrigger,
		NumAxes,
	}
	
	public KeyCode[] thisToUnityButtonMappings = new KeyCode[(int)ButtonNames.NumButtons];
	public bool[] hasDownTriggered = new bool[(int)ButtonNames.NumButtons];
	public bool[] hasUpTriggered = new bool[(int)ButtonNames.NumButtons];
	
	public string[] thisToUnityAxesMappings = new string[(int)AxisNames.NumAxes];
	



	public Radio(Bot bot, float size) : base(bot, size){
		SetupInverseMaps();
		SetupMappingsOSX();
	}
	
	
	public Radio(Module parent, int spokeId, float size) : base(parent, spokeId, size){
		SetupInverseMaps();
		SetupMappingsOSX();
	}
	
	

	public override string GetTypeName(){
		return "Radio";
	}
	
	public override string GetShortTypeName(){
		return "R";
	}

	
	public override void DebugPrint(){
		Debug.Log("Type = Radio");
		base.DebugPrint();
	}
	
	public override ModuleType GetModuleType(){
		return ModuleType.kRadio;
	}
	
	
	void SetupInverseMaps(){
		for (int i = 0; i < (int)ButtonNames.NumButtons; ++i){
			ButtonNames enumName = (ButtonNames)i;
			stringToButtonNamesMap.Add (enumName.ToString(), enumName);
		}
		
		for (int i = 0; i < (int)AxisNames.NumAxes; ++i){
			AxisNames enumName = (AxisNames)i;
			stringToAxisNamesMap.Add (enumName.ToString(), enumName);
			Debug.Log ("enumName.ToString() = " + enumName.ToString());
		}
	}
	
	
	public float GetAxisValue(string name){
		AxisNames enumName = stringToAxisNamesMap[name];
		return Input.GetAxis(thisToUnityAxesMappings[(int)enumName]);
		
	}
	
	public bool IsButtonDown(string name){
		ButtonNames enumName = stringToButtonNamesMap[name];
		return Input.GetKey(thisToUnityButtonMappings[(int)enumName]);
	}
	
	public bool HasButtonDownTriggered(string name){
		return hasUpTriggered[(int)stringToButtonNamesMap[name]];
	}	
	
	public bool HasButtonUpTriggered(string name){
		
		return hasDownTriggered[(int)stringToButtonNamesMap[name]];
	}	
	
	
	
	
	public void ButtonTriggerReset(){
		for (int i = 0; i < (int)ButtonNames.NumButtons; ++i){
			hasDownTriggered[i] = false;
			hasUpTriggered[i] = false;
		}
	}
	
	void SetupMappingsOSX(){
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_A] = KeyCode.JoystickButton16;
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_B] = KeyCode.JoystickButton17;
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_X] = KeyCode.JoystickButton18;
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_Y] = KeyCode.JoystickButton19;
		
		
		thisToUnityAxesMappings[(int)AxisNames.Joystick_LeftStick_X] = "X axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_LeftStick_Y] = "Y axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_RightStick_X] = "3rd axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_RightStick_Y] = "4th axis";
		
		
		thisToUnityAxesMappings[(int)AxisNames.Joystick_LeftTrigger] = "5th axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_RightTrigger] = "6th axis";		
		
	}
	
	
	void SetupMappingsWin(){
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_A] = KeyCode.JoystickButton0;
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_B] = KeyCode.JoystickButton1;
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_X] = KeyCode.JoystickButton2;
		thisToUnityButtonMappings[(int)ButtonNames.JoystickButton_Y] = KeyCode.JoystickButton3;
		
		thisToUnityAxesMappings[(int)AxisNames.Joystick_LeftStick_X] = "X axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_LeftStick_Y] = "Y axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_RightStick_X] = "4th axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_RightStick_Y] = "5th axis";
		
		thisToUnityAxesMappings[(int)AxisNames.Joystick_LeftTrigger] = "9th axis";
		thisToUnityAxesMappings[(int)AxisNames.Joystick_RightTrigger] = "10th axis";			}

	
}
