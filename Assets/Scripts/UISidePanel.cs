using UnityEngine;
using System.Collections;

public class UISidePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<RectTransform>().anchorMax = new Vector2(Camera.main.rect.xMin, Camera.main.rect.height);
		GetComponent<RectTransform>().offsetMax = Vector2.zero;
		GetComponent<RectTransform>().offsetMin = Vector2.zero;
	}
}
