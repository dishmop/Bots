using UnityEngine;
using System.Collections;
using System;

public class EditorFactory : MonoBehaviour {
	public static EditorFactory singleton = null;
	public GameObject moduleButtonPrefab = null;
	public GameObject modulePicturePrefab = null;	
	
	Module[] modules = new Module[(int)ModuleType.kNumTypes];
	
	public string GetModuleName(ModuleType type){
		return modules[(int)type].GetTypeName();
		
	}
	
	public string GetModuleShortName(ModuleType type){
		return modules[(int)type].GetShortTypeName();
		
	}
	
	public GameObject ConstructEditorPicture(Module module, bool isShort){
		GameObject go = GameObject.Instantiate(modulePicturePrefab);
		EditorModulePicture picture = go.GetComponent<EditorModulePicture>();
		picture.moduleType = module.GetModuleType();
		picture.color = Editor.singleton.heavyColor;
		picture.rodCol = Editor.singleton.heavyColor;
		picture.isShort = isShort;
		picture.dataGuid = module.guid;
		
		return go;
	}
	
	public GameObject ConstructEditorPicture(ModuleType moduleType, bool isShort){
		GameObject go = GameObject.Instantiate(modulePicturePrefab);
		EditorModulePicture picture = go.GetComponent<EditorModulePicture>();
		picture.moduleType = moduleType;
		picture.color = Editor.singleton.heavyColor;
		picture.rodCol = Editor.singleton.heavyColor;
		picture.isShort = isShort;
		picture.dataGuid = Guid.Empty;
		
		return go;
	}
	
	public Module ConstructModule(ModuleType type, Bot bot){
		switch (type){
			case ModuleType.kCell:{
				return new Cell(bot);
			}
			case ModuleType.kEngine:{
				return new Engine(bot);
			}
		}
		return null;
	}
	
	public Module ConstructModule(ModuleType type, Module parent, int spokeId){
		switch (type){
			case ModuleType.kCell:{
				return new Cell(parent, spokeId);
			}
			case ModuleType.kEngine:{
				return new Engine(parent, spokeId);
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
		
		modules[(int)ModuleType.kCell] = new Cell((Bot)null);
		modules[(int)ModuleType.kEngine] = new Engine((Bot)null);
	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
