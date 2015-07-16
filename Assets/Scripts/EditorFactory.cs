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
