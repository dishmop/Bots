using UnityEngine;
using System.Collections;

public class WaveCamera : MonoBehaviour {
	public Shader waveSetup;
	public GameObject quadGO;
	
	public bool setup = true;

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().enabled = false;
		GetComponent<Camera>().RenderWithShader(waveSetup, "");
		
		quadGO.GetComponent<Renderer>().material.SetFloat ("_Width", GetComponent<Camera>().targetTexture.width);
		quadGO.GetComponent<Renderer>().material.SetFloat ("_Height", GetComponent<Camera>().targetTexture.height);
	}
	
	// Update is called once per frame
	void Update () {
		if (setup){
			GetComponent<Camera>().RenderWithShader(waveSetup, "");
		}
		else{
			GetComponent<Camera>().Render();
		}
	
	}
}
