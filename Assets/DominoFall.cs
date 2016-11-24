using UnityEngine;
using System.Collections;

public class DominoFall : MonoBehaviour {

	public GameObject player;
	public ParticleSystem greenBeacon; // Turn off spiral after domino animation (else will be at angle)


	// Use this for initialization
	void Start () {
		//if(player == null) player = GameObject.Find ("Player");
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 12.458f;


	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			float distToPlayer = Vector3.Distance (player.transform.position, dominoTrigger.transform.position);
			if (distToPlayer < LanternManager.lanternRange) {
				FallDown ();
			}
		}*/
	}

	void TriggerInteraction() {
		Debug.Log ("Hit a domino!");
		this.gameObject.GetComponentInParent<Animator> ().SetTrigger ("FallTrigger");
		// Turn off beacon for green
		var temp = greenBeacon.emission;
		temp.enabled = false;
	}
}
