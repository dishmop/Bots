using UnityEngine;
using System.Collections;

public class AetherSimCamera : MonoBehaviour {
	public Shader simSetup;
	public Shader simScatter;
	public Shader simPropagate;
	public Shader renderShader;
	public Shader sourceShader;
	public GameObject quadGO;
	public GameObject renderCamera;
	public GameObject sourceCamera;
	public RenderTexture tex1;
	public RenderTexture tex2;
	public RenderTexture tex3;
	
	public int intTimer = 0;
		

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().enabled = false;

		
		quadGO.GetComponent<Renderer>().material.SetFloat ("_Width", GetComponent<Camera>().targetTexture.width);
		quadGO.GetComponent<Renderer>().material.SetFloat ("_Height", GetComponent<Camera>().targetTexture.height);
		
		GetComponent<Camera>().targetTexture = tex1;
		GetComponent<Camera>().RenderWithShader(simSetup, "");
		
		GetComponent<Camera>().targetTexture = tex3;
		GetComponent<Camera>().RenderWithShader(simSetup, "");
		

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		sourceCamera.GetComponent<Camera>().RenderWithShader(sourceShader, "");

		for (int i = 0; i < 1; ++i){
			quadGO.GetComponent<Renderer>().material.SetInt ("_IntTime", intTimer++);
			GetComponent<Camera>().targetTexture = tex2;
			GetComponent<Camera>().RenderWithShader(simScatter, "");
			GetComponent<Camera>().targetTexture = tex1;
			GetComponent<Camera>().RenderWithShader(simPropagate, "");
			
			renderCamera.GetComponent<Camera>().RenderWithShader(renderShader, "");
		}
	
	}
}
