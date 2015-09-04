using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;

public class TextBox : MonoBehaviour {

	public GameObject panel;
	[Multiline]
	public string textToEdit;
	public bool isEditable;
	public bool isListBox;
	public bool isSingleLine;
	
	public Font font;
	public Vector2 scrollPosition = Vector2.zero;
	public Texture2D blackTeture;
	public string listBoxSelectedText;
	public int listBoxSelectedItem;
	public Guid guid;
	public bool wasInFocus;
	
	List<string> listBoxItems = new List<string>();
	Dictionary<string, Color> listBoxItemColLookup = new Dictionary<string, Color>();

	
	
	int lastCursorPos = 0;
	
	float letterWidth;
	float margineSizeX;
	float letterHeight;
	float margineSizeY;

	
	
	KeyCode lastKeyCode = KeyCode.None;
	int keyUpCounter = 0;
	
	public void ClearListBoxItems(){
		listBoxItems.Clear();
		listBoxItemColLookup.Clear();
	}
		
	public void AddItemToListBox(string text, Color highlightCol){
		listBoxItems.Add (text);
		listBoxItemColLookup.Add (text, highlightCol);
	}
	
	public void RemoveItemFromListBox(string text){
		listBoxItems.Remove (text);
		listBoxItemColLookup.Remove (text);
	}

	// Use this for initialization
	void Start () {
	
		guid = Guid.NewGuid();
	}
	
	public void AddRichText(string text, Color color){
		string itemString = "<color=" + CreateColorString(color) + ">" + text  + "</color>" + "\n";
		textToEdit += itemString;
	}
	
	string ConstructStringFromListBox(){
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < listBoxItems.Count(); ++i){
			string text = listBoxItems[i];
			Color col = listBoxItemColLookup[text];
			
			Color useCol = (listBoxSelectedItem == i) ? col : Color.Lerp (col, Color.black, 0.25f);
			string itemString = "<color=" + CreateColorString(useCol) + ">" + text  + "</color>" + "\n";
			sb.Append(itemString);
		}

		return sb.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void GUICalcStringDims(string text, out int numLines, out int maxLineLen, int cursorPos, out int cursorPosX, out int cursorPosY){
		numLines = 1;
		maxLineLen = 0;
		int currentLineLen = 0;
		
		bool cursorDone = false;
		cursorPosX = 0;
		cursorPosY = 0;
		bool isInsideTag = false;
		
		for (int i = 0; i < text.Length; ++i){
		
			if (i == cursorPos){
				cursorPosX = currentLineLen;
				cursorPosY = numLines-1;
				cursorDone = true;
			}
			if (text[i] == '<'){
				isInsideTag = true;
			}
			if (text[i] == '>'){
				isInsideTag = false;
				continue;
			}
			if (isInsideTag) continue;

			if (text[i] == '\n'){
				numLines++;
				if (currentLineLen > maxLineLen){
					maxLineLen = currentLineLen;
				}
				currentLineLen = 0;
			}
			else{
				currentLineLen++;
			}
		}
		if (currentLineLen > maxLineLen){
			maxLineLen = currentLineLen;
		}
		if (!cursorDone){
			cursorPosX = currentLineLen;
			cursorPosY = numLines-1;
		}
		
	}
	
	Vector2 GUICalcStringDims(string text, GUIStyle style, int cursorPos, out Vector2 cursorPosVec, out int cursorPosY){
		int numLines;
		int maxLineLen;
		int cursorPosX;
		GUICalcStringDims(text, out numLines, out maxLineLen, cursorPos, out cursorPosX, out cursorPosY);
		
		Vector2 textSize1 = style.CalcSize(new GUIContent("A"));
		Vector2 textSize2 = style.CalcSize(new GUIContent("AB"));
		letterWidth = textSize2.x - textSize1.x;
		margineSizeX = textSize1.x - letterWidth;
		
		Vector2 textSize3 = style.CalcSize(new GUIContent("A\nB"));
		letterHeight = textSize3.y - textSize1.y;
		margineSizeY = textSize1.x - letterWidth;
		
		
		float textWidthInPixels = (margineSizeX + letterWidth * (maxLineLen + 0.5f));	// add 0.5 for cursor
		float textHeightInPixels = (margineSizeY +  letterHeight * (numLines));
		
		float cursorPosXFloat = (letterWidth * (cursorPosX));
		float cursorPosYFloat = (letterHeight * (cursorPosY));
		
		cursorPosVec = new Vector2(cursorPosXFloat, cursorPosYFloat);
		return new Vector2(textWidthInPixels, textHeightInPixels);
	
	}
	
	TextEditor GetActiveEditor(){
		TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
		return editor;
	}
	
	string ConstructControlName(){
		return guid.ToString();
		
	}
	
//	public void TriggerReturn(){
//		//if (GUI.GetNameOfFocusedControl() == ConstructControlName()){
//			textToEdit += "Return";
//		//}
//	}
	
	void FixedUpdate(){
		if (listBoxSelectedItem >= 0 && listBoxSelectedItem < listBoxItems.Count()){
			listBoxSelectedText = listBoxItems[listBoxSelectedItem];
		}
		
	}
	
	void OnClickList(Vector2 mousePosition, Rect screenRect){
		float pixelsFromTopOfScrenRect = screenRect.height - screenRect.yMax + mousePosition.y;
		float pixelsFromTopOfView = pixelsFromTopOfScrenRect + scrollPosition.y;
		
		int linNum = (int)((pixelsFromTopOfView - margineSizeY) /  letterHeight);
		listBoxSelectedItem = Mathf.Min (linNum, listBoxItems.Count());
		listBoxSelectedText = listBoxItems[listBoxSelectedItem];
		
		//Debug.Log ("OnClickList: " + linNum);
	}
	
	void OnGUI() {
		Vector3[] worldCorners = new Vector3[4];
		panel.GetComponent<RectTransform>().GetWorldCorners(worldCorners);
		
		// Convert to screen space of old GUI
		for (int i = 0; i < worldCorners.Count (); ++i){
			worldCorners[i].y = Screen.height - worldCorners[i].y;
			
		}
		
		GUIStyle style = GUI.skin.textArea;
		
		style.fontSize = 11;
		style.font = font;
		style.normal.background = null;
		style.focused.background = null;
		style.active.background = null;
		style.hover.background = null;
		
		Color textColor = isEditable ? Color.white : Color.gray;
		style.normal.textColor = textColor;
		style.focused.textColor = textColor;
		style.active.textColor = textColor;
		style.normal.textColor = textColor;
		style.wordWrap = false;
		
		style.richText = (isListBox || !isEditable);
		
		
		
		// Find cursor pos in current active window - I suppose
		TextEditor editor = GetActiveEditor();
	
		
		string textToShow = isListBox ? ConstructStringFromListBox() : textToEdit;
		
		Vector2 cursorPosVec;
		int cursorPosLineNum;
		Vector2 stringDims = GUICalcStringDims(textToShow, style, editor.pos, out cursorPosVec, out cursorPosLineNum);
		
		Rect screenRect = new Rect(worldCorners[1], worldCorners[3] - worldCorners[1]);

		
		// For Crazy OSX standalong keydown bug
		bool crazyEnterAdded = false;
		
		if (GUI.GetNameOfFocusedControl() == ConstructControlName()){
			if (isListBox){
				float border = 20;
				Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
				Rect useScreenRect = new Rect(screenRect.xMin, screenRect.yMin, screenRect.width - border, screenRect.height - border);
				if (!wasInFocus){
					
					if (useScreenRect.Contains(mousePos)){
						OnClickList(mousePos, screenRect);
					}
					
				}
				if (Event.current.isMouse){
					if (Event.current.type == EventType.MouseDown){
						if (useScreenRect.Contains(mousePos)){
							OnClickList(mousePos, screenRect);
						}
					}
				}
				editor.SelectNone();
								//listBoxSelectedItem =  cursorPosLineNum;
			}
			else{
				listBoxSelectedItem = -1;
			}
			if (editor.pos != lastCursorPos && !isListBox){
				int borderLetters = 1;
				
				// our rect is thre size of the screen rect. need to move this as little as possible to get the cursor confortably inside it
				if (cursorPosVec.x - borderLetters * letterWidth < scrollPosition.x){
					scrollPosition.x = cursorPosVec.x - borderLetters * letterWidth;
					
				}
				if (cursorPosVec.x  + (2 + borderLetters) * letterWidth > scrollPosition.x + screenRect.width){
					scrollPosition.x = cursorPosVec.x - screenRect.width + (2 + borderLetters) * letterWidth ;
						
				}
				if (cursorPosVec.y - borderLetters * letterHeight < scrollPosition.y){
					scrollPosition.y = cursorPosVec.y - borderLetters * letterHeight;
						
				}
				if (cursorPosVec.y + (2 + borderLetters) * letterHeight > scrollPosition.y + screenRect.height){
					scrollPosition.y = cursorPosVec.y - screenRect.height + (2 + borderLetters) * letterHeight ;
					
				}
				

				
			}

//			if (isListBox){
//				int countToStart = 0;
//				editor.MoveLeft();
//				
//				while (editor.pos - countToStart != -1 && textToEdit[editor.pos - countToStart] != '\n'){
//					countToStart++;
//				}
//				countToStart--;
//				
//				
//				int countToEnd = 0;
//				while (editor.pos + countToEnd != textToEdit.Length && textToEdit[editor.pos + countToEnd] != '\n'){
//					countToEnd++;
//				}
//				
//				
//				for (int i = 0; i < countToStart; ++i){
//					editor.MoveLeft();
//				}
//				
//				for (int i = 0; i < countToStart + countToEnd; ++i){
//					editor.SelectRight();
//				}
//				
//				
//			}
			
			// If we are a list box - make the whoel ine that the cursor is on selected

			
			
			#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
			crazyEnterAdded = DealwithCrazyMacKeyDownBug(editor.pos);
			#endif
			
			lastCursorPos = editor.pos;
			wasInFocus = true;
		}
		else{
			wasInFocus = false;
		}

		
		//		Rect viewRect = new Rect(0, 0, stringDims.x, stringDims.y);
		Rect viewRect = new Rect(0, 0, Mathf.Max (screenRect.width, stringDims.x), Mathf.Max (screenRect.height, stringDims.y));
		
		if (isSingleLine){
			viewRect = screenRect;
		}
		
		if (!isSingleLine) scrollPosition = GUI.BeginScrollView(screenRect, scrollPosition, viewRect);
		GUI.SetNextControlName(ConstructControlName());
		string newText;
		if (isSingleLine){
			newText = GUI.TextField(viewRect, textToShow, style);
		}
		else{
			 newText = GUI.TextArea(viewRect, textToShow, style);
		}
		if (!isSingleLine) GUI.EndScrollView();
		
		if (crazyEnterAdded){
			GetActiveEditor().MoveRight();
		}
		
	
		
		
		if (GUI.GetNameOfFocusedControl() == ConstructControlName()){
			if (isEditable){
				GUI.skin.settings.cursorColor = Color.white;
				textToEdit = newText;
			}
			else{
				GUI.skin.settings.cursorColor = Color.black;
			}

		}

		
	}
	
	bool DealwithCrazyMacKeyDownBug(int cursorPos){
		bool crazyEnterAdded = false;
		TextEditor editor = GetActiveEditor();
		
		if (Event.current.keyCode !=  KeyCode.None){
			// Deal with Arrow keys
			if (Event.current.type == EventType.KeyUp){
				keyUpCounter++;
				if (keyUpCounter % 2 == 1){
					
					if (Event.current.keyCode == KeyCode.LeftArrow){
						//textToEdit += "Left: " + keyUpCounter + "\n";
						editor.MoveLeft();
						
					}
					if (Event.current.keyCode == KeyCode.RightArrow){
						editor.MoveRight();
						//textToEdit += "Right: " + keyUpCounter + "\n";
					}
					if (Event.current.keyCode == KeyCode.UpArrow){
						editor.MoveUp();
						//textToEdit += "Up: " + keyUpCounter + "\n";
					}
					if (Event.current.keyCode == KeyCode.DownArrow){
						editor.MoveDown();
						//textToEdit += "Down: " + keyUpCounter + "\n";
					}
				}
				
				
			}
			if (Event.current.type == EventType.KeyDown){ 
				if (Event.current.keyCode == KeyCode.Return){
					//editor.MoveDown();
					textToEdit = textToEdit.Insert(cursorPos, "\n");
					crazyEnterAdded = true;
					//textToEdit += "cursorPos = " + cursorPos + "\n";
				}
			}
			
			if (lastKeyCode != Event.current.keyCode){
				keyUpCounter = 0;
			}
			
			
			if (Event.current.keyCode != KeyCode.None){
				lastKeyCode = Event.current.keyCode;
			}
		}
		return crazyEnterAdded;
		
		
	}
	
	string CreateColorString(Color col){
		int red = (int)(col.r * 255);
		int green = (int)(col.g * 255);
		int blue = (int)(col.b * 255);
		return string.Format("#{0:x2}{1:x2}{2:x2}", red, green, blue);
	}
}
