using UnityEngine;
using System.Collections;

public class WandFlash : MonoBehaviour {

	public ParticleSystem wandFlash;

	//private bool isTriggered = false;

	// Use this for initialization
	void Start () {
		//if(player == null) player = GameObject.Find ("Player");
		this.gameObject.GetComponentInParent<InteractionManager>().trigger_cooldown_time = 1.4f;
		this.gameObject.GetComponentInParent<InteractionManager> ().range_factor = 1.2f;
	}

	// Update is called once per frame
	void Update () {
		/*if (!isTriggered && Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			//Debug.Log ("WandFlash:-- Clicked");
			float distToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
			//Debug.Log ("WandFlash:-- DistToPlayer = " + distToPlayer);
			if (distToPlayer < LanternManager.lanternRange * 1.2) { // increased range because odd shape
				//Debug.Log ("WandFlash:-- Triggered");
				isTriggered = true;
				StartCoroutine ("triggerCountdown");
				wandFlash.GetComponent<ParticleSystem> ().Play ();
				this.transform.parent.GetComponent<Rigidbody>().AddForce(1800 * Vector3.up);
				this.transform.parent.GetComponent<Rigidbody>().AddTorque(100000 * Vector3.right);

			}
		}*/
	}
	void TriggerInteraction() {
		wandFlash.GetComponent<ParticleSystem> ().Play ();
		this.transform.parent.GetComponent<Rigidbody>().AddForce(1800 * Vector3.up);
		this.transform.parent.GetComponent<Rigidbody>().AddTorque(100000 * Vector3.right);
	}

}
