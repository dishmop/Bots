using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Vectrosity;
using UnityEngine.Events;

public class EditorButton : MonoBehaviour {
	public GameObject pictureGO;
	public GameObject textGO;
	public ModuleType moduleType;
	public bool isRadio = true;
	
	public UnityEvent onActivate;
	
	
	float nonRadioActiveduraction = 0.2f;
	float activationTime = 0;

	
	
	public enum State{
		kNormal,
		kOver,
		kActive
	}
	
	State state = State.kNormal;

	
	public State GetState(){
		return state;
	}
	
	public void SetState(State newState){
		if ( state != State.kActive && newState == State.kActive){
			activationTime = Time.time;
			onActivate.Invoke();
		}
		state = newState;
		

	}
	

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
		if (!isRadio && state == State.kActive && Time.time > activationTime + nonRadioActiveduraction){
			SetState(State.kOver);
		}
		Color color = Color.white;
		switch (state){
			case State.kNormal:{
				//lineWidth = Editor.singleton.GetLinePencilLightWidth();
				
				//textColor = Editor.singleton.textNormalColor;
				color = Editor.singleton.lightColor;
				break;
			}
				case State.kOver:{
				//lineWidth = Editor.singleton.GetLinePencilMediumWidth();
				//textColor = Editor.singleton.textOverColor;
				color = Editor.singleton.mediumColor;
				break;
			}
				case State.kActive:{
				//lineWidth = Editor.singleton.GetLinePencilHeavyWidth();
				//textColor = Editor.singleton.textActiveColor;
				color = Editor.singleton.heavyColor;
				break;
			}
		}

		if (pictureGO){
			pictureGO.GetComponent<EditorModulePicture>().moduleType = moduleType;
			pictureGO.GetComponent<EditorModulePicture>().color = color;
			pictureGO.GetComponent<EditorModulePicture>().rodCol = color;
		}
		GetComponent<DrawnBox>().color = color;
		if (textGO){
			textGO.GetComponent<TextMesh>().color = color;
		}
		
	}
		
}
