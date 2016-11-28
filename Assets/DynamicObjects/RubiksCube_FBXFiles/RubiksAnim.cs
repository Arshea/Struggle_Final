using UnityEngine;
using System.Collections;

public class RubiksAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TriggerInteraction(){
		GetComponent<Animator> ().SetTrigger ("IdleSolve");
	}
}
