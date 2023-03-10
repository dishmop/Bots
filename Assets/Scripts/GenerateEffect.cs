using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GenerateEffect : MonoBehaviour {

	public float fadeInDuration = 0.5f;
	public float fadeOutDuration = 0.5f;
	public Color col1;
	public Color col2;
	public float magScale = 1.1f;
	
	
	enum State{
		kInactive,
		kInitialise,
		kWaitForSpace,
		kFadeEffectIn,
		kShowBot,
		kFadeEffectOut,
		kFinaliseEffect,
		kAwaitingDetach,
		kActivateBot
	};
	
	State state = State.kInactive;
	List<GameObject> effectObjects = new List<GameObject>();
	
	float stateStartTime = 0;
	
	public bool IsConstructing(){
		return state != State.kWaitForSpace && state != State.kInactive && state != State.kInitialise;
	}

	public void InitialiseEffect(float duration){
		fadeInDuration = duration * 0.5f;
		fadeOutDuration = duration * 0.5f;
		state = State.kInitialise;
		GetComponent<BotBot>().SetBotVisible(false);
		GetComponent<BotBot>().SetBotActive(false);

	}
	
	void CreateEffectGeometry(){
		// Create spheres
		string useName = "effectSphere";
		int count = 0;
		foreach (GameObject moduleGO in GetComponent<BotBot>().modulesToModuleGOs.Values){
			GameObject effectSphere = GameObject.Instantiate(BotFactory.singleton.generateEffectSpherePrefab);
			effectSphere.name = useName + "_" + count++;
			effectSphere.transform.SetParent(transform);
			effectSphere.transform.position = moduleGO.transform.position;
			effectSphere.transform.rotation = moduleGO.transform.rotation;
			effectSphere.transform.localScale = moduleGO.transform.localScale * magScale;
			
			effectObjects.Add (effectSphere);
		}
		// Generate rods
		foreach (GameObject[] rodArray in GetComponent<BotBot>().modulesToRodGOs.Values){
			for (int i = 0; i < 3; ++i){
				GameObject moduleGO = rodArray[i];
				if (moduleGO != null){
					GameObject effectRod = GameObject.Instantiate(BotFactory.singleton.generateEffectRodPrefab);
					effectRod.transform.SetParent(transform);
					effectRod.transform.position = moduleGO.transform.position;
					effectRod.transform.rotation = moduleGO.transform.rotation;
					effectRod.transform.localScale = moduleGO.transform.localScale * magScale;
					effectObjects.Add (effectRod);
				}	
			}
		}
		//SetRenderQueue(Balancing.singleton.effectRenderQueueNum);
	}
	
	void SetRenderQueue(int queueNum){
		foreach (GameObject go in effectObjects){
			//go.GetComponent<Renderer>().material.renderQueue = queueNum;
			Debug.Log (go.name + ": " + go.GetComponent<Renderer>().material.renderQueue);
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
		//SetRenderQueue(Balancing.singleton.effectRenderQueueNum);
		switch (state){
		case State.kInitialise:{
			CreateEffectGeometry();
			stateStartTime = Time.time;
			state = State.kWaitForSpace;
			break;
		}
		case State.kWaitForSpace:{
			if (!GetComponent<BotBot>().isOverlapTriggering){
				
				GetComponent<BotBot>().SolidifyColliders();
				state = State.kFadeEffectIn;
			//	Debug.Log ("<color=red>" + Time.fixedTime + ": " + gameObject.name + ": state = State.kFadeEffectIn </color>");
			}
			else{
				//Debug.Log (Time.fixedTime + ": " + gameObject.name + ": Waiting for space");
			}
			break;
		
		}
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
				if (transform.parent != null){
					transform.parent.GetComponent<BotConstructor>().OnConstructionReady();
				}
				state = State.kAwaitingDetach;
				break;
			}
			case State.kAwaitingDetach:{
				if (transform.parent == null){
					state = State.kInactive;
				}
				break;
			}


		}
	}
}
