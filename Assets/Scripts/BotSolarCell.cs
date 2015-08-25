using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BotSolarCell : BotModule {

	public SolarCell solarCell;
	
	public Color ringColor;
	public float activity = 0;
	
	public override void Start(){
		base.Start ();
		transform.FindChild("Particle System").GetComponent<ParticleSystem>().emissionRate = 0;
		transform.FindChild("Model").GetComponent<Renderer>().material.EnableKeyword ("_EMISSION");
	}
	
	
	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		
		float maxEnergyPerFrame = module.volume * 0.25f;
		
		float propUsage = activity / GetMaxEnergyPerFrame();
		
		Color thisRingColor = transform.FindChild("Model").GetComponent<Renderer>().material.GetColor("_EmissionColor");
		Color targetCol = Color.Lerp(Color.black, ringColor, propUsage);
		Color useColor = Color.Lerp (thisRingColor, targetCol, 0.1f);
		transform.FindChild("Model").GetComponent<Renderer>().material.SetColor("_EmissionColor", useColor);
		transform.FindChild("Particle System").GetComponent<ParticleSystem>().emissionRate = Mathf.Lerp(0, 1000, propUsage);
		
		availableGiftedFuel = 0.25f * activity;
		
	}
	
	
	public override void GameUpdatePostPowerCalc ()
	{
		usedPower = availablePower;
	}
	
	
	float GetMaxEnergyPerFrame(){
		return module.volume * 0.5f;
		
	}

	

	
	public override void ApplyRadiation(float energy){
	
		float energyToConvert = Mathf.Min (GetMaxEnergyPerFrame(), energy);
		activity = energyToConvert;
		
		base.ApplyRadiation( (energy - activity));
		
		
	}
	
	

}
