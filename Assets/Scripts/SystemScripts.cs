using UnityEngine;
using System.Collections;

public class SystemScripts : MonoBehaviour {

	public static SystemScripts singleton = null;
	
	public Object[] systemScripts;
	
	void FixedUpdate(){
	
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
