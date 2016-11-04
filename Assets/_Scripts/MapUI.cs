using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour {


    public GameObject mapCanvas;

	private bool showMap;

	// Use this for initialization
	void Start () {
        showMap = false;
		mapCanvas.SetActive (showMap);

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Map"))
        {
            showMap = !showMap;
            mapCanvas.SetActive(showMap);
		
        }
	}

	void LateUpdate () {
		
	}
}
