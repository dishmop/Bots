using UnityEngine;
using System.Collections;

public class AetherSimCamera : MonoBehaviour {
	public Shader simSetup;
	public Shader simScatter;
	public Shader simPropagate;
	public Shader renderShader;
	public GameObject quadGO;
	public GameObject renderCamera;
	public RenderTexture tex1;	// Setup andf propogated into here
	public RenderTexture tex2; 	// Scatter into here
	public RenderTexture tex3;	// apply sources from here
	
	public int intTimer = 0;
		
	public bool setup = true;

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().enabled = false;
		GetComponent<Camera>().targetTexture = tex1;
		GetComponent<Camera>().RenderWithShader(simSetup, "");
		
		quadGO.GetComponent<Renderer>().material.SetFloat ("_Width", GetComponent<Camera>().targetTexture.width);
		quadGO.GetComponent<Renderer>().material.SetFloat ("_Height", GetComponent<Camera>().targetTexture.height);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

			
		if (setup){
			GetComponent<Camera>().targetTexture = tex1;
			GetComponent<Camera>().RenderWithShader(simSetup, "");
		}
		else{
			for (int i = 0; i < 4; ++i){
				GetComponent<Camera>().targetTexture = tex2;
				GetComponent<Camera>().RenderWithShader(simScatter, "");
				GetComponent<Camera>().targetTexture = tex1;
				GetComponent<Camera>().RenderWithShader(simPropagate, "");
				
				renderCamera.GetComponent<Camera>().RenderWithShader(renderShader, "");
			}
		}
	
	}
}
