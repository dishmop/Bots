using UnityEngine;
using Vectrosity;

public class VectorCircle : VectorLine{


	float _radius;
	Vector3 _centre;
	Vector3 _planeNormal;
	
	const int numLines = 64;
	
	public float radius
	{
		get { return _radius; }
		set { _radius = radius; Reconstruct();}
	}
	
	public Vector3 centre
	{
		get { return _centre; }
		set { _centre = centre; Reconstruct();}
	}
	
	public Vector3 planeNormal
	{
		get { return _planeNormal; }
		set { _planeNormal = planeNormal; Reconstruct();}
	}
	

	
	void Reconstruct(){
		
		Vector3 normalisedNormal = _planeNormal.normalized;
		Vector3 xDir;
		if (Mathf.Abs(normalisedNormal.x) > 0.5f){
			xDir = new Vector3(normalisedNormal.y, -normalisedNormal.x, normalisedNormal.z);
		}
		else if (Mathf.Abs(normalisedNormal.y) > 0.5f){
			xDir = new Vector3(normalisedNormal.x, normalisedNormal.z, -normalisedNormal.y);
		}
		else{
			xDir = new Vector3(-normalisedNormal.z, normalisedNormal.y, normalisedNormal.x);
		}
		Vector3 yDir = Vector3.Cross(xDir, normalisedNormal);
		
		for (int i = 0; i < numLines; ++i){
			float angRad = 2 * Mathf.PI * (float)i / (float)(numLines - 1);
			Vector3 dir = Mathf.Sin(angRad) * xDir +  Mathf.Cos(angRad) * yDir;
			base.points3[i] = centre + radius * dir;
			
		}
		
		

		
	}
	


	public VectorCircle (string name, Vector3 centre, float radius, Vector3 planeNormal, Material material, float lineWidth) : 
		base(name, new Vector3[numLines], material, lineWidth, LineType.Continuous, Joins.Weld){
		_radius = radius;
		_centre = centre;
		_planeNormal = planeNormal;
		
		Reconstruct();
		 
		
	}
	
	
}
