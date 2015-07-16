using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorUI : MonoBehaviour {
	public static EditorUI singleton = null;
	public GameObject buttonFrame;
	public GameObject gridGO;
	public float placementPointRadius;
	public bool hasCurrentPlacementPos = false;
	public Vector3 currentPlacementPos = Vector3.zero;
	public GameObject cursorPicture;
	
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
			float placementPointRadiusSQ = placementPointRadius * placementPointRadius;
			currentPlacementPos = Vector3.zero;
			hasCurrentPlacementPos = false;
			foreach (Vector3 point in gridGO.GetComponent<GridPaper>().placementPoints){
				if ((point - mousePos).sqrMagnitude < placementPointRadiusSQ){
					currentPlacementPos = point;
					hasCurrentPlacementPos = true;
				}
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
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
		TestForPlacementIntersection();
		if (hasCurrentPlacementPos){
			if (cursorPicture != null && cursorPicture.GetComponent<EditorModulePicture>().moduleType != activeModuleButtonGO.GetComponent<EditorButton>().moduleType){
				GameObject.Destroy(cursorPicture);
				cursorPicture = null;
			}
			if (cursorPicture == null){
				cursorPicture = GameObject.Instantiate(EditorFactory.singleton.modulePicturePrefab);
				cursorPicture.transform.SetParent(transform);
				cursorPicture.transform.localScale = new Vector3(placementPointRadius, placementPointRadius, 1);
				cursorPicture.transform.position = currentPlacementPos;
				cursorPicture.GetComponent<EditorModulePicture>().moduleType = activeModuleButtonGO.GetComponent<EditorButton>().moduleType;
				cursorPicture.GetComponent<EditorModulePicture>().textColor = Editor.singleton.textOverColor;
				cursorPicture.GetComponent<EditorModulePicture>().lineWidth = Editor.singleton.pencilLineWidthLight;
			}
			cursorPicture.transform.position = currentPlacementPos;
		}
		else{
			if (cursorPicture != null){
				GameObject.Destroy(cursorPicture);
				cursorPicture = null;
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
