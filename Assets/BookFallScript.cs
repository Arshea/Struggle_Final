using UnityEngine;
using System.Collections;

public class BookFallScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TriggerBookFall() {
		Debug.Log ("Book falling");
		GetComponent<Animator> ().SetTrigger ("IdleFall");

	}
}
