using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateEffect : MonoBehaviour {

	public float fadeInDuration = 0.5f;
	public float fadeOutDuration = 0.5f;
	public Color col1;
	public Color col2;
	
	enum State{
		kInactive,
		kInitialise,
		kFadeEffectIn,
		kShowBot,
		kFadeEffectOut,
		kFinaliseEffect,
		kActivateBot
	};
	
	State state = State.kInactive;
	List<GameObject> effectObjects = new List<GameObject>();
	
	float stateStartTime = 0;
	

	public void InitialiseEffect(){
		state = State.kInitialise;
		GetComponent<BotBot>().SetBotVisible(false);
		GetComponent<BotBot>().SetBotActive(false);
		CreateEffectGeometry();
		stateStartTime = Time.time;
		state = State.kFadeEffectIn;
	}
	
	void CreateEffectGeometry(){
		// Create spheres
		foreach (GameObject moduleGO in GetComponent<BotBot>().modulesToModuleGOs.Values){
			GameObject effectSphere = GameObject.Instantiate(BotFactory.singleton.generateEffectSpherePrefab);
			effectSphere.transform.SetParent(transform);
			effectSphere.transform.position = moduleGO.transform.position;
			effectSphere.transform.rotation = moduleGO.transform.rotation;
			effectObjects.Add (effectSphere);
		}
		// Generate rods
		foreach (GameObject moduleGO in GetComponent<BotBot>().modulesToRodGOs.Values){
			GameObject effectRod = GameObject.Instantiate(BotFactory.singleton.generateEffectRodPrefab);
			effectRod.transform.SetParent(transform);
			effectRod.transform.position = moduleGO.transform.position;
			effectRod.transform.rotation = moduleGO.transform.rotation;
			effectObjects.Add (effectRod);
		}
		
	}
	
	void PropogateColors(){
		foreach (GameObject effectGO in effectObjects){
			effectGO.GetComponent<Renderer>().material.SetColor("_Color0", col1);
			effectGO.GetComponent<Renderer>().material.SetColor("_Color1", col2);
		}
	}
	
	
	void DestroyEffectGeometry(){
		// Create spheres
		foreach (GameObject effectGO in effectObjects){
			GameObject.Destroy(effectGO);
		}
		effectObjects.Clear();
		
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state){
			case State.kFadeEffectIn:{
				float alpha = (Time.time -  stateStartTime) / fadeInDuration;
				alpha = Mathf.Pow(alpha, 3);
				col1.a = alpha;
				col2.a = alpha;
				PropogateColors();
				if (Time.time > stateStartTime + fadeInDuration){
					state = State.kShowBot;
				}
				break;
			}
			case State.kShowBot:{
				GetComponent<BotBot>().SetBotVisible(true);
				stateStartTime = Time.time;
				state = State.kFadeEffectOut;
				break;
			}
			case State.kFadeEffectOut:{
				float alpha = (Time.time -  stateStartTime) / fadeOutDuration;
				alpha = Mathf.Pow(alpha, 3);
				col1.a = 1 - alpha;
				col2.a = 1 - alpha;
				PropogateColors();
				if (Time.time > stateStartTime + fadeOutDuration){
					state = State.kFinaliseEffect;
				}
				
				break;
			}
			case State.kFinaliseEffect:{
				DestroyEffectGeometry();
				GetComponent<BotBot>().SetBotActive(true);
				break;
			}
		}
	}
}
