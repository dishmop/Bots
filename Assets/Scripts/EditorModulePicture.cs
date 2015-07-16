using UnityEngine;
using System.Collections;
using Vectrosity;

public class EditorModulePicture : MonoBehaviour {
	public ModuleType moduleType;
	public GameObject TextGO;
	public Color textColor;
	public float lineWidth;
	VectorCircle circle;

	// Use this for initialization
	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		float left = 	mesh.bounds.min.x;
		float right = 	mesh.bounds.max.x;
		float bottom = 	mesh.bounds.min.y;
		float top = 	mesh.bounds.max.y;
		float width = 	right - left;
		float height = 	top - bottom;
		float radius = 	0.5f * Mathf.Min (width, height);	
		Vector3 centre = new Vector3(0.5f * (left + right), 0.5f *(top + bottom), 0);
		
		circle = new VectorCircle("Module circle", centre, radius, new Vector3 (0, 0, 1), Editor.singleton.pencilLine, Editor.singleton.GetLinePencilLightWidth());
		circle.textureScale = Editor.singleton.textureScale;
		GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		circle.drawTransform = transform;
		circle.lineWidth = lineWidth;
		circle.Draw3D();
		
		TextGO.GetComponent<TextMesh>().text = EditorFactory.singleton.GameModuleName(moduleType);
		TextGO.GetComponent<TextMesh>().color = textColor;
	}
	
	void OnDestroy(){
		VectorLine circleLine = circle as VectorLine;
		VectorLine.Destroy(ref circleLine);
	}
}
