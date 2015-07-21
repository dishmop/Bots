using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BotBot : MonoBehaviour {
	public Bot bot;
	public Bounds bounds;
	
	Dictionary<Guid, GameObject> modulesToGO = new Dictionary<Guid, GameObject>();
	
	public void RegisterModule(Module module, GameObject go){
		modulesToGO.Add (module.guid, go);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
