using UnityEngine;
using System.Collections;

public class BotEngine : BotModule {
	public Engine engine;
	public float readPower;
	Vector3 propForce = Vector3.zero;

	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		
		GetComponent<Collider2D>().enabled = transform.parent.GetComponent<BotBot>().isBotActive;
		if (!transform.parent.GetComponent<BotBot>().isBotActive) return;
		requestedPower = engine.CalcPowerRequirements(engine.desAmount);
		
	
	}
	
	public override void GameUpdatePostPowerCalc ()
	{
		if (float.IsNaN(availablePower)){
			Debug.Log ("Error");
		}
		readPower = availablePower * Balancing.singleton.enginePowerToForceMultiplier;
		propForce = transform.rotation * new Vector3(0, readPower);
		usedPower = availablePower;
	}
	
	
	public override void FixedUpdate(){
		base.FixedUpdate();
		if (transform.parent.GetComponent<Rigidbody2D>() != null){
			transform.parent.GetComponent<Rigidbody2D>().AddForceAtPosition(propForce, transform.position);
		}
		//Debug.DrawLine(transform.position,  transform.position + 0.1f * propForce, Color.red);
	}
}
