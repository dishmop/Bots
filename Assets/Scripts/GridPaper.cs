using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class GridPaper : MonoBehaviour {
	public Color lineCol = Color.white;
	public float gridLinethickeness;
	float sideLen = 0.5f;
	Camera editorCamera;
	
	// Can't we get this automatically?
	float basePlaneHalfSide = 2.5f;
	
	
	List<VectorLine> lines = new List<VectorLine>();


	VectorLine ConstructGridLine(Vector3[] points){
		VectorLine line = new VectorLine("Vertical grid line", points, Editor.singleton.pencilLineLight, gridLinethickeness);
		line.textureScale = 16;
		line.drawTransform = transform;
		line.color = lineCol;
		line.Draw3D();
		return line;
		
	}

	void ConstructGrid(){
		Vector3[] points = new Vector3[2];
		VectorLine.SetCamera3D(editorCamera);
		
		// Used for the vertical lines
		float sideLenH = 0.5f * sideLen / Mathf.Tan (30 * Mathf.Deg2Rad);
		int numLinesH = (int)(2 * basePlaneHalfSide / sideLenH);
		
		// Used for the sloping lines
		float sideLenH2 = sideLenH * 2;
		float numLinesH2 = (int)(2 * basePlaneHalfSide / sideLenH2);
		float sideLenV2 = sideLenH2 * Mathf.Tan (30 * Mathf.Deg2Rad);
		int numLinesV2 =  (int)(2 * basePlaneHalfSide / sideLenV2);

		// Get the grid window extents		
		float width = numLinesH * sideLenH;
		float height = numLinesV2 * sideLenV2;
		float left = -basePlaneHalfSide;
		float bottom = -basePlaneHalfSide;
		float top = -basePlaneHalfSide + height;
		float right = -basePlaneHalfSide + width;
		
		// Vertical lines
		
		for (int i = 0; i < numLinesH + 1; ++i){
			points[0] = new Vector3(left + i * sideLenH, Editor.singleton.pencilZoffset, bottom);
			points[1] = new Vector3(left + i * sideLenH, Editor.singleton.pencilZoffset, top);
			lines.Add (ConstructGridLine(points));
		}

		// Right up lines		
		Vector2 rightUpDir2D = new Vector3(1, Mathf.Tan(30 * Mathf.Deg2Rad));
		
		// Along the bottom
		for (int i = 0; i < numLinesH2 + 1; ++i){
			Vector2 startPos2D = new Vector2(left + i * sideLenH2,  bottom);
		
			// Figure out how far we can go before hitting the top
			float distTop = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightUpDir2D, new Vector2(left, top), new Vector2(1, 0));
			float distRight = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightUpDir2D, new Vector2( right, bottom), new Vector2(0, 1));
			Vector2 endPos2D = startPos2D + Mathf.Min (distTop, distRight) * rightUpDir2D;
			
			points[0] = new Vector3(startPos2D.x, Editor.singleton.pencilZoffset, startPos2D.y);
			points[1] = new Vector3(endPos2D.x, Editor.singleton.pencilZoffset, endPos2D.y);
			lines.Add (ConstructGridLine(points));
		}
		
		// Along the left side
		// Don't include the first position (already done)
	 	for (int i = 1; i < numLinesV2; ++i){
			Vector2 startPos2D = new Vector2(left, bottom + i * sideLenV2);
			
			// Figure out how far we can go before hitting the top or the right
			float distTop = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightUpDir2D, new Vector2( left, top), new Vector2(1, 0));
			float distRight = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightUpDir2D, new Vector2( right, bottom), new Vector2(0, 1));
			Vector2 endPos2D = startPos2D + Mathf.Min (distTop, distRight) * rightUpDir2D;
			
			points[0] = new Vector3(startPos2D.x, Editor.singleton.pencilZoffset, startPos2D.y);
			points[1] = new Vector3(endPos2D.x, Editor.singleton.pencilZoffset, endPos2D.y);
			lines.Add (ConstructGridLine(points));
		}	
		
		// Right down lines
		Vector2 rightDownDir2D = new Vector3(1, -Mathf.Tan(30 * Mathf.Deg2Rad));
		
		// Along the top
		for (int i = 0; i < numLinesH2 + 1; ++i){
			Vector2 startPos2D = new Vector2(left + i * sideLenH2,  top);
			
			// Figure out how far we can go before hitting the bottom or the right
			float distBottom = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightDownDir2D, new Vector2( left, bottom), new Vector2(1, 0));
			float distRight = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightDownDir2D, new Vector2( right, bottom), new Vector2(0, 1));
			Vector2 endPos2D = startPos2D + Mathf.Min (distBottom, distRight) * rightDownDir2D;
			
			points[0] = new Vector3(startPos2D.x, Editor.singleton.pencilZoffset, startPos2D.y);
			points[1] = new Vector3(endPos2D.x, Editor.singleton.pencilZoffset, endPos2D.y);
			lines.Add (ConstructGridLine(points));
		}
		
		// Along the left side
		// Don't include the first position (already done)
		for (int i = 1; i < numLinesV2; ++i){
			Vector2 startPos2D = new Vector2( left, top -i * sideLenV2);
			
			// Figure out how far we can go before hitting the top or the right
			// Figure out how far we can go before hitting the bottom or the right
			float distBottom = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightDownDir2D, new Vector2( left, bottom), new Vector2(1, 0));
			float distRight = MathUtils.Geometry2D.IntersectRay2D(startPos2D, rightDownDir2D, new Vector2( right, bottom), new Vector2(0, 1));
			Vector2 endPos2D = startPos2D + Mathf.Min (distBottom, distRight) * rightDownDir2D;
			
			points[0] = new Vector3(startPos2D.x, Editor.singleton.pencilZoffset, startPos2D.y);
			points[1] = new Vector3(endPos2D.x, Editor.singleton.pencilZoffset, endPos2D.y);
			lines.Add (ConstructGridLine(points));
		}	
		
		// do the borders - The left is already done
		// Bottom
		points[0] = new Vector3(left, Editor.singleton.pencilZoffset, bottom);
		points[1] = new Vector3(right, Editor.singleton.pencilZoffset, bottom);
		lines.Add (ConstructGridLine(points));
		
		// Top
		points[0] = new Vector3(left, Editor.singleton.pencilZoffset, top);
		points[1] = new Vector3(right, Editor.singleton.pencilZoffset, top);
		lines.Add (ConstructGridLine(points));
		
			
		
		
		
	}

	// Use this for initialization
	void Start () {
		editorCamera = Editor.singleton.editorCamera.GetComponent<Camera>();
		ConstructGrid();
		
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (VectorLine line in lines){
			line.drawTransform = transform;
			line.lineWidth = gridLinethickeness/(float)editorCamera.orthographicSize;
			line.color = lineCol;
			line.Draw3D();
		}
		
	
	}
}
