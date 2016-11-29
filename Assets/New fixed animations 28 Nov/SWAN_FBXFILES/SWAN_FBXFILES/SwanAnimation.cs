using UnityEngine;
using System.Collections;

public class SwanAnimation : MonoBehaviour {

	public float flightTime = 2.15f;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = flightTime;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TriggerInteraction(){
		GetComponent<Animator> ().SetTrigger ("IdleFly");
		StartCoroutine ("swanFlying");
	}

	IEnumerator swanFlying() {
		yield return new WaitForSeconds (2.0f);
		//Debug.Log ("SwanAnimation:: Going back to the ground thanks I had fun");
		GetComponent<Animator> ().SetTrigger ("FlyIdle");
	}
}
