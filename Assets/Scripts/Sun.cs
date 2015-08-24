using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.EnableKeyword ("_EMISSION");
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)));
	
	}
}
