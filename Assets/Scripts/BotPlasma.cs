using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BotPlasma : BotModule {

	public Plasma plasma;
	

	
	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		
		float baseHeat = module.GetMaxKelvin() * (module.volume * module.GetVolumetricHeatCapacity()) / Balancing.singleton.heatToTempMul;
		
		
		

		module.heatEnergy = baseHeat;
	
		
		requestedPower = 20f * transform.localScale.x;

		
	}
	
	
	public override void OnGameCollision(){
		toBeDestroyed = true;
	}
	
	
	
	public override void GameUpdatePostPowerCalc ()
	{
		if (!MathUtils.FP.Feq(requestedPower, availablePower)){
			toBeDestroyed = true;
		}
		usedPower = availablePower;
	
	}
	
	
	public override void HandleHeat(){
		if (IsAttachedToConstructor()) return ;
		
		float temp = 0.5f;
		Color heatGlow = CalcHeatGlow(temp);
		if (GetComponent<Renderer>() != null){
			GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
			Color col = GetComponent<Renderer>().material.GetColor("_Color");
			col.a = temp;
			GetComponent<Renderer>().material.SetColor("_Color", col);
		}
		else{
			transform.FindChild("Model").GetComponent<Renderer>().material.SetColor("_EmissionColor", heatGlow);
			
			Color col = transform.FindChild("Model").GetComponent<Renderer>().material.GetColor("_Color");
			col.a = temp;
			transform.FindChild("Model").GetComponent<Renderer>().material.SetColor("_Color", col);
		}
		
		
	
	}
	
	
	// In most cases this just ups the temperature
	public override void HandleRadiation(){

		
	}
	
	
	

}
