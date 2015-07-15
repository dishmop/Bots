using UnityEngine;
using System.Collections;

public class Editor : MonoBehaviour {

	public static Editor singleton = null;
	public GameObject editorCamera;
	public float pencilZoffset = 0.01f;
	public Material pencilLineLight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake(){
		if (singleton != null) Debug.LogError ("Error assigning singleton");
		singleton = this;
	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
