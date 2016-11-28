using UnityEngine;
using System.Collections;

public class SwanAnimator : MonoBehaviour {

	public GameObject player;
	bool triggered = false;
	// Use this for initialization
	void Start () {
		if(player == null) player = GameObject.Find ("Player");

		GetComponent<Animation>().Play();
		//GetComponent<Animator> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!triggered && Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			if (Vector3.Distance (player.transform.position, transform.position) < LanternManager.lanternRange) {

				//GetComponent<Animator> ().enabled = true;
				Debug.Log("SWAN:" + GetComponent<Animation>().name);
				GetComponent<Animation>().Play();
			}
		}
	}
}
