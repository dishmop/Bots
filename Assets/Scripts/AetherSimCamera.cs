using UnityEngine;
using System.Collections;

public class AetherSimCamera : MonoBehaviour {
	public Shader simSetup;
	public Shader simScatter;
	public Shader simPropagate;
	public Shader simBlur;
	public Shader renderShader;
	public Shader renderPowerShader;
	public Shader sourceShader;
	public Shader sourceSinkShader;
	public GameObject quadGO;
	public GameObject renderCamera;
	public GameObject sourceCamera;
	public RenderTexture tex1;
	public RenderTexture tex2;
	public RenderTexture tex3;
	public RenderTexture tex4;
	public RenderTexture textTemp;
	public RenderTexture renderTex;	
	public Texture2D resultPicture;
	public GameObject aetherRenderPlane;
	
	public RenderTexture[] circularBuffer;
	public int bufferIndex = 0;
	const int numHistoryFrames =12;

	
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
		
		circularBuffer = new RenderTexture[numHistoryFrames];
		for (int i = 0; i < numHistoryFrames; ++i){
			circularBuffer[i] = new RenderTexture(tex1.width, tex1.height, 0, RenderTextureFormat.ARGBFloat);
			circularBuffer[i].Create();
		}
		
		for (int i = 0; i < numHistoryFrames; ++i){
			quadGO.GetComponent<Renderer>().material.SetTexture ("_History_" + i, circularBuffer[i]);
		}
		
		resultPicture = new Texture2D(tex1.width, tex1.height, TextureFormat.ARGB32, false);

	}

	
	// Update is called once per frame
	public void GameUpdate () {

		sourceCamera.GetComponent<Camera>().targetTexture = tex3;
		sourceCamera.GetComponent<Camera>().RenderWithShader(sourceShader, "");

		sourceCamera.GetComponent<Camera>().targetTexture = tex4;
		sourceCamera.GetComponent<Camera>().RenderWithShader(sourceSinkShader, "");

		for (int i = 0; i < 2; ++i){
			quadGO.GetComponent<Renderer>().material.SetInt ("_IntTime", intTimer++);
			
			
			GetComponent<Camera>().targetTexture = tex2;
			GetComponent<Camera>().RenderWithShader(simScatter, "");
			
			GetComponent<Camera>().targetTexture = tex2;
			GetComponent<Camera>().RenderWithShader(simBlur, "");
			

			GetComponent<Camera>().targetTexture = tex1;
			GetComponent<Camera>().RenderWithShader(simPropagate, "");
			
			GetComponent<Camera>().targetTexture = circularBuffer[bufferIndex];
			GetComponent<Camera>().RenderWithShader(renderShader, "");
			
			GetComponent<Camera>().targetTexture = renderTex;
			quadGO.GetComponent<Renderer>().material.SetInt ("_BufferIndex",bufferIndex);
			GetComponent<Camera>().RenderWithShader(renderPowerShader, "");	
			
			bufferIndex = (bufferIndex + 1) % 12;
		}
		
		// Transfer data from rendertexture to standard texture so we can read it
		RenderTexture.active = renderTex;
		
		resultPicture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
		resultPicture.Apply();
		//RenderTexture.active = null; // added to avoid errors 
		
		aetherRenderPlane.GetComponent<Renderer>().material.SetTexture("_EmissionMap", resultPicture);
	}
}
