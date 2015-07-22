using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BotBot : MonoBehaviour {
	public Bot bot;
	public Bounds bounds;
	public Dictionary<Guid, GameObject> modulesToModuleGOs = new Dictionary<Guid, GameObject>();
	public Dictionary<Guid, GameObject> modulesToRodGOs = new Dictionary<Guid, GameObject>();
	public float speed = 0;
	
	bool isBotVisible = true;
	bool isBotActive = false;
	
	public void SetBotActive(bool active){
		isBotActive = active;
	}
	
	public void RegisterModule(Module module, GameObject go){
		modulesToModuleGOs.Add (module.guid, go);
	}
	
	public void RegisterRod(Module module, GameObject go){
		modulesToRodGOs.Add (module.guid, go);
	}
	
	public void SetBotVisible(bool visible){
		isBotVisible = visible;
		HandleVisibility();
	}
	
	void HandleVisibility(Transform thisTransform){
		if (thisTransform.GetComponent<MeshRenderer>() != null){
			thisTransform.GetComponent<MeshRenderer>().enabled = isBotVisible;
		}
		foreach (Transform child in thisTransform){
			HandleVisibility(child);
		}
		
	}


	void HandleVisibility(){
		foreach (GameObject go in modulesToModuleGOs.Values){
			HandleVisibility(go.transform);
			
		}
		foreach (GameObject go in modulesToRodGOs.Values){
			HandleVisibility(go.transform);
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		speed = GetComponent<Rigidbody2D>().velocity.magnitude;
	
	
	}
}
