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
		
		if (count == 4){
			float forceModifier = 1;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), size * 2);
			foreach (Collider2D collider in colliders){
				
				Vector3 colliderPos = collider.gameObject.transform.position;
				Vector3 fromCentreToCollider = colliderPos - transform.position;
				Vector2 forceDir = forceModifier * new Vector2(fromCentreToCollider.x, fromCentreToCollider.y);
				Vector2 impactPos = new Vector2(colliderPos.x, colliderPos.y);
				if (collider.attachedRigidbody != null){
					collider.attachedRigidbody.AddForceAtPosition(forceDir, impactPos, ForceMode2D.Impulse);
				}
				
			}
		}
		
		if (count > 10) particleSystem.emissionRate = 0;
		
		
		if (count > 20){
			GameObject.Destroy (gameObject);
		}
		++count;
	
	}
}
