using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Vectrosity;

public class EditorUI : MonoBehaviour {
	public static EditorUI singleton = null;
	public GameObject buttonFrame;
	public GameObject gridGO;
	public float placementPointRadius;
	public float moduleRadius;
	public bool isInsideGrid = false;
	public GridPaper.PlacementPoint currentPlacement = null;
	public GameObject cursorPicture;
	
	int cursorSpoke = -1;
	
	bool hasBotChanged = true;
	List<VectorLine>	validGridLines = new List<VectorLine>();
	
	Dictionary<Guid, GameObject> botDrawing = new Dictionary<Guid, GameObject>();

	
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
		//Vector3 offset = new Vector3(0, 0, -0.01f);
		validGridLines.Clear ();
		if (editorBot.rootModule != null){
			foreach (GridPaper.PlacementPoint point in gridGO.GetComponent<GridPaper>().placementPoints){
				// Go round three of the spokes
				for (int i = 0; i < 3; ++i){
					GridPaper.PlacementPoint otherPoint = point.neighbouringPoints[i];
					if (otherPoint != null){
						if ((point.picture != null && otherPoint.picture == null) ||
						    (point.picture == null  && otherPoint.picture != null))
						{
							Vector3[] points = new Vector3[2];
							points[0] = point.pos;
							points[1] = otherPoint.pos;
							
							VectorLine newLine = new VectorLine("Editor grid line", points, Editor.singleton.pencilLineDotted, Editor.singleton.GetLinePencilLightWidth());
							newLine.textureScale =  Editor.singleton.textureScale;
							validGridLines.Add(newLine);
						}
					}
				}
			}
		}
		
	}
	
	void DrawBot(){
		hasBotChanged = false;
		
		if (editorBot.rootModule == null) return;
		
		
		Queue<Module> moduleQueue = new Queue<Module>();
		Queue<GridPaper.PlacementPoint> placementQueue = new Queue<GridPaper.PlacementPoint>();
		
		moduleQueue.Enqueue(editorBot.rootModule);
		placementQueue.Enqueue(botPoint);
		
		while (moduleQueue.Count() != 0){
			Module thisModule = moduleQueue.Dequeue();
			GridPaper.PlacementPoint thisPlacement = placementQueue.Dequeue();
			
			// Add any children to the queue (don't include the parent)
			for (int i = 0; i < 6; ++i){
				if (i != thisModule.parentSpoke){
					if (thisModule.modules[i] != null){
						moduleQueue.Enqueue(thisModule.modules[i]);
						placementQueue.Enqueue(thisPlacement.neighbouringPoints[i]);
					}
				}
				
			}
			
			HandleModuleDraw(thisModule, thisPlacement);
		}
			
		
		if (hasBotChanged){
			DrawValidGridLines();
		}
		
	}
	
	
	void HandleModuleDraw(Module module, GridPaper.PlacementPoint point){
		if (module == null) return;
		
		// If the flag is dirty and we have a representation, then remove the representation
		if (module.dirtyFlag[(int)Module.DirtyFlag.kEditor] && botDrawing.ContainsKey(module.repId[(int)Module.DirtyFlag.kEditor])){
			GameObject.Destroy (botDrawing[module.repId[(int)Module.DirtyFlag.kEditor]]);
			module.repId[(int)Module.DirtyFlag.kEditor] = Guid.Empty;
		}
		// If we don't have a representation, then we need to make one
		if (!botDrawing.ContainsKey(module.repId[(int)Module.DirtyFlag.kEditor])){
			
			GameObject newPicture = EditorFactory.singleton.ConstructEditorPicture(module);
			module.repId[(int)Module.DirtyFlag.kEditor] = newPicture.GetComponent<EditorModulePicture>().guid;
			newPicture.transform.SetParent(transform);
			newPicture.transform.position = point.pos;
			newPicture.transform.localScale = new Vector3(moduleRadius, moduleRadius, 1);
			newPicture.GetComponent<EditorModulePicture>().dataGuid = module.guid;
			
			newPicture.GetComponent<EditorModulePicture>().SetSpoke(module.parentSpoke, gridGO.GetComponent<GridPaper>().GetSeperation());
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
					cursorPicture.transform.localScale = new Vector3(moduleRadius, moduleRadius, 1);
					cursorPicture.transform.position = currentPlacement.pos;
					cursorPicture.GetComponent<EditorModulePicture>().moduleType = activeModuleButtonGO.GetComponent<EditorButton>().moduleType;
					cursorPicture.GetComponent<EditorModulePicture>().textColor = Editor.singleton.textOverColor ;
					cursorPicture.GetComponent<EditorModulePicture>().lineWidth = Editor.singleton.pencilLineWidthLight;
				}
				cursorPicture.transform.position = currentPlacement.pos;
				if (Input.GetMouseButtonDown(0)){
					editorBot.rootModule = EditorFactory.singleton.ConstructModule(activeModuleButtonGO.GetComponent<EditorButton>().moduleType, editorBot);
					botPoint = currentPlacement;
					gridGO.GetComponent<GridPaper>().isVisible = false;
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
		TestForPlacementIntersection();
		//Debug.Log ("cursorSpoke = " + cursorSpoke);
		
		// The the pictures should be visible
		foreach (GridPaper.PlacementPoint point in gridGO.GetComponent<GridPaper>().placementPoints){
			if (point.picture != null){
				point.picture.GetComponent<EditorModulePicture>().moduleVisible = true;
			}
			
		}
		if (isInsideGrid){
			Ray mouseRay = Editor.singleton.editorCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			Plane uiPlane = new Plane(new Vector3(0, 0, 1), 0);
			float dist;
			if (!uiPlane.Raycast(mouseRay, out dist)){
				return;
			}
			Vector3 mousePos = mouseRay.GetPoint(dist);
				
				
			if (currentPlacement != null){
				if (cursorPicture != null && (cursorPicture.GetComponent<EditorModulePicture>().moduleType != activeModuleButtonGO.GetComponent<EditorButton>().moduleType || (cursorPicture.transform.position - currentPlacement.pos).sqrMagnitude > 0.5f)){
					GameObject.Destroy(cursorPicture);
					cursorPicture = null;
				}
				// Where should the chassis rod be?
				cursorSpoke = -1;
				float minDistToOtherSq = 100;
				for (int i = 0; i < 6; ++i){
					GridPaper.PlacementPoint otherPoint = currentPlacement.neighbouringPoints[i];
					if (otherPoint != null && otherPoint.picture != null){
						float distToOtherSq = (otherPoint.pos - mousePos).sqrMagnitude;
						if (distToOtherSq < minDistToOtherSq){
							minDistToOtherSq = distToOtherSq;
							cursorSpoke = i;
						}
					}
				}
				if (cursorPicture == null){
					// If we have a place for the rod and we don;t have a picture, hen make one

					if (cursorSpoke != -1){
						cursorPicture = GameObject.Instantiate(EditorFactory.singleton.modulePicturePrefab);
						cursorPicture.transform.SetParent(transform);
						cursorPicture.transform.localScale = new Vector3(moduleRadius, moduleRadius, 1);
						cursorPicture.transform.position = currentPlacement.pos;
						cursorPicture.GetComponent<EditorModulePicture>().moduleType = activeModuleButtonGO.GetComponent<EditorButton>().moduleType;
						cursorPicture.GetComponent<EditorModulePicture>().textColor = Editor.singleton.textOverColor ;
						cursorPicture.GetComponent<EditorModulePicture>().lineWidth = Editor.singleton.pencilLineWidthLight;
					}

					
					
				}
				if (cursorPicture != null){
					// if we are over an already constructed module, then make it one invisible
					if (currentPlacement.picture != null){
						currentPlacement.picture.GetComponent<EditorModulePicture>().moduleVisible = false;
					}
					cursorPicture.GetComponent<EditorModulePicture>().SetSpoke(cursorSpoke, gridGO.GetComponent<GridPaper>().GetSeperation());
				}
				if (cursorPicture != null){
					cursorPicture.transform.position = currentPlacement.pos;
					if (Input.GetMouseButtonDown(0)){
					
						// Find the parent Module - by first finding the other placementpoint
						GridPaper.PlacementPoint parentPoint = currentPlacement.neighbouringPoints[cursorSpoke];

						Guid parentModuleGuid = parentPoint.picture.GetComponent<EditorModulePicture>().dataGuid;
						Module parentModule = editorBot.FindModule(parentModuleGuid);
						int parentSpoke = SpokeDirs.CalcInverseSpoke(cursorSpoke);
						
						EditorFactory.singleton.ConstructModule(activeModuleButtonGO.GetComponent<EditorButton>().moduleType, parentModule, parentSpoke);

						GameObject.Destroy(cursorPicture);
						cursorPicture = null;
					}
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
		}	
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
