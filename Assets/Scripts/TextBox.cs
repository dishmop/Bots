using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class TextBox : MonoBehaviour {

	public GameObject panel;
	public string textToEdit = "Starting text";
	public bool isEditable;
	public Font font;
	public Vector2 scrollPosition = Vector2.zero;
	public Texture2D blackTeture;
	
	int lastCursorPos = 0;
	
	float letterWidth;
	float margineSizeX;
	float letterHeight;
	float margineSizeY;
	
		
		
	

	// Use this for initialization
	void Start () {
	
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
		
		for (int i = 0; i < text.Length; ++i){
			if (i == cursorPos){
				cursorPosX = currentLineLen;
				cursorPosY = numLines-1;
				cursorDone = true;
			}
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
	
	Vector2 GUICalcStringDims(string text, GUIStyle style, int cursorPos, out Vector2 cursorPosVec){
		int numLines;
		int maxLineLen;
		int cursorPosX;
		int cursorPosY;
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
		return name + "_TextArea";
		
	}
	
	
	void OnGUI() {
		Vector3[] worldCorners = new Vector3[4];
		panel.GetComponent<RectTransform>().GetWorldCorners(worldCorners);
		
		// Convert to screen space of old GUI
		for (int i = 0; i < worldCorners.Count (); ++i){
			worldCorners[i].y = Screen.height - worldCorners[i].y;
			
		}
		
		GUIStyle style = GUI.skin.textArea;
		
		style.font = font;
		style.normal.background = null;
		style.focused.background = null;
		style.active.background = null;
		style.hover.background = null;
		style.wordWrap = false;
		
		
		
		// Find cursor pos in current active window - I suppose
		TextEditor currentTextEditor = GetActiveEditor();
		int cursorPos =  currentTextEditor.pos;
		
		Vector2 cursorPosVec;
		Vector2 stringDims = GUICalcStringDims(textToEdit, style, cursorPos, out cursorPosVec);
		
		Rect screenRect = new Rect(worldCorners[1], worldCorners[3] - worldCorners[1]);

		if (GUI.GetNameOfFocusedControl() == ConstructControlName()){
			if (cursorPos != lastCursorPos){
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
			lastCursorPos = cursorPos;
		}
		
//		Rect viewRect = new Rect(0, 0, stringDims.x, stringDims.y);
		Rect viewRect = new Rect(0, 0, Mathf.Max (screenRect.width, stringDims.x), Mathf.Max (screenRect.height, stringDims.y));
		
		scrollPosition = GUI.BeginScrollView(screenRect, scrollPosition, viewRect);
		GUI.SetNextControlName(ConstructControlName());
		string newText = GUI.TextArea(viewRect, textToEdit, style);
		GUI.EndScrollView();
		
		
		
		if (isEditable){
			GUI.skin.settings.cursorColor = Color.white;
			textToEdit = newText;
		}
		else{
			GUI.skin.settings.cursorColor = Color.black;
		}

		
	}
}
