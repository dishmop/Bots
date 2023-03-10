using UnityEngine;
using System.Collections;

public class BotEngine : BotModule {
	public Engine engine;
	public float readPower;
	Vector3 propForce = Vector3.zero;

	
	// Update is called once per frame
	public override void GameUpdate () {
		base.GameUpdate();
		
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
		if (engine.desAmount < 0) propForce *= -1;
		usedPower = availablePower;
	}
	
	void LateUpdate(){
		SetParticleVelocities( -0.1f * propForce);
		
		                                
	}

	
	
	public override void FixedUpdate(){
		base.FixedUpdate();
		if (transform.parent.GetComponent<Rigidbody2D>() != null){
			transform.parent.GetComponent<Rigidbody2D>().AddForceAtPosition(propForce, transform.position);
		}
		Debug.DrawLine(transform.position,  transform.position + 0.1f * propForce, Color.red);
	}
	
	public void SetParticleVelocities(Vector3 vel){
		ParticleSystem particleSystem = transform.FindChild("Particle System").GetComponent<ParticleSystem>();
		ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystem.particleCount+1];
		int numParticles = particleSystem.GetParticles(p);
		
		
		for (int i = 0; i < numParticles; ++i) {
			p[i].velocity = vel;
		}
		
		particleSystem.SetParticles(p, numParticles);    
		
		// Set the rate too
		particleSystem.emissionRate = Mathf.Lerp (0, 1000, 0.05f * vel.magnitude);
		
		}
	
}
