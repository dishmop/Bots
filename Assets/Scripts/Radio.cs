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
		
		
		Key_Backspace,		// The backspace key.
		Key_Delete,			// The forward delete key.
		Key_Tab,			// The tab key.
		Key_Clear,			// The Clear key.
		Key_Return,			// Return key.
		Key_Pause,			// Pause on PC machines.
		Key_Escape,			// Escape key.
		Key_Space,			// Space key.
		Key_Keypad0,		// Numeric keypad 0.
		Key_Keypad1,		// Numeric keypad 1.
		Key_Keypad2,		// Numeric keypad 2.
		Key_Keypad3,		// Numeric keypad 3.
		Key_Keypad4,		// Numeric keypad 4.
		Key_Keypad5,		// Numeric keypad 5.
		Key_Keypad6,		// Numeric keypad 6.
		Key_Keypad7,		// Numeric keypad 7.
		Key_Keypad8,		// Numeric keypad 8.
		Key_Keypad9,		// Numeric keypad 9.
		Key_KeypadPeriod,	// Numeric keypad '.'.
		Key_KeypadDivide,	// Numeric keypad '/'.
		Key_KeypadMultiply,	// Numeric keypad '*'.
		Key_KeypadMinus,	// Numeric keypad '-'.
		Key_KeypadPlus,		// Numeric keypad '+'.
		Key_KeypadEnter,	// Numeric keypad enter.
		Key_KeypadEquals,	// Numeric keypad '='.
		Key_UpArrow,		// Up arrow key.
		Key_DownArrow,		// Down arrow key.
		Key_RightArrow,		// Right arrow key.
		Key_LeftArrow,		// Left arrow key.
		Key_Insert,			// Insert key key.
		Key_Home,			// Home key.
		Key_End,			// End key.
		Key_PageUp,			// Page up.
		Key_PageDown,		// Page down.
		Key_F1,				// F1 function key.
		Key_F2,				// F2 function key.
		Key_F3,				// F3 function key.
		Key_F4,				// F4 function key.
		Key_F5,				// F5 function key.
		Key_F6,				// F6 function key.
		Key_F7,				// F7 function key.
		Key_F8,				// F8 function key.
		Key_F9,				// F9 function key.
		Key_F10,			// F10 function key.
		Key_F11,			// F11 function key.
		Key_F12,			// F12 function key.
		Key_F13,			// F13 function key.
		Key_F14,			// F14 function key.
		Key_F15,			// F15 function key.
		Key_Alpha0,			// The '0' key on the top of the alphanumeric keyboard.
		Key_Alpha1,			// The '1' key on the top of the alphanumeric keyboard.
		Key_Alpha2,			// The '2' key on the top of the alphanumeric keyboard.
		Key_Alpha3,			// The '3' key on the top of the alphanumeric keyboard.
		Key_Alpha4,			// The '4' key on the top of the alphanumeric keyboard.
		Key_Alpha5,			// The '5' key on the top of the alphanumeric keyboard.
		Key_Alpha6,			// The '6' key on the top of the alphanumeric keyboard.
		Key_Alpha7,			// The '7' key on the top of the alphanumeric keyboard.
		Key_Alpha8,			// The '8' key on the top of the alphanumeric keyboard.
		Key_Alpha9,			// The '9' key on the top of the alphanumeric keyboard.
		Key_Exclaim,		// Exclamation mark key '!'.
		Key_DoubleQuote,	// Double quote key '"'.
		Key_Hash,			// Hash key '#'.
		Key_Dollar,			// Dollar sign key '$'.
		Key_Ampersand,		// Ampersand key '&'.
		Key_Quote,			// Quote key '.
		Key_LeftParen,		// Left Parenthesis key '('.
		Key_RightParen,		// Right Parenthesis key ')'.
		Key_Asterisk,		// Asterisk key '*'.
		Key_Plus,			// Plus key '+'.
		Key_Comma,			// Comma ',' key.
		Key_Minus,			// Minus '-' key.
		Key_Period,			// Period '.' key.
		Key_Slash,			// Slash '/' key.
		Key_Colon,			// Colon ':' key.
		Key_Semicolon,		// Semicolon ';' key.
		Key_Less,			// Less than '<' key.
		Key_Equals,			// Equals '=' key.
		Key_Greater,		// Greater than '>' key.
		Key_Question,		// Question mark '?' key.
		Key_At,				// At key '@'.
		Key_LeftBracket,	// Left square bracket key '['.
		Key_Backslash,		// Backslash key '\'.
		Key_RightBracket,	// Right square bracket key ']'.
		Key_Caret,			// Caret key '^'.
		Key_Underscore,		// Underscore '_' key.
		Key_BackQuote,		// Back quote key '`'.
		Key_A,				// 'a' key.
		Key_B,				// 'b' key.
		Key_C,				// 'c' key.
		Key_D,				// 'd' key.
		Key_E,				// 'e' key.
		Key_F,				// 'f' key.
		Key_G,				// 'g' key.
		Key_H,				// 'h' key.
		Key_I,				// 'i' key.
		Key_J,				// 'j' key.
		Key_K,				// 'k' key.
		Key_L,				// 'l' key.
		Key_M,				// 'm' key.
		Key_N,				// 'n' key.
		Key_O,				// 'o' key.
		Key_P,				// 'p' key.
		Key_Q,				// 'q' key.
		Key_R,				// 'r' key.
		Key_S,				// 's' key.
		Key_T,				// 't' key.
		Key_U,				// 'u' key.
		Key_V,				// 'v' key.
		Key_W,				// 'w' key.
		Key_X,				// 'x' key.
		Key_Y,				// 'y' key.
		Key_Z,				// 'z' key.
		Key_Numlock,		// Numlock key.
		Key_CapsLock,		// Capslock key.
		Key_ScrollLock,		// Scroll lock key.
		Key_RightShift,		// Right shift key.
		Key_LeftShift,		// Left shift key.
		Key_RightControl,	// Right Control key.
		Key_LeftControl,	// Left Control key.
		Key_RightAlt,		// Right Alt key.
		Key_LeftAlt,		// Left Alt key.
		Key_LeftCommand,	// Left Command key.
		Key_LeftApple,		// Left Command key.
		Key_LeftWindows,	// Left Windows key.
		Key_RightCommand,	// Right Command key.
		Key_RightApple,		// Right Command key.
		Key_RightWindows,	// Right Windows key.
		Key_AltGr,			// Alt Gr key.
		Key_Help,			// Help key.
		Key_Print,			// Print key.
		Key_SysReq,			// Sys Req key.
		Key_Break,			// Break key.
		Key_Menu,			// Menu key.
		Mouse_Button0,		// First (primary) mouse button.
		Mouse_Button1,		// Second (secondary) mouse button.
		Mouse_Button2,		// Third mouse button.		
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
	public bool[] isDown = new bool[(int)ButtonNames.NumButtons];
	public float[] axisValues = new float[(int)AxisNames.NumAxes];
	
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
//			Debug.Log ("enumName.ToString() = " + enumName.ToString());
		}
	}
	
	
	public float GetAxisValue(string name){
		return axisValues[(int)stringToAxisNamesMap[name]];
		
	}
	
	public bool IsButtonDown(string name){
		return isDown[(int)stringToButtonNamesMap[name]];
	}
	
	public bool HasButtonDownTriggered(string name){
		bool retVal = hasUpTriggered[(int)stringToButtonNamesMap[name]];
		hasUpTriggered[(int)stringToButtonNamesMap[name]] = false;
		return retVal;
	}	
	
	public bool HasButtonUpTriggered(string name){
		bool retVal = hasDownTriggered[(int)stringToButtonNamesMap[name]];
		hasDownTriggered[(int)stringToButtonNamesMap[name]] = false;
		return retVal;
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
		
		
		thisToUnityButtonMappings[(int)ButtonNames.Key_Backspace] = 	KeyCode.Backspace;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Delete] = 		KeyCode.Delete;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Tab] = 			KeyCode.Tab;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Clear] = 		KeyCode.Clear;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Return] = 		KeyCode.Return;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Pause] = 		KeyCode.Pause;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Escape] = 		KeyCode.Escape;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Space] =			KeyCode.Space;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad0] = 		KeyCode.Keypad0;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad1] = 		KeyCode.Keypad1;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad2] = 		KeyCode.Keypad2;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad4] = 		KeyCode.Keypad4;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad5] = 		KeyCode.Keypad5;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad6] = 		KeyCode.Keypad6;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad7] = 		KeyCode.Keypad7;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad8] =	 	KeyCode.Keypad8;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Keypad9] = 		KeyCode.Keypad9;
		thisToUnityButtonMappings[(int)ButtonNames.Key_KeypadPeriod] = 	KeyCode.KeypadPeriod;
		thisToUnityButtonMappings[(int)ButtonNames.Key_KeypadDivide] = 	KeyCode.KeypadDivide;
		thisToUnityButtonMappings[(int)ButtonNames.Key_KeypadMultiply] = KeyCode.KeypadMultiply;
		thisToUnityButtonMappings[(int)ButtonNames.Key_KeypadMinus] = 	KeyCode.KeypadMinus;
		thisToUnityButtonMappings[(int)ButtonNames.Key_KeypadPlus] = 	KeyCode.KeypadPlus;
		thisToUnityButtonMappings[(int)ButtonNames.Key_KeypadEnter] = 	KeyCode.KeypadEnter;
		thisToUnityButtonMappings[(int)ButtonNames.Key_KeypadEquals] = KeyCode.KeypadEquals;
		thisToUnityButtonMappings[(int)ButtonNames.Key_UpArrow] =		 KeyCode.UpArrow;
		thisToUnityButtonMappings[(int)ButtonNames.Key_DownArrow] = 	KeyCode.DownArrow;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightArrow] = 	KeyCode.RightArrow;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftArrow] = 	KeyCode.LeftArrow;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Insert] = 		KeyCode.Insert;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Home] = 			KeyCode.Home;
		thisToUnityButtonMappings[(int)ButtonNames.Key_End] = 			KeyCode.End;
		thisToUnityButtonMappings[(int)ButtonNames.Key_PageUp] = 		KeyCode.PageUp;
		thisToUnityButtonMappings[(int)ButtonNames.Key_PageDown] = 		KeyCode.PageDown;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F1] = 			KeyCode.F1;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F2] = 			KeyCode.F2;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F3] = 			KeyCode.F3;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F4] = 			KeyCode.F4;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F5] = 			KeyCode.F5;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F6] = 			KeyCode.F6;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F7] = 			KeyCode.F7;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F8] = 			KeyCode.F8;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F9] = 			KeyCode.F9;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F10] = 			KeyCode.F10;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F11] = 			KeyCode.F11;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F12] = 			KeyCode.F12;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F13] = 			KeyCode.F13;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F14] = 			KeyCode.F14;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F15] = 			KeyCode.F15;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha0] = 		KeyCode.Alpha0;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha1] = 		KeyCode.Alpha1;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha2] = 		KeyCode.Alpha2;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha3] = 		KeyCode.Alpha3;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha4] = 		KeyCode.Alpha4;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha5] = 		KeyCode.Alpha5;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha6] = 		KeyCode.Alpha6;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha7] = 		KeyCode.Alpha7;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha8] = 		KeyCode.Alpha8;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Alpha9] = 		KeyCode.Alpha9;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Exclaim] = 		KeyCode.Exclaim;
		thisToUnityButtonMappings[(int)ButtonNames.Key_DoubleQuote] = 	KeyCode.DoubleQuote;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Hash] = 			KeyCode.Hash;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Dollar] = 		KeyCode.Dollar;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Ampersand] = 	KeyCode.Ampersand;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Quote] = 		KeyCode.Quote;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftParen] = 	KeyCode.LeftParen;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightParen] = 	KeyCode.RightParen;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Asterisk] = 		KeyCode.Asterisk;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Plus] = 			KeyCode.Plus;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Comma] = 		KeyCode.Comma;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Minus] = 		KeyCode.Minus;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Period] = 		KeyCode.Period;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Slash] = 		KeyCode.Slash;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Colon] = 		KeyCode.Colon;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Semicolon] = 	KeyCode.Semicolon;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Less] = 			KeyCode.Less;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Equals] = 		KeyCode.Equals;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Greater] = 		KeyCode.Greater;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Question] = 		KeyCode.Question;
		thisToUnityButtonMappings[(int)ButtonNames.Key_At] = 			KeyCode.At;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftBracket] = 	KeyCode.LeftBracket;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Backslash] = 	KeyCode.Backslash;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightBracket] = 	KeyCode.RightBracket;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Caret] = 		KeyCode.Caret;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Underscore] = 	KeyCode.Underscore;
		thisToUnityButtonMappings[(int)ButtonNames.Key_BackQuote] = 	KeyCode.BackQuote;
		thisToUnityButtonMappings[(int)ButtonNames.Key_A] = KeyCode.A;
		thisToUnityButtonMappings[(int)ButtonNames.Key_B] = KeyCode.B;
		thisToUnityButtonMappings[(int)ButtonNames.Key_C] = KeyCode.C;
		thisToUnityButtonMappings[(int)ButtonNames.Key_D] = KeyCode.D;
		thisToUnityButtonMappings[(int)ButtonNames.Key_E] = KeyCode.E;
		thisToUnityButtonMappings[(int)ButtonNames.Key_F] = KeyCode.F;
		thisToUnityButtonMappings[(int)ButtonNames.Key_G] = KeyCode.G;
		thisToUnityButtonMappings[(int)ButtonNames.Key_H] = KeyCode.H;
		thisToUnityButtonMappings[(int)ButtonNames.Key_I] = KeyCode.I;
		thisToUnityButtonMappings[(int)ButtonNames.Key_J] = KeyCode.J;
		thisToUnityButtonMappings[(int)ButtonNames.Key_K] = KeyCode.K;
		thisToUnityButtonMappings[(int)ButtonNames.Key_L] = KeyCode.L;
		thisToUnityButtonMappings[(int)ButtonNames.Key_M] = KeyCode.M;
		thisToUnityButtonMappings[(int)ButtonNames.Key_N] = KeyCode.N;
		thisToUnityButtonMappings[(int)ButtonNames.Key_O] = KeyCode.O;
		thisToUnityButtonMappings[(int)ButtonNames.Key_P] = KeyCode.P;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Q] = KeyCode.Q;
		thisToUnityButtonMappings[(int)ButtonNames.Key_R] = KeyCode.R;
		thisToUnityButtonMappings[(int)ButtonNames.Key_S] = KeyCode.S;
		thisToUnityButtonMappings[(int)ButtonNames.Key_T] = KeyCode.T;
		thisToUnityButtonMappings[(int)ButtonNames.Key_U] = KeyCode.U;
		thisToUnityButtonMappings[(int)ButtonNames.Key_V] = KeyCode.V;
		thisToUnityButtonMappings[(int)ButtonNames.Key_W] = KeyCode.W;
		thisToUnityButtonMappings[(int)ButtonNames.Key_X] = KeyCode.X;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Y] = KeyCode.Y;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Z] = KeyCode.Z;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Numlock] = 		KeyCode.Numlock;
		thisToUnityButtonMappings[(int)ButtonNames.Key_CapsLock] = 		KeyCode.CapsLock;
		thisToUnityButtonMappings[(int)ButtonNames.Key_ScrollLock] =	KeyCode.ScrollLock;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightShift] = 	KeyCode.RightShift;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftShift] = 	KeyCode.LeftShift;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightControl] = 	KeyCode.RightControl;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftControl] = 	KeyCode.LeftControl;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightAlt] = 		KeyCode.RightAlt;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftAlt] = 		KeyCode.LeftAlt;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftCommand] = 	KeyCode.LeftCommand;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftApple] = 	KeyCode.LeftApple;
		thisToUnityButtonMappings[(int)ButtonNames.Key_LeftWindows] = 	KeyCode.LeftWindows;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightCommand] = 	KeyCode.RightCommand;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightApple] = 	KeyCode.RightApple;
		thisToUnityButtonMappings[(int)ButtonNames.Key_RightWindows] = 	KeyCode.RightWindows;
		thisToUnityButtonMappings[(int)ButtonNames.Key_AltGr] = 		KeyCode.AltGr;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Help] = 			KeyCode.Help;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Print] = 		KeyCode.Print;
		thisToUnityButtonMappings[(int)ButtonNames.Key_SysReq] = 		KeyCode.SysReq;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Break] = 		KeyCode.Break;
		thisToUnityButtonMappings[(int)ButtonNames.Key_Menu] = 			KeyCode.Menu;
		
		thisToUnityButtonMappings[(int)ButtonNames.Mouse_Button0] = KeyCode.Mouse0;
		thisToUnityButtonMappings[(int)ButtonNames.Mouse_Button1] = KeyCode.Mouse1;
		thisToUnityButtonMappings[(int)ButtonNames.Mouse_Button2] = KeyCode.Mouse2;		
		
		
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
