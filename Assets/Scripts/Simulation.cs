using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation : MonoBehaviour {

	public static Simulation singleton = null;
	
	List<GameObject> bots = new List<GameObject>();
	
	public void RegisterBot(GameObject bot){
		bots.Add (bot);
	}
	
	public void UnregisterBot(GameObject bot){
		bots.Remove(bot);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		foreach (GameObject bot in bots){
			bot.GetComponent<BotBot>().GameUpdate();
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
