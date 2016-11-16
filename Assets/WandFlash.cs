using UnityEngine;
using System.Collections;

public class WandFlash : MonoBehaviour {

	public GameObject player;
	public ParticleSystem wandFlash;

	private bool isTriggered = false;

	// Use this for initialization
	void Start () {
		if(player == null) player = GameObject.Find ("Player");
	}

	// Update is called once per frame
	void Update () {
		if (!isTriggered && Input.GetButtonDown ("Interact") && LanternManager.ammunition > 0) {
			//Debug.Log ("WandFlash:-- Clicked");
			float distToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
			//Debug.Log ("WandFlash:-- DistToPlayer = " + distToPlayer);
			if (distToPlayer < LanternManager.lanternRange * 1.2) { /* increased range because odd shape*/
				//Debug.Log ("WandFlash:-- Triggered");
				isTriggered = true;
				StartCoroutine ("triggerCountdown");
				wandFlash.GetComponent<ParticleSystem> ().Play ();
				this.transform.parent.GetComponent<Rigidbody>().AddForce(1800 * Vector3.up);
				this.transform.parent.GetComponent<Rigidbody>().AddTorque(100000 * Vector3.right);

			}
		}
	}

	IEnumerator triggerCountdown() {
		yield return new WaitForSeconds (1.4f);
		isTriggered = false;
	}
}
