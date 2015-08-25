using UnityEngine;
using System.Collections;

public class BotFuelCell : BotModule {
	public FuelCell fuelCell;

	
	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		
	}
}
