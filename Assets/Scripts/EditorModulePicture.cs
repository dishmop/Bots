using UnityEngine;
using System.Collections;
using Vectrosity;
using System;

public class EditorModulePicture : MonoBehaviour {
	public ModuleType moduleType;
	public GameObject TextGO;
	public Color textColor;
	public float lineWidth;
	public int spoke = -1;
	public float moduleSeperation = 0;
	public float rodWidth = 0.1f;
	public Guid guid;
	public bool moduleVisible = true;
	
	// guid of the object we are representing
	public Guid dataGuid = Guid.Empty;
	
	VectorCircle circle;
	VectorLine rod;
	
	
	float left;
	float right;
	float bottom;
	float top;
	float width;
	float height;
	float radius;
	
	public void SetSpoke(int spokeDir, float seperation){
		VectorLine.Destroy (ref rod);
		spoke = spokeDir;
		moduleSeperation = seperation;
		HandleSpoke();
	}

	// Use this for initialization
	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		left = 		mesh.bounds.min.x;
		right = 	mesh.bounds.max.x;
		bottom = 	mesh.bounds.min.y;
		top = 		mesh.bounds.max.y;
		width = 	right - left;
		height = 	top - bottom;
		radius = 	0.5f * Mathf.Min (width, height);	
		Vector3 centre = new Vector3(0.5f * (left + right), 0.5f *(top + bottom), 0);
		
		circle = new VectorCircle("Module circle", centre, radius, new Vector3 (0, 0, 1), Editor.singleton.pencilLine, Editor.singleton.GetLinePencilLightWidth());
		circle.textureScale = Editor.singleton.textureScale;
		
		HandleSpoke();

		GetComponent<MeshRenderer>().enabled = false;
	}
	

	
	void HandleSpoke(){
	
		if (spoke == -1){
			if (rod != null){
				VectorLine.Destroy (ref rod);
			}
			return;
		}
		
		if (rod == null){
			Vector3[] points = new Vector3[4];	
			rod = new VectorLine("rod", points, Editor.singleton.pencilLine, Editor.singleton.GetLinePencilLightWidth());
		}
		Vector3 dir = SpokeDirs.GetDirVector(spoke);
		Vector3 widthDir = Vector3.Cross(dir, new Vector3(0, 0, 1));
		float useSeperation = moduleSeperation / transform.localScale.x;
		Vector3 startPosCentre = dir * radius;
		Vector3 endPosCentre = dir * (useSeperation - radius);
		rod.points3[0] = startPosCentre + widthDir * rodWidth;
		rod.points3[1] = endPosCentre + widthDir * rodWidth;
		rod.points3[2] = startPosCentre - widthDir * rodWidth;
		rod.points3[3] = endPosCentre - widthDir * rodWidth;;
		rod.drawTransform = transform;
		rod.lineWidth = Editor.singleton.GetLinePencilMediumWidth();
		rod.Draw3D();
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		circle.active = moduleVisible;
		TextGO.SetActive(moduleVisible);
		if (moduleVisible){
			
			circle.drawTransform = transform;
			circle.lineWidth = lineWidth;
			circle.Draw3D();
			
			
			TextGO.GetComponent<TextMesh>().text = EditorFactory.singleton.GameModuleName(moduleType);
			TextGO.GetComponent<TextMesh>().color = textColor;
		}
		HandleSpoke();
		
	}
	
	void Awake(){
		guid = Guid.NewGuid();
	}
	
	void OnDestroy(){
		VectorLine circleLine = circle as VectorLine;
		VectorLine.Destroy(ref circleLine);
		VectorLine.Destroy(ref rod);
	}
}
