using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

public class GridPaper : MonoBehaviour {
	public bool isDotted = false;
	public class PlacementPoint{
		public PlacementPoint(Vector3 pos){
			this.pos = pos;
		}
		public Vector3 pos;
		public GameObject picture; 
		public PlacementPoint[] neighbouringPoints = new PlacementPoint[6];
		
	}
	public List<PlacementPoint>	placementPoints = new List<PlacementPoint>();
	public Rect				boundingRect;
	
	float sideLen = 1f/8f;	// in units of one width
	
	
	
	List<VectorLine> lines = new List<VectorLine>();


	public VectorLine ConstructGridLine(Vector3[] points){
		VectorLine line = new VectorLine("Vertical grid line", points, Editor.singleton.pencilLine, Editor.singleton.GetLinePencilLightWidth());
		line.textureScale =  Editor.singleton.textureScale;
		line.drawTransform = transform;
		
		return line;
	}
	

	void ConstructGrid(){
		Vector3[] points = new Vector3[2];
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		float left = mesh.bounds.min.x;
		float bottom = mesh.bounds.min.y;
		float targetWidth = mesh.bounds.max.x - left;
		float targetHeight = mesh.bounds.max.y - bottom;
		
		float useSideLen = sideLen * targetWidth;
		
		// Used for the vertical lines
		float sideLenH = 0.5f * useSideLen / Mathf.Tan (30 * Mathf.Deg2Rad);
		int numLinesH = (int)(targetWidth / sideLenH);
		
		// Used for the sloping lines
		float sideLenH2 = sideLenH * 2;
		float numLinesH2 = (int)(targetWidth/ sideLenH2);
		float sideLenV2 = sideLenH2 * Mathf.Tan (30 * Mathf.Deg2Rad);
		int numLinesV2 =  (int)(targetHeight / sideLenV2);

		// Get the grid window extents		
		float width = numLinesH * sideLenH;
		float height = numLinesV2 * sideLenV2;
		float top = bottom + height;
		float right = left + width;
		
		// Vertical lines
		for (int i = 0; i < numLinesH + 1; ++i){
			points[0] = new Vector3(left + i * sideLenH, bottom, 0);
			points[1] = new Vector3(left + i * sideLenH, top, 0);
			lines.Add (ConstructGridLine(points));
		}
		
		// While we are about it, construct the placement points
		for (int i = 0; i < numLinesH + 1; ++i){
			Vector3 botttomPos = new Vector3(left + i * sideLenH, bottom, 0);
			if ((i % 2) == 0){
				for (int j = 0; j < numLinesV2+1; ++ j){
					PlacementPoint newPoint = new PlacementPoint(botttomPos + j * useSideLen * new Vector3(0, 1, 0));
					placementPoints.Add(newPoint);
				}
			}
			else{
				for (int j = 0; j < numLinesV2; ++ j){
					PlacementPoint newPoint = new PlacementPoint(botttomPos + (j + 0.5f) * useSideLen * new Vector3(0, 1, 0));
					placementPoints.Add(newPoint);
				}
			}
		}
		for (int i = 0; i < placementPoints.Count(); ++i){
			placementPoints[i].pos = transform.TransformPoint(placementPoints[i].pos);
		}
		// Link the placement positions up with spokes so we cna navigate around them more easily
		int totCount = 0;
		int xCount = 0;
		for (int i = 0; i < numLinesH + 1; ++i){
			if ((i % 2) == 0){
				int yCount = 0;
				for (int j = 0; j < numLinesV2+1; ++ j){
					PlacementPoint thisPoint = placementPoints[totCount];
					// If not the top row
					if (yCount != numLinesV2){
						thisPoint.neighbouringPoints[0] = placementPoints[totCount + 1];
					}
					// If not the top row and not the right column
					if (yCount != numLinesV2 && xCount != numLinesH){
						thisPoint.neighbouringPoints[1] = placementPoints[totCount + numLinesV2 + 1];
					}
					// If not the bottom row and not the right column
					if (yCount != 0 && xCount != numLinesH){
						thisPoint.neighbouringPoints[2] = placementPoints[totCount + numLinesV2];
					}
					// If not the bottom row
					if (yCount != 0){
						thisPoint.neighbouringPoints[3] = placementPoints[totCount -1];
						
					}
					// If not the bottom row and not the left column
					if (yCount != 0 && xCount != 0){
						thisPoint.neighbouringPoints[4] = placementPoints[totCount - numLinesV2 - 1];
						
					}
					// If not the top row and not the left column
					if (yCount != numLinesV2 && xCount != 0){
						thisPoint.neighbouringPoints[5] = placementPoints[totCount - numLinesV2];
						
					}
					totCount++;
					yCount++;
				}
			}
			else{
				int yCount = 0;
				for (int j = 0; j < numLinesV2; ++ j){
					PlacementPoint thisPoint = placementPoints[totCount];
					// If not the top row
					if (yCount != numLinesV2-1){
						thisPoint.neighbouringPoints[0] = placementPoints[totCount + 1];
					}
					// If not the top row and not the right column
					if (yCount != numLinesV2-1 && xCount != numLinesH){
						thisPoint.neighbouringPoints[1] = placementPoints[totCount + numLinesV2 + 1];
					}
					// If not the bottom row and not the right column
					if (yCount != 0 && xCount != numLinesH){
						thisPoint.neighbouringPoints[2] = placementPoints[totCount + numLinesV2];
					}
					// If not the bottom row
					if (yCount != 0){
						thisPoint.neighbouringPoints[3] = placementPoints[totCount -1];
						
					}
					// If not the bottom row and not the left column
					if (yCount != 0 && xCount != 0){
						thisPoint.neighbouringPoints[4] = placementPoints[totCount - numLinesV2 - 1];
						
					}
					// If not the top row and not the left column
					if (yCount != numLinesV2-1 && xCount != 0){
						thisPoint.neighbouringPoints[5] = placementPoints[totCount - numLinesV2];
						
					}				
					totCount++;
					yCount++;
				}
			}
			xCount++;
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
			
			points[0] = new Vector3(startPos2D.x, startPos2D.y, 0);
			points[1] = new Vector3(endPos2D.x, endPos2D.y, 0);
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
			
			points[0] = new Vector3(startPos2D.x, startPos2D.y, 0);
			points[1] = new Vector3(endPos2D.x, endPos2D.y, 0);
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
			
			points[0] = new Vector3(startPos2D.x, startPos2D.y, 0);
			points[1] = new Vector3(endPos2D.x, endPos2D.y, 0);
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
			
			points[0] = new Vector3(startPos2D.x, startPos2D.y, 0);
			points[1] = new Vector3(endPos2D.x, endPos2D.y, 0);
			lines.Add (ConstructGridLine(points));
		}	
		
		// do the borders - The left is already done
		// Bottom
		points[0] = new Vector3(left, bottom, 0);
		points[1] = new Vector3(right, bottom, 0);
		lines.Add (ConstructGridLine(points));
		
		// Top
		points[0] = new Vector3(left, top, 0);
		points[1] = new Vector3(right, top, 0);
		lines.Add (ConstructGridLine(points));
		
		Vector3 bottomLeft = transform.TransformPoint(new Vector3(left, bottom));
		Vector3 topRight = transform.TransformPoint(new Vector3(right, top));
		boundingRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
		
	}

	// Use this for initialization
	void Start () {
		ConstructGrid();
		GetComponent<MeshRenderer>().enabled = false;
		
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (VectorLine line in lines){
			line.drawTransform = transform;
			line.lineWidth = Editor.singleton.GetLinePencilLightWidth();
			line.textureScale = Editor.singleton.textureScale;
			line.material = isDotted ? Editor.singleton.pencilLineDotted : Editor.singleton.pencilLine;
			line.Draw3D();
		}
		
	
	}
}
