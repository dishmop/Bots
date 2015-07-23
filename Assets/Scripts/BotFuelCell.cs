using UnityEngine;
using System.Collections;

public class BotFuelCell : MonoBehaviour {
	public FuelCell fuelCell;
	

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		
	}
}
