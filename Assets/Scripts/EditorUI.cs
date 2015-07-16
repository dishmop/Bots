using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

public class EditorUI : MonoBehaviour {
	public static EditorUI singleton = null;
	public GameObject buttonFrame;
	public GameObject gridGO;
	public float placementPointRadius;
	public bool isInsideGrid = false;
	public GridPaper.PlacementPoint currentPlacement = null;
	public GameObject cursorPicture;
	
	bool hasBotChanged = true;
	List<VectorLine>	validGridLines = new List<VectorLine>();
	
	Dictionary<int, GameObject> botDrawing = new Dictionary<int, GameObject>();
	int nextRepId = 100;
	
	Bot	editorBot = new Bot();
	GridPaper.PlacementPoint botPoint = null;
	
	GameObject activeModuleButtonGO;
	float buttonBoarderPerc = 0.1f;
	GameObject[] moduleButtons = new GameObject[(int)ModuleType.kNumTypes];
	
	// Use this for initialization
	void Start () {
	
		Renderer rend = buttonFrame.GetComponent<Renderer>();
		float left = 	rend.bounds.min.x;
		float right = 	rend.bounds.max.x;
		float bottom = 	rend.bounds.min.y;
		float top = 	rend.bounds.max.y;
		float width = 	right - left;
		float height = 	top - bottom;
		

		
		float buttonStride = Mathf.Min (width, height / (float)ModuleType.kNumTypes);
		float buttonSize = buttonStride / (1 + buttonBoarderPerc);
		
		Vector3 startPos = new Vector3(0.5f * (left + right), top - 0.5f * buttonStride, 0);
		
		for (int  i = 0; i < (int)ModuleType.kNumTypes; ++i){
			GameObject newButton = GameObject.Instantiate(EditorFactory.singleton.moduleButtonPrefab);
			newButton.transform.SetParent(transform);
			newButton.transform.position = startPos - new Vector3(0, i * buttonStride, 0);
			newButton.transform.localScale = new Vector3(buttonSize, buttonSize, 1);
			newButton.GetComponent<EditorButton>().moduleType = (ModuleType)i;
			moduleButtons[i] = newButton;
		}
		
		activeModuleButtonGO = moduleButtons[0];
		activeModuleButtonGO.GetComponent<EditorButton>().state = EditorButton.State.kActive;
		
		buttonFrame.GetComponent<MeshRenderer>().enabled = false;
	}
	
	void TestForPlacementIntersection(){
		Ray mouseRay = Editor.singleton.editorCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		Plane uiPlane = new Plane(new Vector3(0, 0, 1), 0);
		float dist;
		if (uiPlane.Raycast(mouseRay, out dist)){
			Vector3 mousePos = mouseRay.GetPoint(dist);
			
			// Test if we are inside the grid zone
			isInsideGrid = gridGO.GetComponent<GridPaper>().boundingRect.Contains(mousePos);
			
			currentPlacement = null;
			if (isInsideGrid){
				// Test if we hit any of the placement points
				float placementPointRadiusSQ = placementPointRadius * placementPointRadius;
				foreach (GridPaper.PlacementPoint point in gridGO.GetComponent<GridPaper>().placementPoints){
					if ((point.pos - mousePos).sqrMagnitude < placementPointRadiusSQ){
						currentPlacement = point;						
					}
				}
			}
			
			
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
		HandleButtonInput();
		
		// if we haven't placed anything yet
		if (editorBot.rootModule == null){
			HandleEmptyBotInput();
		}
		else{
			HandleNonEmptyBotInput();
		}
		
		DrawBot();
		UpdateGridLines();
		
	}
	
	void UpdateGridLines(){
		foreach (VectorLine line in validGridLines){
			line.drawTransform = transform;
			line.lineWidth = Editor.singleton.GetLinePencilLightWidth();
			line.textureScale = Editor.singleton.textureScale;
			line.Draw3D();
		}
	}
	
	
	void DrawValidGridLines(){
		for (int i = 0; i < validGridLines.Count(); ++i){
			VectorLine line = validGridLines[i];
			VectorLine.Destroy(ref line);
		}
		validGridLines.Clear ();
		if (editorBot.rootModule != null){
			foreach (GridPaper.PlacementPoint point in gridGO.GetComponent<GridPaper>().placementPoints){
				// Go round three of the spokes
				for (int i = 0; i < 3; ++i){
					GridPaper.PlacementPoint otherPoint = point.neighbouringPoints[i];
					if (point.picture == null && otherPoint != null && otherPoint.picture == null){
						Vector3[] points = new Vector3[2];
						points[0] = point.pos;
						points[1] = otherPoint.pos;
						
						VectorLine newLine = gridGO.GetComponent<GridPaper>().ConstructGridLine(points);
						validGridLines.Add(newLine);
					}
				}
			}
		}
		
	}
	
	void DrawBot(){
		hasBotChanged = false;
		HandleModuleDraw(editorBot.rootModule, botPoint);
		if (hasBotChanged){
			DrawValidGridLines();
		}
		
	}
	
	int GetNextRepId(){
		return nextRepId++;
	}
	
	void HandleModuleDraw(Module module, GridPaper.PlacementPoint point){
		if (module == null) return;
		
		// If the flag is dirty and we have a representation, then remove the representation
		if (module.dirtyFlag[(int)Module.DirtyFlag.kEditor] && botDrawing.ContainsKey(module.repId[(int)Module.DirtyFlag.kEditor])){
			GameObject.Destroy (botDrawing[module.repId[(int)Module.DirtyFlag.kEditor]]);
			module.repId[(int)Module.DirtyFlag.kEditor] = -1;
		}
		// If we don't have a representation, then we need to make one
		if (!botDrawing.ContainsKey(module.repId[(int)Module.DirtyFlag.kEditor])){
			module.repId[(int)Module.DirtyFlag.kEditor] = GetNextRepId();
			GameObject newPicture = EditorFactory.singleton.ConstructEditorPicture(editorBot.rootModule);
			newPicture.transform.SetParent(transform);
			newPicture.transform.position = botPoint.pos;
			newPicture.transform.localScale = 2f * new Vector3(placementPointRadius, placementPointRadius, 1);;
			module.dirtyFlag[(int)Module.DirtyFlag.kEditor] = false;
			botDrawing.Add(module.repId[(int)Module.DirtyFlag.kEditor], newPicture);
			hasBotChanged = true;
			point.picture = newPicture;
		}
	}
	
	void HandleEmptyBotInput(){

		TestForPlacementIntersection();
		if (isInsideGrid){
			if (currentPlacement != null){
				if (cursorPicture != null && cursorPicture.GetComponent<EditorModulePicture>().moduleType != activeModuleButtonGO.GetComponent<EditorButton>().moduleType){
					GameObject.Destroy(cursorPicture);
					cursorPicture = null;
				}
				if (cursorPicture == null){
					cursorPicture = GameObject.Instantiate(EditorFactory.singleton.modulePicturePrefab);
					cursorPicture.transform.SetParent(transform);
					cursorPicture.transform.localScale = 2f * new Vector3(placementPointRadius, placementPointRadius, 1);
					cursorPicture.transform.position = currentPlacement.pos;
					cursorPicture.GetComponent<EditorModulePicture>().moduleType = activeModuleButtonGO.GetComponent<EditorButton>().moduleType;
					cursorPicture.GetComponent<EditorModulePicture>().textColor = Editor.singleton.textOverColor;
					cursorPicture.GetComponent<EditorModulePicture>().lineWidth = Editor.singleton.pencilLineWidthLight;
				}
				cursorPicture.transform.position = currentPlacement.pos;
				if (Input.GetMouseButtonDown(0)){
					editorBot.rootModule = EditorFactory.singleton.ConstructModule(activeModuleButtonGO.GetComponent<EditorButton>().moduleType);
					botPoint = currentPlacement;
					gridGO.GetComponent<GridPaper>().isDotted = true;
					GameObject.Destroy(cursorPicture);
					cursorPicture = null;
				}
			}
		}
		else{
			if (cursorPicture != null){
				GameObject.Destroy(cursorPicture);
				cursorPicture = null;
			}
		}
	}
	
	void HandleNonEmptyBotInput(){

	}
	
	void HandleButtonInput(){
		Ray mouseRay = Editor.singleton.editorCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		
		int layerMask = 1 << 5;
		RaycastHit raycastHit;
		foreach (GameObject go in moduleButtons){
			if (go.GetComponent<EditorButton>().state == EditorButton.State.kOver){
				go.GetComponent<EditorButton>().state = EditorButton.State.kNormal;
			}
		}
		if (Physics.Raycast(mouseRay, out raycastHit, 20, layerMask)){
			EditorButton button = raycastHit.collider.gameObject.GetComponent<EditorButton>();
			if (button){
				if (button.state != EditorButton.State.kActive){
					button.state = EditorButton.State.kOver;
				}
				if (Input.GetMouseButtonDown(0)){
					foreach (GameObject go in moduleButtons){
						go.GetComponent<EditorButton>().state = EditorButton.State.kNormal;
					}
					button.state = EditorButton.State.kActive;
					activeModuleButtonGO = raycastHit.collider.gameObject;
				}
			}
		}	}
	
	void Awake(){
		// Singleton
		if (singleton != null) Debug.LogError ("Error assigning singleton");
		singleton = this;
		

	}
	
	
	
	void OnDestroy(){
		singleton = null;
	}
}
