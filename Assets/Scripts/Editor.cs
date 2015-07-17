using UnityEngine;
using System.Collections;
using Vectrosity;
using System;

public class Editor : MonoBehaviour {

	public static Editor singleton = null;
	public GameObject editorCamera;
	public Material pencilLine;
	public Material pencilCursorLine;
	public Material pencilLineDotted;
	public float pencilLineWidthLight = 10;
	public float textureScale = 1;

	[NonSerialized]
	public Color lightColor = new Color(1, 1, 1, 0.25f);
	[NonSerialized] 
	public Color mediumColor = new Color(1, 1, 1, 0.5f);
	[NonSerialized] 
	public Color heavyColor = new Color(1, 1, 1, 0.75f);
	
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
