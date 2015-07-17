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
	VectorLine boxLine;
	
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
	
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		float left = 	mesh.bounds.min.x;
		float right = 	mesh.bounds.max.x;
		float bottom = 	mesh.bounds.min.y;
		float top = 	mesh.bounds.max.y;
		
		Vector3[] points = new Vector3[5];
		points[0] = new Vector3(left, bottom, 0);
		points[1] = new Vector3(left, top, 0);
		points[2] = new Vector3(right, top, 0);
		points[3] = new Vector3(right, bottom, 0);
		points[4] = new Vector3(left, bottom, 0);
		
		boxLine = new VectorLine("Button box", points, Editor.singleton.pencilLine, Editor.singleton.GetLinePencilHeavyWidth(), LineType.Continuous, Joins.Weld);
		boxLine.textureScale = 16;
		boxLine.drawTransform = transform;
		boxLine.textureScale = Editor.singleton.textureScale;
		
		GetComponent<MeshRenderer>().enabled = false;
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
		boxLine.drawTransform = transform;
		boxLine.lineWidth = Editor.singleton.GetLinePencilHeavyWidth();
		boxLine.color = color;
		boxLine.Draw3D();
		if (pictureGO){
			pictureGO.GetComponent<EditorModulePicture>().moduleType = moduleType;
			pictureGO.GetComponent<EditorModulePicture>().color = color;
			pictureGO.GetComponent<EditorModulePicture>().rodCol = color;
		}
		if (textGO){
			textGO.GetComponent<TextMesh>().color = color;
		}
		
	}
		
}
