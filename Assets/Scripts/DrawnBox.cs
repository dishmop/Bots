using UnityEngine;
using System.Collections;
using Vectrosity;

public class DrawnBox : MonoBehaviour {
	public Color color;
	VectorLine boxLine;
	
	// Use this for initialization
	void Start () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		float left = 	mesh.bounds.min.x;
		float right = 	mesh.bounds.max.x;
		float bottom = 	mesh.bounds.min.y;
		float top = 	mesh.bounds.max.y;
		
		Vector3[] points = new Vector3[5];
		points[0] = new Vector3(left, bottom, 0);
		points[1] = new Vector3(left, top, 0);
		points[2] = new Vector3(right, top, 0);
		points[3] = new Vector3(right, bottom, 0);
		points[4] = new Vector3(left, bottom, 0);
		
		boxLine = new VectorLine("Button box", points, Editor.singleton.pencilLine, Editor.singleton.GetLinePencilHeavyWidth(), LineType.Continuous, Joins.Weld);
		boxLine.textureScale = 16;
		boxLine.drawTransform = transform;
		boxLine.textureScale = Editor.singleton.textureScale;
		
		GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		boxLine.drawTransform = transform;
		boxLine.lineWidth = Editor.singleton.GetLinePencilHeavyWidth();
		boxLine.color = color;
		boxLine.Draw3D();
	
	}
}
