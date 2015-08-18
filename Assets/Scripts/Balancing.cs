using UnityEngine;
using System.Collections;

public class Balancing : MonoBehaviour {
	public static Balancing singleton = null;

	//
	public enum ModuleShape{
		kHemisphere,
		kDisc,
	};	
	
	public ModuleShape moduleShape = ModuleShape.kDisc;
	
	public float ConvertModuleSizeToRadius(float size){
		switch (moduleShape){
			case ModuleShape.kHemisphere:{
				return Mathf.Pow(size, 1f/3f);
			}
			case ModuleShape.kDisc:{
				return Mathf.Pow(size, 1f/2f);
			}
		}
		return 0f;
	}



	
	
	void Awake(){
		// Singleton
		if (singleton != null) Debug.LogError ("Error assigning singleton");
		singleton = this;
		
		
	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
