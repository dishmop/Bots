using UnityEngine;
using System.Collections;
using Vectrosity;

public class Editor : MonoBehaviour {

	public static Editor singleton = null;
	public GameObject editorCamera;
	public Material pencilLine;
	public Material pencilLineDotted;
	public float pencilLineWidthLight = 10;
	public float textureScale = 1;
	public Color textNormalColor;
	public Color textOverColor;
	public Color textActiveColor;
	
	public float GetLinePencilLightWidth(){
		return pencilLineWidthLight/(float)editorCamera.GetComponent<Camera>().orthographicSize;
	}
	public float GetLinePencilMediumWidth(){
		return 2 * pencilLineWidthLight/(float)editorCamera.GetComponent<Camera>().orthographicSize;
	}
	public float GetLinePencilHeavyWidth(){
		return 4 * pencilLineWidthLight/(float)editorCamera.GetComponent<Camera>().orthographicSize;
	}
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake(){
		// Singleton
		if (singleton != null) Debug.LogError ("Error assigning singleton");
		singleton = this;
		
		// Other stuff
		VectorLine.SetCamera3D(editorCamera.GetComponent<Camera>());
		
	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
