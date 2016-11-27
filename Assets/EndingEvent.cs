using UnityEngine;
using System.Collections;


// This is triggered from PickUpLight when GameManager returns the correct progress state
public class EndingEvent : MonoBehaviour {

	private Transform[] contents;


	// Use this for initialization
	void Start () {
		contents = transform.GetComponentsInChildren<Transform> ();
		foreach (Transform o in contents) {
			if(o.name != "Ending Event")
				o.gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void triggerEnding() {
		foreach (Transform o in contents) {
			o.gameObject.SetActive (true);
		}
	}
}
