using UnityEngine;
using System.Collections;

public class BotMono : MonoBehaviour {

	public Bot bot;
	

	void ConstructExample(){
		Cell cell = new Cell();
		new Engine(cell, 1);
		new Engine(cell, 5);
		bot = new Bot(cell, "Sample Bot");
		
	}

	// Use this for initialization
	void Start () {
		ConstructExample();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
