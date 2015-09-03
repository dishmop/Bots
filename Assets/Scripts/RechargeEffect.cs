using UnityEngine;
using System.Collections;

public class RechargeEffect : MonoBehaviour {

	public GameObject chargeeModuleGO;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!chargeeModuleGO) return;
		
		Vector3 hereToThere = chargeeModuleGO.transform.position - transform.position;
		float dist = hereToThere.magnitude;
		transform.rotation = Quaternion.FromToRotation(new Vector3(0, 1, 0), hereToThere);
		// 5 is the speed set in teh inspector
		GetComponent<ParticleSystem>().startLifetime = dist / 5;
		

		
	
	}
}
