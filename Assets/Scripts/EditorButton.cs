using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Vectrosity;

public class EditorButton : MonoBehaviour {
	public GameObject pictureGO;
	public ModuleType moduleType;

	
	
	public enum State{
		kNormal,
		kOver,
		kActive
	}
	
	public State state = State.kNormal;
	

	VectorLine boxLine;
	
	public void OnPointerEnter(){
		Debug.Log("OnPointerEnter: Type = " + moduleType.ToString());
	}
	
	public void OnPointerExit(){
		Debug.Log("OnPointerExit: Type = " + moduleType.ToString());
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
		pictureGO.GetComponent<EditorModulePicture>().moduleType = moduleType;
		pictureGO.GetComponent<EditorModulePicture>().color = color;
		pictureGO.GetComponent<EditorModulePicture>().rodCol = color;
	}
		
}
