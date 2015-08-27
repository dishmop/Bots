using UnityEngine;
using System.Collections;

public class BotExplosion : MonoBehaviour {

	public float size = 1;
	
	public int count = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Trigger(float size){
		this.size = size;
		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (count < 0) return;
	
		ParticleSystem particleSystem = GetComponent<ParticleSystem>();
		particleSystem.maxParticles = Mathf.Max (4, (int)(Mathf.Pow(size, 1.2f) * 10));
		particleSystem.emissionRate = particleSystem.maxParticles * 10;
		particleSystem.startSpeed = size * 4;
		
		if (count > 10) particleSystem.emissionRate = 0;
		if (count > 1000){
			GameObject.Destroy (gameObject);
		}
		++count;
	
	}
}
