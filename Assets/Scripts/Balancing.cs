using UnityEngine;
using System.Collections;

public class Balancing : MonoBehaviour {
	public static Balancing singleton = null;
	
	public float enginePowerToForceMultiplier = 200;
	public int effectRenderQueueNum = 3000;
	public float heatFromFieldMul = 0;
	public float heatToRemoveProp = 0;
	public float heatToTempMul = 2;
	public float heatToTempMulSurf = 0;
	public float SurfToFieldMul = 1;

	//
	public enum ModuleShape{
		kHemisphere,		// actually a sphere - but equivilent to hemisphere of twice the density
		kDisc,
	};	
	
	public ModuleShape moduleShape = ModuleShape.kDisc;
	
	public float ConvertModuleVolumeToRadius(float size){
		switch (moduleShape){
			case ModuleShape.kHemisphere:{
				return Mathf.Pow(size, 1f/3f);
			}
			case ModuleShape.kDisc:{
				return Mathf.Pow(size, 1f/2f);
			}
		}
		return 0f;
	}
	
	// Multiply this by the density to get the moment of intertia around centre
	public float ConvertVolumeToUnitMomentOfInertia(float size){
		float radius = ConvertModuleVolumeToRadius(size);
		
		switch (moduleShape){
			case ModuleShape.kHemisphere:{
				return (2f/5f) * radius * radius;
			}
			case ModuleShape.kDisc:{
				return 0.5f * radius * radius;
			}
		}
		return 0f;
	}

	
	
	void Awake(){
		// Singleton
		if (singleton != null) Debug.LogError ("Error assigning singleton");
		singleton = this;
		
		
	}
	
	void OnDestroy(){
		singleton = null;
	}
}
