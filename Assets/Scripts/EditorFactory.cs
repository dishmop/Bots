using UnityEngine;
using System.Collections;

public class EditorFactory : MonoBehaviour {
	public static EditorFactory singleton = null;
	public GameObject moduleButtonPrefab = null;
	public GameObject modulePicturePrefab = null;	
	
	Module[] modules = new Module[(int)ModuleType.kNumTypes];
	
	public string GameModuleName(ModuleType type){
		return modules[(int)type].GetTypeName();
		
	}
	
	public GameObject ConstructEditorPicture(Module module){
		GameObject go = GameObject.Instantiate(modulePicturePrefab);
		EditorModulePicture picture = go.GetComponent<EditorModulePicture>();
		picture.moduleType = module.GetModeulType();
		picture.textColor = Editor.singleton.textOverColor;
		picture.lineWidth = Editor.singleton.pencilLineWidthLight;
		
		return go;
	}
	
	public Module ConstructModule(ModuleType type){
		switch (type){
			case ModuleType.kCell:{
				return new Cell();
			}
			case ModuleType.kEngine:{
				return new Engine();
			}
		}
		return null;
	}
	
	public Module ConstructModule(ModuleType type, Module parent, int spokeId){
		switch (type){
			case ModuleType.kCell:{
				return new Cell();
			}
			case ModuleType.kEngine:{
				return new Engine();
			}
		}
		return null;
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
		
		modules[(int)ModuleType.kCell] = new Cell();
		modules[(int)ModuleType.kEngine] = new Engine();
	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
