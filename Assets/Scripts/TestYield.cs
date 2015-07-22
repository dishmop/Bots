using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestYield : MonoBehaviour {
	public static TestYield singleton = null;
	
	IEnumerator<int> enumerator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void RestartLoop(){
		enumerator = CallInternalLoop().GetEnumerator();
	}
	
	public void CallLoop(){
		
		enumerator.MoveNext();
	}
	
	IEnumerable<int> CallInternalLoop(){
		for(int i = 0; i < 100; ++i){
			Debug.Log ("Internal Loop: " + i);
			yield return i;
		}
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
