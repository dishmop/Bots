using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {

	public static GameMode singleton = null;
	
	public enum Mode{
		kInitialise,
		kFrontEnd,
		kPlay,
		kScript
	}
	
	public Mode mode = Mode.kInitialise;

	// Use this for initialization
	void Start () {
	
		mode = Mode.kScript;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == Mode.kScript){
			Camera.main.rect = new Rect(Balancing.singleton.scriptUIProp, 0, 1, 1 - Balancing.singleton.topMenuProp);
		}
		else{
			Camera.main.rect = new Rect(0, 0, 1, 1 - Balancing.singleton.topMenuProp);
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
