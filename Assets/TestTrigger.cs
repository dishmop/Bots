using UnityEngine;
using System.Collections;

public class TestTrigger : MonoBehaviour {

	public bool isOverlapping = true;
	
	public bool isOverlappingThisFrame = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		isOverlapping = isOverlappingThisFrame;
		
		if (isOverlapping){
			Debug.Log("<color=red>" + Time.fixedTime + ": Overlap </color>");
		}
		else{
			Debug.Log("<color=blue>" + Time.fixedTime + ": No Overlap </color>");
		}
		
		isOverlappingThisFrame = false;
	
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		HandleTrigger(collider);
	}

	void OnTriggerStay2D(Collider2D collider){
		HandleTrigger(collider);
	}
	
	void HandleTrigger(Collider2D collider){
		isOverlappingThisFrame = true;
	}
}
