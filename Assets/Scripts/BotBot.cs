using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BotBot : MonoBehaviour {
	public Bot bot;
	public Bounds bounds;
	public Dictionary<Guid, GameObject> modulesToModuleGOs = new Dictionary<Guid, GameObject>();
	public Dictionary<Guid, GameObject> modulesToRodGOs = new Dictionary<Guid, GameObject>();
	
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


	void HandleVisibility(){
		foreach (GameObject go in modulesToModuleGOs.Values){
			go.GetComponent<MeshRenderer>().enabled = isBotVisible;
		}
		foreach (GameObject go in modulesToRodGOs.Values){
			go.GetComponent<MeshRenderer>().enabled = isBotVisible;
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
